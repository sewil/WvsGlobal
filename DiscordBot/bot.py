import mariadb
from typing import Literal
import discord
import os
from discord import app_commands
import re
import mysql.connector
from dotenv import load_dotenv
import bcrypt
from enum import Enum
from utils import get_gm_level

# class syntax
class GMLevel(Enum):
    Tester = 16
    GMIntern = 32
    GM = 64
    Admin = 128

# Load environment variables from .env file
load_dotenv()

TOKEN = os.getenv("DISCORD_TOKEN")
SERVER_ID = os.getenv("DISCORD_SERVER_ID")
MIN_GM_LEVEL = int(os.getenv("MIN_GM_LEVEL") or 0)

DB_HOST = os.getenv("DB_HOST")
DB_NAME = os.getenv("DB_NAME")
DB_USER = os.getenv("DB_USER")
DB_PASS = os.getenv("DB_PASS")

RSVP_DB_HOST = os.getenv("RSVP_DB_HOST")
RSVP_DB_NAME = os.getenv("RSVP_DB_NAME")
RSVP_DB_USER = os.getenv("RSVP_DB_USER")
RSVP_DB_PASS = os.getenv("RSVP_DB_PASS")

class RegisterModal(discord.ui.Modal, title="Register"):
    server: str
    username = discord.ui.TextInput(label="Username", placeholder="Manji")
    dob = discord.ui.TextInput(label="Date of birth (For char deletion)", placeholder="YYYY-MM-DD")
    password = discord.ui.TextInput(label="Password", placeholder="*****", min_length=4, max_length=12)
    password2 = discord.ui.TextInput(label="Password (again)", placeholder="*****", min_length=4, max_length=12)

    def __init__(self, server: str):
        super().__init__()
        self.server = server
        self.title = f"Register ({server})"

    async def on_submit(self, interaction: discord.Interaction):
        message = f'Something went wrong!'
        if self.password.value != self.password2.value:
            message = "Mismatching passwords!"
        elif re.match(r"^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$", self.dob.value) == None:
            message = "Invalid date of birth!"
        else:
            message = db_register(interaction, self)

        # Handle the form submission
        await interaction.response.send_message(
            message,
            ephemeral=True
        )


class ChangePasswordModal(discord.ui.Modal, title="Change password"):
    server: str
    username = discord.ui.TextInput(label="Username", placeholder="Manji")
    current_password = discord.ui.TextInput(label="Current password", placeholder="*****")
    new_password = discord.ui.TextInput(label="New password", placeholder="*****", min_length=4, max_length=12)
    new_password2 = discord.ui.TextInput(label="New password (again)", placeholder="*****", min_length=4, max_length=12)

    def __init__(self, server: str):
        super().__init__()
        self.server = server
        self.title = f"Change password ({server})"

    async def on_submit(self, interaction: discord.Interaction):
        message = f'Something went wrong!'
        if self.new_password.value != self.new_password2.value:
            message = "Mismatching passwords!"
        elif self.current_password.value == self.new_password.value:
            message = "Can't change to the same password!"
        else:
            message = db_change_password(interaction, self)

        # Handle the form submission
        await interaction.response.send_message(
            message,
            ephemeral=True
        )

def db_connect(form):
    if form.server == "rsvp":
        cnx = mariadb.connect(
            user=RSVP_DB_USER,
            password=RSVP_DB_PASS,
            host=RSVP_DB_HOST,
            database=RSVP_DB_NAME,
            port=3306
        )
        cur = cnx.cursor()
        return (cnx, cur)
    else:
        host = DB_HOST
        user = DB_USER
        pw = DB_PASS
        name = DB_NAME
        cnx = mysql.connector.connect(
            host=host,
            port=3306,
            user=user,
            password=pw,
            database=name,
        )

        cur = cnx.cursor()

        return (cnx, cur)

def db_register(interaction: discord.Interaction, form: RegisterModal):
    try:
        user_id = interaction.user.id
        is_rsvp = form.server == "rsvp"
        dob_formatted = int(form.dob.value.replace("-", ""))
        hashed_password = bcrypt.hashpw(form.password.value.encode(), bcrypt.gensalt(13 if is_rsvp else 10, prefix=b'2a'))

        if is_rsvp:
            insert_query = "INSERT INTO users (username, password, email, gender, admin, char_delete_password) VALUES (%s, %s, %s, %s, %s, %s)"
            query_params = (form.username.value, hashed_password, user_id, 10, 0, dob_formatted)
        else:
            member: discord.Member = interaction.user
            gm_level = get_gm_level(member.roles)
            insert_query = "INSERT INTO users (username, discord_id, password, email, gender, gm, char_delete_password) VALUES (%s, %s, %s, %s, %s, %s, %s)"
            query_params = (form.username.value, user_id, hashed_password, '', 10, gm_level, dob_formatted)

        (cnx, cur) = db_connect(form)

        # Check username/discord_id already exists
        check_query = f"""
            SELECT COUNT(*) FROM users
            WHERE LOWER(username) = LOWER(%s) OR LOWER({"email" if is_rsvp else "discord_id"}) = LOWER(%s)
        """
        cur.execute(check_query, (form.username.value, user_id))
        if cur.fetchone()[0] > 0:
            return "This user is already registered!"

        cur.execute(insert_query, query_params)
        cnx.commit()

        return f'Welcome {form.username}!'
    except mysql.connector.Error as e:
        print(f"Database error occurred: {e}")
        return 'An unknown error occurred, please try again later!'
    finally:
        if 'cur' in locals(): cur.close()
        if 'cnx' in locals(): cnx.close()

def db_change_password(interaction: discord.Interaction, form: ChangePasswordModal):
    try:
        user_id = interaction.user.id

        (cnx, cur) = db_connect(form)

        is_rsvp = form.server == "rsvp"

        # Check username/discord_id combination exists
        check_query = f"""
            SELECT * FROM users
            WHERE LOWER(username) = LOWER(%s) AND LOWER({"email" if is_rsvp else "discord_id"}) = LOWER(%s)
        """
        cur.execute(check_query, (form.username.value, user_id))
        user = cur.fetchone()
        if user == None:
            return "User not found! Make sure to use the same Discord account that you registered with."

        db_pass = user[2 if is_rsvp else 3].encode()
        hashed_password = bcrypt.hashpw(form.current_password.value.encode(), db_pass)
        if hashed_password != db_pass:
            return 'Wrong password!'

        hashed_new_password = bcrypt.hashpw(form.new_password.value.encode(), bcrypt.gensalt(13 if is_rsvp else 10, prefix=b'2a'))

        insert_query = "UPDATE users SET password=%s WHERE ID=%s"
        cur.execute(insert_query, (hashed_new_password, user[0]))
        cnx.commit()

        return f'Password changed!'
    except mysql.connector.Error as e:
        print(f"Database error occurred: {e}")
        return 'An unknown error occurred, please try again later!'
    finally:
        if 'cur' in locals(): cur.close()
        if 'cnx' in locals(): cnx.close()

# Initialize the bot
intents = discord.Intents.default()
client = discord.Client(intents=intents)
tree = app_commands.CommandTree(client)
guild = discord.Object(id=SERVER_ID)

@tree.command(name="register", description="Opens a registration modal.", guild=guild)
async def register(interaction: discord.Interaction, server: Literal["rsvp", "openmg"]):
    # Instantiate the modal
    modal = RegisterModal(server)
    # Get GM Level
    member: discord.Member = interaction.user
    gm_level = get_gm_level(member.roles)
    if gm_level < MIN_GM_LEVEL:
        await interaction.response.send_message(
            'Missing required role(s) to do this!',
            ephemeral=True
        )
    else:
        # Show the modal to the user
        await interaction.response.send_modal(modal)

@tree.command(name="change_password", description="Opens a change password modal.", guild=guild)
async def changePassword(interaction: discord.Interaction, server: Literal["rsvp", "openmg"]):
    modal = ChangePasswordModal(server)
    await interaction.response.send_modal(modal)

@client.event
async def on_ready():
    await tree.sync(guild=guild)
    print("Ready!")

# Start the bot
client.run(TOKEN)
