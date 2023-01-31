namespace WvsBeta.Common.Sessions
{
    public enum ClientMessages : byte
    {
        __PADDING_0,
        // Login Headers
        LOGIN_CHECK_PASSWORD,
        __PADDING_1,
        __PADDING_2,
        LOGIN_SELECT_CHANNEL,
        LOGIN_WORLD_SELECT,
        LOGIN_EULA,
        LOGIN_SET_GENDER,
        LOGIN_CHECK_PIN,
        LOGIN_UPDATE_PIN,
        LOGIN_WORLD_INFO_REQUEST,
        LOGIN_SELECT_CHARACTER,
        MIGRATE_IN,
        LOGIN_CHECK_CHARACTER_NAME,
        LOGIN_CREATE_CHARACTER,
        LOGIN_DELETE_CHARACTER,

        // __START_SOCKET
        PONG = 16,
        CLIENT_CRASH_REPORT,
        CLIENT_HASH = 18,
        RETURN_TO_LOGIN,
        // __END_SOCKET

        ENTER_PORTAL = 22,
        CHANGE_CHANNEL,
        ENTER_CASH_SHOP = 24,
        MOVE_PLAYER = 25,
        SIT_REQUEST,

        //Damage Headers
        CLOSE_RANGE_ATTACK = 27,
        RANGED_ATTACK = 28,
        MAGIC_ATTACK,
        TAKE_DAMAGE = 31,

        CHAT = 32,
        EMOTE,

        __PADDING_31,
        __PADDING_32,

        //NPC Interaction Headers
        NPC_TALK = 37,
        NPC_TALK_MORE,
        SHOP_ACTION,
        STORAGE_ACTION,

        //Inventory Headers
        ITEM_MOVE = 41,
        ITEM_USE,
        SUMMON_BAG_USE,
        PET_EAT_FOOD,
        CASH_ITEM_USE, // Assumed Value
        RETURN_SCROLL_USE, // Assumed Value
        SCROLL_USE = 48, // Assumed Value

        //Player Stat Headers
        DISTRIBUTE_AP = 49,
        HEAL_OVER_TIME,
        DISTRIBUTE_SP,
        GIVE_BUFF,
        CANCEL_BUFF, // Assumed Value
        PREPARE_SKILL,

        DROP_MESOS,
        GIVE_FAME,
        __PADDING_52,
        CHAR_INFO_REQUEST,
        SPAWN_PET, // Assumed Value
        CHARACTER_IS_DEBUFFED, // spammed.
        ENTER_SCRIPTED_PORTAL,
        MAP_TRANSFER_REQUEST, // Teleport rock stuff

        REPORT_USER,
        __PADDING_59,
        __PADDING_60,
        GROUP_MESSAGE,
        WHISPER = 71,
        MESSENGER = 72,
        MINI_ROOM_OPERATION = 73,
        PARTY_OPERATION,
        DENY_PARTY_REQUEST,
        __PADDING_76,
        __PADDING_77,
        __PADDING_78,
        __PADDING_79,
        FRIEND_OPERATION = 80,
        NOTE_OPERATION,
        ENTER_TOWN_PORTAL,
        __PADDING_72,
        __PADDING_73,
        PET_MOVE = 85,
        PET_ACTION,
        PET_INTERACTION,
        __PADDING_88,
        __PADDING_89,
        __PADDING_90,
        SUMMON_MOVE = 91,
        SUMMON_ATTACK,
        SUMMON_DAMAGED,
        __PADDING_83,
        __PADDING_84,
        __PADDING_85,
        __PADDING_86,
        __PADDING_87,
        MOB_MOVE = 99,
        MOB_APPLY_CONTROL, // Only when PickUpDrop or FirstAttack
        MOB_PICKUP_DROP,
        __PADDING_91,
        __PADDING_92,
        NPC_ANIMATE,
        __PADDING_94,
        __PADDING_95,
        __PADDING_96,
        DROP_PICK_UP,
        __PADDING_98,
        __PADDING_99,
        REACTOR_HIT,
        __PADDING_101,
        __PADDING_102,
        __PADDING_103,
        __PADDING_104,
        FIELD_SNOWBALL_ATTACK,
        FIELD_COCONUT_ATTACK,
        __PADDING_107,
        __PADDING_108,
        FIELD_TOURNAMENT_MATCHTABLE, // '/matchtable 1' sends this
        __PADDING_110,
        __PADDING_111,
        __PADDING_112,
        __PADDING_113,
        FIELD_CONTIMOVE_STATE, // CONTISTATE?
        __PADDING_115,
        __PADDING_116,
        __PADDING_117,
        CASHSHOP_QUERY_CASH,
        CASHSHOP_ACTION,
        CASHSHOP_ENTER_COUPON,

        // THERE SHOULD BE 120 (+ 1) FIELDS

        __CUSTOM_DC_ME__ = 0xC0,

        JUNK
    }
}