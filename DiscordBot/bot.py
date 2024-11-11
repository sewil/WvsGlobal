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

class RegisterModal(discord.ui.Modal, title="Register"):
    # Define form fields
    username = discord.ui.TextInput(label="Username", placeholder="Manji")
    dob = discord.ui.TextInput(label="Date of birth (For char deletion)", placeholder="YYYY-MM-DD")
    password = discord.ui.TextInput(label="Password", placeholder="*****", min_length=4, max_length=12)
    password2 = discord.ui.TextInput(label="Password (again)", placeholder="*****", min_length=4, max_length=12)

    async def on_submit(self, interaction: discord.Interaction):
        message = f'Something went wrong!'
        if self.password.value != self.password2.value:
            message = "Mismatching passwords!"
        elif re.match(r"^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$", self.dob.value) == None:
            message = "Invalid date of birth!"
        else:
            # Get GM Level
            member: discord.Member = interaction.user
            gm_level = get_gm_level(member.roles)
            message = db_register(interaction, self, gm_level)

        # Handle the form submission
        await interaction.response.send_message(
            message,
            ephemeral=True
        )


class ChangePasswordModal(discord.ui.Modal, title="Change password"):
    # Define form fields
    username = discord.ui.TextInput(label="Username", placeholder="Manji")
    current_password = discord.ui.TextInput(label="Current password", placeholder="*****")
    new_password = discord.ui.TextInput(label="New password", placeholder="*****", min_length=4, max_length=12)
    new_password2 = discord.ui.TextInput(label="New password (again)", placeholder="*****", min_length=4, max_length=12)

    async def on_submit(self, interaction: discord.Interaction):
        message = f'Something went wrong!'
        user_id = interaction.user.id
        if self.new_password.value != self.new_password2.value:
            message = "Mismatching passwords!"
        elif self.current_password.value == self.new_password.value:
            message = "Can't change to the same password!"
        else:
            message = db_change_password(user_id, self)

        # Handle the form submission
        await interaction.response.send_message(
            message,
            ephemeral=True
        )

def db_connect():
    cnx = mysql.connector.connect(
        host=DB_HOST,
        port=3306,
        user=DB_USER,
        password=DB_PASS,
        database=DB_NAME
    )
    cur = cnx.cursor()
    return (cnx, cur)

def db_register(interaction, form, gm_level):
    try:
        (cnx, cur) = db_connect()

        # Check username/discord_id already exists
        user_id = interaction.user.id
        check_query = """
            SELECT COUNT(*) FROM users
            WHERE LOWER(username) = LOWER(%s) OR LOWER(discord_id) = LOWER(%s)
        """
        cur.execute(check_query, (form.username.value, user_id))
        if cur.fetchone()[0] > 0:
            return "This user is already registered!"

        dob_formatted = int(form.dob.value.replace("-", ""))
        hashed_password = bcrypt.hashpw(form.password.value.encode(), bcrypt.gensalt(10, prefix=b'2a'))

        insert_query = "INSERT INTO users (username, discord_id, password, email, gender, gm, char_delete_password) VALUES (%s, %s, %s, %s, %s, %s, %s)"
        cur.execute(insert_query, (form.username.value, user_id, hashed_password, '', 10, gm_level, dob_formatted))
        cnx.commit()

        return f'Welcome {form.username}!'
    except mysql.connector.Error as e:
        print(f"Database error occurred: {e}")
        return 'An unknown error occurred, please try again later!'
    finally:
        if 'cur' in locals(): cur.close()
        if 'cnx' in locals(): cnx.close()

def db_change_password(user_id, form):
    try:
        (cnx, cur) = db_connect()

        # Check username/discord_id combination exists
        check_query = """
            SELECT * FROM users
            WHERE LOWER(username) = LOWER(%s) AND LOWER(discord_id) = LOWER(%s)
        """
        cur.execute(check_query, (form.username.value, user_id))
        user = cur.fetchone()
        if user == None:
            return "User not found! Make sure to use the same Discord account that you registered with."

        db_pass = user[3].encode()
        hashed_password = bcrypt.hashpw(form.current_password.value.encode(), db_pass)
        if hashed_password != db_pass:
            return 'Wrong password!'

        hashed_new_password = bcrypt.hashpw(form.new_password.value.encode(), bcrypt.gensalt(10, prefix=b'2a'))

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
async def register(interaction: discord.Interaction):
    # Instantiate the modal
    modal = RegisterModal()
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
async def changePassword(interaction: discord.Interaction):
    modal = ChangePasswordModal()
    await interaction.response.send_modal(modal)

@client.event
async def on_ready():
    await tree.sync(guild=guild)
    print("Ready!")

# Start the bot
client.run(TOKEN)
