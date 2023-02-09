namespace WvsBeta.Common.Sessions
{
	public enum ServerMessages : byte
    {
        // ___START_LOGIN,
        CHECK_PASSWORD_RESULT = 1,
        __UNKNOWN_2 = 2,
        CHECK_USER_LIMIT_RESULT = 3,
        SET_GENDER_RESULT = 4,
        CONFIRM_EULA_RESULT = 5,
        __UNKNOWN_6,
        PIN_OPERATION = 7,
        PIN_ASSIGNED = 8,
        WORLD_INFORMATION = 9,
        SELECT_CHANNEL_RESULT = 10,
        SELECT_CHARACTER_RESULT = 11,
        CHECK_CHARACTER_NAME_AVAILABLE = 12,
        CREATE_NEW_CHARACTER_RESULT = 13,
        DELETE_CHARACTER_RESULT = 14,
        // __END_LOGIN

        // __START_SOCKET
        MIGRATE_COMMAND = 15, // Change channel
        PING = 16,
        __UNKNOWN_17 = 17,
        __UNKNOWN_18 = 18,
        __UNKNOWN_19 = 19,
        __MISSING_20 = 20,
        __MISSING_21 = 21,
        __MISSING_22 = 22,
        __UNKNOWN_23 = 23,
        // ___END_SOCKET

        // __START_CWVSCONTEXT
        INVENTORY_OPERATION = 27,
        INVENTORY_GROW = 28,
        STAT_CHANGED = 29,
        FORCED_STAT_SET = 30,
        FORCED_STAT_RESET = 31,
        CHANGE_SKILL_RECORD_RESULT = 32,
        SKILL_USE_RESULT = 33,
        GIVE_POPULARITY_RESULT = 34,
        MESSAGE = 35,
        MEMO_RESULT = 36,
        MAP_TRANSFER_RESULT = 37,
        ANTI_MACRO_RESULT = 38,
        __MISSING_39 = 39,
        QUEST_CLEAR = 40,
        SUE_CHARACTER_RESULT = 41,
        __MISSING_42 = 42,
        __MISSING_43 = 43,
        CHARACTER_INFO = 44,
        PARTY_RESULT = 45,
        FRIEND_RESULT = 46,
        __MISSING_47 = 47,
        GUILD_RESULT = 48,
        TOWN_PORTAL = 49,
        BROADCAST_MSG = 50,
        // __END_CWVSCONTEXT

        // ___START_STAGE
        SET_FIELD = 53,
        SET_CASH_SHOP = 54,
        // ___END_STAGE

        // ___START_FIELD
        TRANSFER_FIELD_REQ_IGNORED = 57,
        TRANSFER_CHANNEL_REQ_IGNORED = 58,
        FIELD_SPECIFIC_DATA = 59,
        GROUP_MESSAGE = 60,
        WHISPER = 61,
        SUMMON_ITEM_INAVAILABLE = 62,
        FIELD_EFFECT = 63,
        BLOW_WEATHER = 64,
        PLAY_JUKE_BOX = 65,
        ADMIN_RESULT = 66,
        QUIZ = 67,
        DESC = 68,
        CLOCK = 69,
        CONTI_TARGET_FIELD = 70,
        CONTISTATE = 71,
        __UNKNOWN_72,
        WARN_MESSAGE = 73,

        ___START_USERPOOL,
        USER_ENTER_FIELD = 75,
        USER_LEAVE_FIELD = 76,

        ___START_USERCOMMON,
        CHAT = 78,
        MINI_ROOM_BALLOON = 79,
        SET_CONSUME_ITEM_EFFECT = 80, // int itemid; See Effect\ItemEff.img\(itemid)

        ___START_PET,
        SPAWN_PET,
        PET_MOVE = 83,
        PET_ACTION = 84,
        PET_NAME_CHANGED = 85,
        PET_INTERACTION = 86,
        ___END_PET,

        ___START_SPAWN,
        SPAWN_ENTER_FIELD = 89,
        SPAWN_LEAVE_FIELD = 90,
        SPAWN_MOVE = 91,
        SPAWN_ATTACK = 92,
        SPAWN_HIT = 93,
        ___END_SPAWN,

        ___START_USERREMOTE,
        MOVE_PLAYER = 97,
        CLOSE_RANGE_ATTACK = 98,
        RANGED_ATTACK = 99,
        MAGIC_ATTACK = 100,
        PREPARE_SKILL = 101, // Skills related: 1111008 (Shout), 1311006 (Dragon Roar), 5001006 (Super Dragon Roar)
        SKILL_CANCEL = 102,
        DAMAGE_PLAYER = 103, // Called 'Hit'
        FACIAL_EXPRESSION = 104,
        __UNKNOWN_105 = 105,
        AVATAR_MODIFIED = 106, //Called UPDATE_CHAR_LOOK in odin, new name = gms-like
        SHOW_FOREIGN_EFFECT = 107, // Called 'Effect'
        GIVE_FOREIGN_BUFF = 108, // CUserRemote::OnSetTemporaryStat
        RESET_TEMPORARY_STAT = 109, // CUserRemote::OnResetTemporaryStat
        UPDATE_PARTYMEMBER_HP = 110,
        __UNKNOWN_111 = 111,
        __UNKNOWN_112 = 112,
        ___END_USERREMOTE,

        ___START_USERLOCAL,
        SIT_RESULT = 115,
        PLAYER_EFFECT = 116, // CUser::OnEffect
        __UNKNOWN_117 = 117, // Reads a byte?
        __MISSING_118,
        MESOBAG_SUCCEED = 119,
        MESOBAG_FAILED = 120,
        ___END_USERLOCAL,

        // ___START_MOBPOOL
        MOB_ENTER_FIELD = 125,
        MOB_LEAVE_FIELD = 126,
        MOB_CHANGE_CONTROLLER = 127,

        // ___START_MOB
        MOB_MOVE = 129,
        MOB_CTRL_ACK = 130, // Mob move response
        __MISSING_131,
        MOB_STAT_SET = 132,
        MOB_STAT_RESET = 133,
        MOB_SUSPEND_RESET = 134,
        MOB_AFFECTED = 135,
        MOB_DAMAGED = 136,
        MOB_SHOW_SPECIAL_EFFECT = 137,
        __PADDING_122,
        ___END_MOB,

        ___END_MOBPOOL,

        ___START_NPCPOOL,
        NPC_ENTER_FIELD = 142,
        NPC_LEAVE_FIELD = 143,
        NPC_CHANGE_CONTROLLER = 144,
        NPC_SET_SPECIAL_ACTION = 145,
        NPC_ANIMATE = 146,
        ___END_NPCPOOL,

        __PADDING_148,
        
        ___START_DROPPOOL,
        DROP_ENTER_FIELD = 150,
        DROP_LEAVE_FIELD = 151,
        ___END_DROPPOOL,

        ___START_MESSAGEBOXPOOL,
        MESSAGE_BOX_CREATE_FAILED = 154,
        MESSAGE_BOX_ENTER_FIELD = 155,
        MESSAGE_BOX_LEAVE_FIELD = 156,
        ___END_MESSAGEBOXPOOL,

        ___START_AFFECTED_AREA,
        AFFECTED_AREA_CREATED = 159,
        AFFECTED_AREA_REMOVED = 160,
        ___END_AFFECTED_AREA,

        ___START_TOWN_PORTAL,
        TOWN_PORTAL_CREATED = 163,
        TOWN_PORTAL_REMOVED = 164,
        ___END_TOWN_PORTAL,

        ___START_REACTORPOOL,
        REACTOR_CHANGE_STATE = 167,
        __MISSING_168,
        REACTOR_ENTER_FIELD = 169,
        REACTOR_LEAVE_FIELD = 170,
        ___END_REACTORPOOL,

        ___START_ETCFIELDOBJ,
        SNOWBALL_STATE,
        SNOWBALL_HIT,

        COCONUT_ATTACK = 175,
        COCONUT_SCORE = 176,
        // MC stuff comes here
        // Zakum timer here
        ___END_ETCFIELDOBJ,

        ___START_SCRIPT,
        SCRIPT_MESSAGE = 179, // IE used for SendSnowballRules
        ___END_SCRIPT,

        ___START_SHOP,
        SHOP = 182,
        SHOP_TRANSACTION = 183,
        ___END_SHOP,
        __PADDING_186,
        ___START_STORAGE,
        STORAGE = 187,
        STORAGE_RESULT = 188,
        ___END_STORAGE,

        ___START_MESSENGER,
        MESSENGER = 191,
        ___END_MESSENGER,

        ___START_MINIROOM,
        MINI_ROOM_BASE = 194,
        ___END_MINIROOM,

        ___START_TOURNAMENT,
        TOURNAMENT_INFO,
        TOURNAMENT_MATCH_TABLE,
        TOURNAMENT_SET_PRIZE,
        TOURNAMENT_NOTICE_UEW,
        TOURNAMENT_AVATAR_INFO,
        ___END_TOURNAMENT,

        __PADDING_203,

        ___START_CASHSHOP,
        CASHSHOP_RECHARGE,
        CASHSHOP_UPDATE_AMOUNTS,
        CASHSHOP_ACTION,
        ___END_CASHSHOP,
    }
}