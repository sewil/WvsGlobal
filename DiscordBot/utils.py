from typing import List
from discord import Role
import json

# Load roles.json
gm_roles = json.load(open("roles.json"))

def get_gm_level(roles: List[Role]):
    gm_level = 0
    for role in roles:
        gm_role = next((gm_role for gm_role in gm_roles if gm_role['role_id'] == role.id), None)
        if gm_role != None:
            gm_level |= gm_role['gm_level']
    return gm_level
