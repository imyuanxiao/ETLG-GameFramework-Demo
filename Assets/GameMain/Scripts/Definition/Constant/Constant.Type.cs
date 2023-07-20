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


            // player menu title
            public const int PLAYERMENU_SPACESHIP = 1011;
            public const int PLAYERMENU_SKILL = 1012;
            public const int PLAYERMENU_MISSION = 2;
            public const int PLAYERMENU_ACHIEVEMENT = 1027;
            public const int PLAYERMENU_KNOWLEDGE_BASE = 4;
            public const int PLAYERMENU_LEADERBOARD = 1032;

            // tip info position
            public const int TIP_INFO_POSITION_DEFAULT = 0;
            public const int TIP_INFO_POSITION_LEFT = 1;

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
            public const int ATTR_Firepower = 3;
            public const int ATTR_Energy = 4;
            public const int ATTR_Agility = 5;
            public const int ATTR_Speed = 6;
            public const int ATTR_Detection = 7;
            public const int ATTR_Capacity = 8;
            public const int ATTR_Firerate = 9;
            public const int ATTR_Dogde = 10;


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
            public const int ACHV_QUIZ = 101;
            public const int ACHV_KNOWLEDGE_BASE = 102;
            public const int ACHV_INTERSTELLAR = 103;
            public const int ACHV_SPACESHIP = 104;
            public const int ACHV_RESOURCE = 105;
            public const int ACHV_BATTLE = 106;
            public const int ACHV_ACHIEVEMENT = 107;
            public const int ACHV_LOGIN = 108;
            public const int ACHV_LEADERSHIP = 109;
            public const int ACHV_HIDDEN = 110;
            
            public const string UNLOCKED_TREASURE_CHEST = "unlocked_treasure_chest";

            // operation
            public const int ADD = 1;
            public const int SUB = 2;
            public const int MUL = 2;

            //questions type
            public const string QUIZ_MULTIPLE_ANSWERS_CHOICES  = "MCM";
            public const string QUIZ_SINGLE_ANSWERS_CHOICES  = "MCS";
            public const string QUIZ_MATCHING_CHOICES  = "MAT";

        }
    }
}
