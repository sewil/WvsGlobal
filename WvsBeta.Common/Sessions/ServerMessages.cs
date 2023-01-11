namespace WvsBeta.Common.Sessions
{
	public enum ServerMessages : byte
    {
        ___START_SOCKET,
        CHECK_PASSWORD_RESULT,
        //Confirm that the server can handle the connection
        __UNKNOWN_2,
        CHECK_USER_LIMIT_RESULT,

        //Set Gender result
        SET_ACCOUNT_RESULT,

        CONFIRM_EULA_RESULT,
        __PADDING_6,
        PIN_OPERATION,
        PIN_ASSIGNED,
        WORLD_INFORMATION,

        SELECT_CHANNEL_RESULT,
        //Confusing naming, basically connect to server header :)
        SELECT_CHARACTER_RESULT,
        CHECK_CHARACTER_NAME_AVAILABLE,
        CREATE_NEW_CHARACTER_RESULT,
        DELETE_CHARACTER_RESULT,

        CHANGE_CHANNEL,
        PING,
        AUTHEN_CODE_CHANGED,
        SECURITY_SOMETHING, // Either read a buffer (passed to GG?) or a set of ints. Looks like CRC info.
        ___END_SOCKET,

        ___START_CHARACTERDATA,
        INVENTORY_OPERATION,
        INVENTORY_GROW,
        STAT_CHANGED,
        FORCED_STAT_SET,
        FORCED_STAT_RESET,
        CHANGE_SKILL_RECORD_RESULT,
        SKILL_USE_RESULT,
        GIVE_POPULARITY_RESULT,
        SHOW_STATUS_INFO, // Called 'Message'
        MEMO_RESULT,
        MAP_TRANSFER_RESULT,
        SUE_CHARACTER_RESULT,
        __PADDING_32, // Possibly ClaimServer stuff
        __PADDING_33,
        CHARACTER_INFO,
        PARTY_RESULT,
        FRIEND_RESULT,
        TOWN_PORTAL,
        BROADCAST_MSG,
        ___END_CHARACTEDATA,

        ___START_STAGE,
        SET_FIELD,
        SET_CASH_SHOP,
        ___END_STAGE,

        ___START_FIELD,
        TRANSFER_FIELD_REQ_IGNORED,
        TRANSFER_CHANNEL_REQ_IGNORED,
        FIELD_SPECIFIC_DATA,
        GROUP_MESSAGE,
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

        ___START_MOBPOOL,
        MOB_ENTER_FIELD,
        MOB_LEAVE_FIELD,
        MOB_CHANGE_CONTROLLER,

        ___START_MOB,
        MOB_MOVE,
        MOB_MOVE_RESPONSE,
        __PADDING_115,
        MOB_STAT_SET,
        MOB_STAT_RESET,
        MOB_SUSPEND_RESET,
        MOB_AFFECTED,
        MOB_DAMAGED,
        MOB_EFFECT_BY_SKILL, // int mapmobid, int skillid. Seems to activate a special animation caused by certain skills (e.g. 3210001 mortal blow), mainly those with the 'special' node
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