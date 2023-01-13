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
        WHISPER,
        SUMMON_ITEM_INAVAILABLE,
        FIELD_EFFECT,
        BLOW_WEATHER,
        PLAY_JUKE_BOX,
        ADMIN_RESULT,
        QUIZ,
        DESC,
        CLOCK,

        CONTIMOVE,
        CONTISTATE,

        WARN_MESSAGE,

        ___START_USERPOOL,
        USER_ENTER_FIELD,
        USER_LEAVE_FIELD,

        ___START_USERCOMMON,
        CHAT,
        MINI_ROOM_BALLOON,
        SET_CONSUME_ITEM_EFFECT, // int itemid; See Effect\ItemEff.img\(itemid)

        ___START_PET,
        SPAWN_PET,
        PET_MOVE,
        PET_ACTION,
        PET_NAME_CHANGED,
        PET_INTERACTION,
        ___END_PET,

        ___START_SPAWN,
        SPAWN_ENTER_FIELD,
        SPAWN_LEAVE_FIELD,
        SPAWN_MOVE,
        SPAWN_ATTACK,
        SPAWN_HIT,
        ___END_SPAWN,

        ___START_USERREMOTE,
        MOVE_PLAYER,
        CLOSE_RANGE_ATTACK,
        RANGED_ATTACK,
        MAGIC_ATTACK,
        PREPARE_SKILL, // Skills related: 1111008 (Shout), 1311006 (Dragon Roar), 5001006 (Super Dragon Roar)
        SKILL_END,
        DAMAGE_PLAYER, // Called 'Hit'
        FACIAL_EXPRESSION,
        AVATAR_MODIFIED, //Called UPDATE_CHAR_LOOK in odin, new name = gms-like
        SHOW_FOREIGN_EFFECT, // Called 'Effect'
        GIVE_FOREIGN_BUFF,
        RESET_FOREIGN_BUFF,
        UPDATE_PARTYMEMBER_HP,
        ___END_USERREMOTE,

        ___START_USERLOCAL,
        SHOW_CHAIR,
        PLAYER_EFFECT, // CUser::OnEffect
        __PADDING_102, // Reads a byte?
        __PADDING_103,
        MESOBAG_SUCCEED,
        MESOBAG_FAILED,
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
        NPC_ENTER_FIELD,
        NPC_LEAVE_FIELD,
        NPC_CHANGE_CONTROLLER,
        NPC_SET_SPECIAL_ACTION,
        NPC_ANIMATE,
        ___END_NPCPOOL,
        __PADDING_132,
        ___START_DROPPOOL,
        DROP_ENTER_FIELD,
        DROP_LEAVE_FIELD,
        ___END_DROPPOOL,

        ___START_MESSAGEBOXPOOL,
        MESSAGE_BOX_CREATE_FAILED,
        MESSAGE_BOX_ENTER_FIELD,
        MESSAGE_BOX_LEAVE_FIELD,
        ___END_MESSAGEBOXPOOL,

        ___START_AFFECTED_AREA,
        AFFECTED_AREA_CREATED,
        AFFECTED_AREA_REMOVED,
        ___END_AFFECTED_AREA,

        ___START_TOWN_PORTAL,
        TOWN_PORTAL_CREATED,
        TOWN_PORTAL_REMOVED,
        ___END_TOWN_PORTAL,

        ___START_REACTORPOOL,
        REACTOR_CHANGE_STATE,
        __PADDING_152,
        REACTOR_ENTER_FIELD,
        REACTOR_LEAVE_FIELD,
        ___END_REACTORPOOL,

        ___START_ETCFIELDOBJ,
        SNOWBALL_STATE,
        SNOWBALL_HIT,

        COCONUT_HIT, // was 156 in v40b, assumed to be 159 in v12
        COCONUT_SCORE, // was 157 in v40b, assumed to be 160 in v12

        // MC stuff comes here

        // Zakum timer here
        ___END_ETCFIELDOBJ,

        ___START_SCRIPT,
        SCRIPT_MESSAGE, // IE used for SendSnowballRules
        ___END_SCRIPT,

        ___START_SHOP,
        SHOP,
        SHOP_TRANSACTION,
        ___END_SHOP,

        ___START_STORAGE,
        STORAGE,
        STORAGE_RESULT,
        ___END_STORAGE,

        ___START_MESSENGER,
        MESSENGER,
        ___END_MESSENGER,

        ___START_MINIROOM,
        MINI_ROOM_BASE,
        ___END_MINIROOM,

        ___START_TOURNAMENT,
        TOURNAMENT_INFO,
        TOURNAMENT_MATCH_TABLE,
        TOURNAMENT_SET_PRIZE,
        TOURNAMENT_NOTICE_UEW,
        TOURNAMENT_AVATAR_INFO,
        ___END_TOURNAMENT,

        __PADDING_186,

        ___START_CASHSHOP,
        CASHSHOP_RECHARGE,
        CASHSHOP_UPDATE_AMOUNTS,
        CASHSHOP_ACTION,
        ___END_CASHSHOP,
    }
}