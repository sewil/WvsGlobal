namespace WvsBeta.Common.Sessions
{
	public enum ServerMessages : byte
    {
        ___START_SOCKET = 0,
        CHECK_PASSWORD_RESULT = 1,
        //Confirm that the server can handle the connection
        CHECK_USER_LIMIT_RESULT = 2,

        //Set Gender result
        SET_ACCOUNT_RESULT = 3,

        //EULA - Not implemented
        CONFIRM_EULA_RESULT = 4,

        // 5 does not exist

        WORLD_INFORMATION = 6,

        //Confusing naming by Nexon, is select channel in selected world
        SELECT_WORLD_RESULT = 7,
        //Confusing naming, basically connect to server header :)
        SELECT_CHARACTER_RESULT = 8,
        CHECK_CHARACTER_NAME_AVAILABLE = 9,
        CREATE_NEW_CHARACTER_RESULT = 10,
        DELETE_CHARACTER_RESULT = 11,

        CHANGE_CHANNEL = 12,
        PING = 13,
        AUTHEN_CODE_CHANGED = 14,
        SECURITY_SOMETHING = 15, // Either read a buffer (passed to GG?) or a set of ints. Looks like CRC info.
        ___END_SOCKET = 16,

        ___START_CHARACTERDATA = 17,
        INVENTORY_OPERATION = 18,
        INVENTORY_GROW = 19,
        STAT_CHANGED = 20,
        FORCED_STAT_SET = 21,
        FORCED_STAT_RESET = 22,
        CHANGE_SKILL_RECORD_RESULT = 23,
        SKILL_USE_RESULT = 24,
        GIVE_POPULARITY_RESULT = 25,
        SHOW_STATUS_INFO = 26, // Called 'Message'
        MEMO_RESULT = 27,
        MAP_TRANSFER_RESULT = 28,
        SUE_CHARACTER_RESULT = 29,
        CHARACTER_INFO = 32,
        PARTY_RESULT = 33,
        FRIEND_RESULT = 34,
        TOWN_PORTAL = 35,
        BROADCAST_MSG = 36,
        ___END_CHARACTEDATA = 37,

        ___START_STAGE = 38,
        SET_FIELD = 39,
        SET_CASH_SHOP = 40,
        ___END_STAGE = 41,

        ___START_FIELD = 42,
        TRANSFER_FIELD_REQ_IGNORED = 43,
        TRANSFER_CHANNEL_REQ_IGNORED = 44,
        FIELD_SPECIFIC_DATA = 45,
        GROUP_MESSAGE = 46,
        WHISPER = 47,
        SUMMON_ITEM_INAVAILABLE = 48,
        FIELD_EFFECT = 49,
        BLOW_WEATHER = 50,
        PLAY_JUKE_BOX = 51,
        ADMIN_RESULT = 52,
        QUIZ = 53,
        DESC = 54,
        CLOCK = 55,
        
        CONTIMOVE = 56,
        CONTISTATE = 57,

        WARN_MESSAGE = 59,
        
        ___START_USERPOOL = 60,
        USER_ENTER_FIELD = 61,
        USER_LEAVE_FIELD = 62,
        
        ___START_USERCOMMON = 63,
        CHAT = 64,
        MINI_ROOM_BALLOON = 65,
        SET_CONSUME_ITEM_EFFECT = 66, // int itemid; See Effect\ItemEff.img\(itemid)

        ___START_PET = 67,
        SPAWN_PET = 68,
        PET_MOVE = 69,
        PET_ACTION = 70,
        PET_NAME_CHANGED = 71,
        PET_INTERACTION = 72,
        ___END_PET = 73,
        
        ___START_SPAWN = 74,
        SPAWN_ENTER_FIELD = 75,
        SPAWN_LEAVE_FIELD = 76,
        SPAWN_MOVE = 77,
        SPAWN_ATTACK = 78,
        SPAWN_HIT = 79,
        ___END_SPAWN = 80,

        ___START_USERREMOTE = 82,
        MOVE_PLAYER = 83,
        CLOSE_RANGE_ATTACK = 84,
        RANGED_ATTACK = 85,
        MAGIC_ATTACK = 86,
        PREPARE_SKILL = 87, // Skills related: 1111008 (Shout), 1311006 (Dragon Roar), 5001006 (Super Dragon Roar)
        SKILL_END = 88,
        DAMAGE_PLAYER = 89, // Called 'Hit'
        FACIAL_EXPRESSION = 90,
        AVATAR_MODIFIED = 91, //Called UPDATE_CHAR_LOOK in odin, new name = gms-like
        SHOW_FOREIGN_EFFECT = 92, // Called 'Effect'
        GIVE_FOREIGN_BUFF = 93,
        RESET_FOREIGN_BUFF = 94,
        UPDATE_PARTYMEMBER_HP = 95,
        ___END_USERREMOTE = 96,

        ___START_USERLOCAL = 97,
        SHOW_CHAIR = 98,
        PLAYER_EFFECT = 99, // CUser::OnEffect, 
        // 100 reads a byte?
        // 101 is missing
        MESOBAG_SUCCEED = 102,
        MESOBAG_FAILED = 103,
        ___END_USERLOCAL = 104,

        ___START_MOBPOOL = 106,
        MOB_ENTER_FIELD = 107,
        MOB_LEAVE_FIELD = 108,
        MOB_CHANGE_CONTROLLER = 109,

        ___START_MOB = 110,
        MOB_MOVE = 111,
        MOB_MOVE_RESPONSE = 112,
        // 113 doesnt exist
        MOB_STAT_SET = 114,
        MOB_STAT_RESET = 115,
        MOB_SUSPEND_RESET = 116,
        MOB_AFFECTED = 117,
        MOB_DAMAGED = 118,
        MOB_EFFECT_BY_SKILL = 119, // int mapmobid, int skillid. Seems to activate a special animation caused by certain skills (e.g. 3210001 mortal blow), mainly those with the 'special' node
        // 120 doesnt exist
        ___END_MOB = 121,

        ___END_MOBPOOL = 122,

        ___START_NPCPOOL = 123,
        NPC_ENTER_FIELD = 124,
        NPC_LEAVE_FIELD = 125,
        NPC_CHANGE_CONTROLLER = 126,
        NPC_SET_SPECIAL_ACTION = 127,
        NPC_ANIMATE = 128,
        ___END_NPCPOOL = 129,
        // 130 doesn't exist
        ___START_DROPPOOL = 131,
        DROP_ENTER_FIELD = 132,
        DROP_LEAVE_FIELD = 133,
        ___END_DROPPOOL = 134,

        ___START_MESSAGEBOXPOOL = 135,
        MESSAGE_BOX_CREATE_FAILED = 136,
        MESSAGE_BOX_ENTER_FIELD = 137,
        MESSAGE_BOX_LEAVE_FIELD = 138,
        ___END_MESSAGEBOXPOOL = 139,

        ___START_AFFECTED_AREA = 140,
        AFFECTED_AREA_CREATED = 141,
        AFFECTED_AREA_REMOVED = 142,
        ___END_AFFECTED_AREA = 143,

        ___START_TOWN_PORTAL = 144,
        TOWN_PORTAL_CREATED = 145,
        TOWN_PORTAL_REMOVED = 146,
        ___END_TOWN_PORTAL = 147,

        ___START_REACTORPOOL = 148,
        REACTOR_CHANGE_STATE = 149,
        // 150 is missing
        REACTOR_ENTER_FIELD = 151,
        REACTOR_LEAVE_FIELD = 152,
        ___END_REACTORPOOL = 153,

        ___START_ETCFIELDOBJ = 154,
        SNOWBALL_STATE = 155,
        SNOWBALL_HIT = 156,
        
        COCONUT_HIT = 157, // was 156 in v40b, assumed to be 157 in v8
        COCONUT_SCORE = 158, // was 157 in v40b, assumed to be 158 in v8

        // MC stuff comes here

        // Zakum timer here
        ___END_ETCFIELDOBJ = 159,

        ___START_SCRIPT = 160,
        SCRIPT_MESSAGE = 161, // IE used for SendSnowballRules
        ___END_SCRIPT = 162,

        ___START_SHOP = 163,
        SHOP = 164,
        SHOP_TRANSACTION = 165,
        ___END_SHOP = 166,

        ___START_STORAGE = 167,
        STORAGE = 168,
        STORAGE_RESULT = 169,
        ___END_STORAGE = 170,

        ___START_MESSENGER = 171,
        MESSENGER = 172,
        ___END_MESSENGER = 173,

        ___START_MINIROOM = 174,
        MINI_ROOM_BASE = 175,
        ___END_MINIROOM = 176,

        ___START_TOURNAMENT = 177,
        TOURNAMENT_INFO = 178,
        TOURNAMENT_MATCH_TABLE = 179,
        TOURNAMENT_SET_PRIZE = 180,
        TOURNAMENT_NOTICE_UEW = 181,
        TOURNAMENT_AVATAR_INFO = 182,
        ___END_TOURNAMENT = 183,

        // 184 missing

        ___START_CASHSHOP = 185,
        CASHSHOP_RECHARGE = 186,
        CASHSHOP_UPDATE_AMOUNTS = 187,
        CASHSHOP_ACTION = 188,
        ___END_CASHSHOP = 189,
    }
}