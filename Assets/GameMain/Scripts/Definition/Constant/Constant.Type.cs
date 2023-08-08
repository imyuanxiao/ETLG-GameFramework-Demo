namespace ETLG
{
    public static partial class Constant
    {
        public static class Type
        {
            public const string NULL = "NULL";

            // UI Type
            public const int UI_OPEN = 0;
            public const int UI_CLOSE = 1;

            public const int NPC_UI_TALK_OPEN = 2;
            public const int NPC_UI_TRADE_OPEN = 3;
            public const int NPC_UI_QUIZ_OPEN = 4;

            // tutorial group id
            public const int TUTORIAL_NEW_GAME = 1;
            public const int TUTORIAL_MAP = 2;
            public const int TUTORIAL_SPACESHIP = 3;
            public const int TUTORIAL_SKILL = 4;
            public const int TUTORIAL_BATTLE = 9;


            // player menu title
/*            public const int PLAYERMENU_SPACESHIP = 1011;
            public const int PLAYERMENU_SKILL = 1012;
            public const int PLAYERMENU_MISSION = 2;
            public const int PLAYERMENU_ACHIEVEMENT = 1027;
            public const int PLAYERMENU_KNOWLEDGE_BASE = 4;
            public const int PLAYERMENU_LEADERBOARD = 1032;*/

            // tip info position
            public const int TIP_INFO_POSITION_DEFAULT = 0;
            public const int TIP_INFO_POSITION_LEFT = 1;
            public const int TIP_INFO_POSITION_DOWN = 2;

            // skill activate state
            public const int SKILL_LOCKED = -1;
            public const int SKILL_UNLOCKED = 0;
            public const int SKILL_UPGRADED = 1;

            // skill type
            public const int SKILL_TYPE_ALL = 0;
            public const int SKILL_TYPE_COMBAT = 1;

            public const int SKILL_TYPE_EXPLORE = 2;

            public const string SKILL_TYPE_ALL_STR = "All";

            public const string SKILL_TYPE_COMBAT_STR = "Combat";
            public const string SKILL_TYPE_EXPLORE_STR = "Explore";

            public const string SKILL_TYPE_ACTIVE_STR = "Active";
            public const string SKILL_TYPE_PASSIVE_STR = "Passive";

            public const string ICON_LOST = "iconLost";


            // skill domain
            public const int DOMAIN_COMMON = 0;
            public const int DOMAIN_CLOUD_COMPUTING = 1;
            public const int DOMAIN_ARTIFICIAL_INTELLIGENCE = 2;
            public const int DOMAIN_CYBERSECURITY = 3;
            public const int DOMAIN_DATA_SCIENCE = 4;
            public const int DOMAIN_BLOCKCHAIN = 5;
            public const int DOMAIN_IoT = 6;


            // NPC Type
            public const int NPC_TYPE_TEACHER = 1;
            public const int NPC_TYPE_EXAMINER = 2;


            // artifact icon type
            public const int ARTIFACT_ICON_DEFAULT = 1;
            public const int TRADE_NPC_PLAYER = 2;
            public const int TRADE_PLAYER_NPC = 3;

            // skill icon type
            public const int SKILL_ICON_DEFAULT = 1;
            public const int SKILL_ICON_SELECT_SPACESHIP = 2;
            public const int SKILL_SKILL_TREE_MAP = 3;

            // attributes

            public const float ATTR_MAX_VALUE = 500;

            public const int ATTR_Durability = 1;
            public const int ATTR_Shields = 2;
            public const int ATTR_Agility = 3;
            public const int ATTR_Energy = 4;
            public const int ATTR_Firepower = 5;
            public const int ATTR_Firerate = 6;


            public const string ARTIFACT_TYPE = "ARTIFACT_TYPE_";


            public const int ARTIFACT_ALL = 0;
            public const int ARTIFACT_TRADE = 1;
            public const int ARTIFACT_SPECIAL = 2;
            public const int ARTIFACT_OTHERS = 4;

            // module type
            public const int MODULE_TYPE_ALL = 0;
            public const int MODULE_TYPE_WEAPON = 1;
            public const int MODULE_TYPE_ATTACK = 2;
            public const int MODULE_TYPE_DEFENSE = 3;
            public const int MODULE_TYPE_POWERDRIVE = 4;
            public const int MODULE_TYPE_SUPPORT = 5;

            //achievement type
            public const int ACHV_CORRECTQUIZ = 1001;
            public const int ACHV_WRONGQUIZ = 1002;
            public const int ACHV_PASSEDQUIZ= 1003;
            public const int ACHV_FINISHEDDIALOG= 1004;
            public const int ACHV_LEARN = 101;
            public const int ACHV_KNOWLEDGE_BASE = 102;
            public const int ACHV_INTERSTELLAR = 103;
            public const int ACHV_SPACESHIP = 104;
            public const int ACHV_RESOURCE = 105;
            public const int ACHV_BATTLE = 106;
            public const int ACHV_ACHIEVEMENT = 107;
            public const int ACHV_LOGIN = 108;
            public const int ACHV_LEADERSHIP = 109;
            public const int ACHV_HIDDEN = 110;

            public const int ACHIV_TOTAL_SPEND_MONEY = 5001;

            public const string UNLOCKED_TREASURE_CHEST = "unlocked_treasure_chest";

            // operation
            public const int ADD = 1;
            public const int SUB = 2;
            public const int MUL = 2;

            //questions type
            public const string QUIZ_MULTIPLE_ANSWERS_CHOICES  = "MCM";
            public const string QUIZ_SINGLE_ANSWERS_CHOICES  = "MCS";
            public const string QUIZ_MATCHING_CHOICES  = "MAT";

            //arrow icon type
            public const int ARROW_RIGHT = 0;
            public const int ARROW_DOWN = 1;

            //landing point select type
            public const int LP_IN_MAP = 0;
            public const int LP_IN_PLANET = 1;

            //leaderboard type
            
            public const int LB_SPACESHIP = 0;
            public const int LB_ACHIVEMENT = 1;
            public const int LB_LEARNING_PROGRESS = 2;

            public const int LB_BOSS_AI = 10;
            public const int LB_BOSS_CLOUDCOMPUTING = 11;
            public const int LB_BOSS_BLOCKCHAIN = 12;
            public const int LB_BOSS_CYBERSECURITY = 13;
            public const int LB_BOSS_DATASCIENCE = 14;
            public const int LB_BOSS_IOT = 15;
            public const int LB_BOSS_FINAL = 16;

            // reward
            public const int REWARD_TYPE_ARTIFACT = 0;
            public const int REWARD_TYPE_SKILL = 1;

            //error type
            public const int ERROR_NETWORK=1;
            public const int ERROR_SERVER = 2;
            public const int ERROR_DATA = 3;
            public const int ALERT_TRADE_MONEYNOTENOUGH = 4;
            public const int ALERT_DIALOG_QUIT = 5;
            public const int ALERT_QUIZ_QUIT = 6;
            public const int ALERT_QUIZ_QUIT_GOTTENAWARD = 7;
            public const int ALERT_DIALOG_QUIT_GOTTENAWARD = 8;

            //backend data type
            public const int BACK_LOGIN_SUCCESS = 1;
            public const int BACK_LOGIN_FAILED = 2;
            public const int BACK_REGISTER_SUCCESS = 3;
            public const int BACK_REGISTER_FAILED = 4;
            public const int BACK_LOGED_IN = 5;
            public const int BACK_SAVE_DOWNLOAD_SUCCESS = 6;
            public const int BACK_SAVE_DOWNLOAD_FAILED = 7;
            public const int BACK_PROFILE_SUCCESS = 8;
            public const int BACK_PROFILE_FAILED = 9;
            public const int BACK_PROFILE_UPDATE_SUCCESS = 10;
            public const int BACK_PROFILE_UPDATE_FAILED = 11;
            public const int BACK_PROFILE_PASSWORD_SUCCESS = 12;
            public const int BACK_PROFILE_PASSWORD_FAILED = 13;
            public const int BACK_RANK_SUCCESS = 13;
            public const int BACK_RANK_FAILED = 14;
            //login purpose
            public const int BACK_PROFILE = 1;
            public const int BACK_UPDATE = 2;
            public const int BACK_PROFILE_PASSWORD = 3;
            public const int BACK_SAVE_DOWNLOAD = 4;
            public const int BACK_SAVE_UPLOAD = 5;

            //UINPC positionX
            public const float POSITION_X_RIGHT = 550f;
            public const float POSITION_X_LEFT = -550f;

        }
    }
}
