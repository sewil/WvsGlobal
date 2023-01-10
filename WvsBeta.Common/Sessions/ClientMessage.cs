namespace WvsBeta.Common.Sessions
{
    public enum ClientMessages : byte
    {
        __PADDING_0 = 0,
        // Login Headers
        LOGIN_CHECK_PASSWORD = 1,
        LOGIN_SELECT_CHANNEL = 2,
        LOGIN_WORLD_SELECT = 3,
        LOGIN_EULA = 4,
        LOGIN_SET_GENDER = 5,
        LOGIN_WORLD_INFO_REQUEST = 6, // Unused
        LOGIN_SELECT_CHARACTER = 7,
        MIGRATE_IN = 8,
        LOGIN_CHECK_CHARACTER_NAME = 9,
        LOGIN_CREATE_CHARACTER = 10,
        LOGIN_DELETE_CHARACTER = 11,

        // Client Communication Headers
        PONG = 12,
        CLIENT_CRASH_REPORT = 13,
        CLIENT_HASH = 14,
        // Game Headers
        
        __PADDING_15 = 15,
        __PADDING_16 = 16,
        ENTER_PORTAL = 17,
        CHANGE_CHANNEL = 18,
        ENTER_CASH_SHOP = 19,
        MOVE_PLAYER = 20,
        SIT_REQUEST = 21,

        //Damage Headers
        CLOSE_RANGE_ATTACK = 22,
        RANGED_ATTACK = 23,
        MAGIC_ATTACK = 24,
        __PADDING_25,
        TAKE_DAMAGE = 26,

        CHAT = 27,
        EMOTE = 28,

        __PADDING_29,
        __PADDING_30,

        //NPC Interaction Headers
        NPC_TALK = 31,
        NPC_TALK_MORE = 32,
        SHOP_ACTION = 33,
        STORAGE_ACTION = 34,

        //Inventory Headers
        ITEM_MOVE = 35,
        ITEM_USE = 36,
        SUMMON_BAG_USE = 37,
        PET_EAT_FOOD = 38,
        CASH_ITEM_USE = 39, // Assumed Value
        RETURN_SCROLL_USE = 40, // Assumed Value
        SCROLL_USE = 41, // Assumed Value
        
        //Player Stat Headers
        DISTRIBUTE_AP = 42,
        HEAL_OVER_TIME = 43,
        DISTRIBUTE_SP = 44,
        GIVE_BUFF = 45,
        CANCEL_BUFF = 46, // Assumed Value
        PREPARE_SKILL = 47,

        DROP_MESOS = 48,
        GIVE_FAME = 49,
        __PADDING_50,
        CHAR_INFO_REQUEST = 51,
        SPAWN_PET = 52, // Assumed Value
        CHARACTER_IS_DEBUFFED = 53, // spammed.
        ENTER_SCRIPTED_PORTAL = 54,
        MAP_TRANSFER_REQUEST = 55, // Teleport rock stuff

        REPORT_USER = 56,
        __PADDING_57,
        __PADDING_58,
        GROUP_MESSAGE = 59,
        WHISPER = 60,
        MESSENGER = 61,
        MINI_ROOM_OPERATION = 62,
        PARTY_OPERATION = 63,
        DENY_PARTY_REQUEST = 64,
        // -- nonexistant headers, such as admin commands and guild! --
        __PADDING_65,
        __PADDING_66,
        FRIEND_OPERATION = 67,
        NOTE_OPERATION = 68,
        ENTER_TOWN_PORTAL = 69,
        __PADDING_70,
        __PADDING_71,
        PET_MOVE = 72,
        PET_ACTION = 73,
        PET_INTERACTION = 74,
        __PADDING_75,
        __PADDING_76,
        __PADDING_77,
        SUMMON_MOVE = 78,
        SUMMON_ATTACK = 79,
        SUMMON_DAMAGED = 80,
        __PADDING_81,
        __PADDING_82,
        __PADDING_83,
        __PADDING_84,
        __PADDING_85,
        MOB_MOVE = 86,
        MOB_APPLY_CONTROL = 87, // Only when PickUpDrop or FirstAttack
        MOB_PICKUP_DROP = 88,
        __PADDING_89,
        __PADDING_90,
        NPC_ANIMATE = 91,
        __PADDING_92,
        __PADDING_93,
        __PADDING_94,
        DROP_PICK_UP = 95,
        __PADDING_96,
        __PADDING_97,
        REACTOR_HIT = 98,
        __PADDING_99,
        __PADDING_100,
        __PADDING_101,
        __PADDING_102,
        FIELD_SNOWBALL_ATTACK = 103,
        FIELD_COCONUT_ATTACK = 104,
        __PADDING_105,
        __PADDING_106,
        FIELD_TOURNAMENT_MATCHTABLE = 107, // '/matchtable 1' sends this
        __PADDING_108,
        __PADDING_109,
        __PADDING_110,
        __PADDING_111,
        FIELD_CONTIMOVE_STATE = 112, // CONTISTATE?
        __PADDING_113,
        __PADDING_114,
        __PADDING_115,
        CASHSHOP_QUERY_CASH = 116,
        CASHSHOP_ACTION = 117,
        CASHSHOP_ENTER_COUPON = 118,
        
        // THERE SHOULD BE 118 (+ 1) FIELDS

        __CUSTOM_DC_ME__ = 190,

        JUNK = 250
    }
}