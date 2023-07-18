
namespace ETLG.Data
{
    public sealed class NPCData
    {
        private DRNPC dRNPC;
       // private QuestData[] quests;

        
        
        public int Id
        {
            get
            {
                return dRNPC.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRNPC.Name;
            }
        }

        public string Avatar
        {
            get
            {
                return dRNPC.Avatar;
            }
        }

        public string Type
        {
            get
            {
                return dRNPC.Type;
            }
        }

        public int Money
        {
            get
            {
                return dRNPC.Money;
            }
        }

        public int[] Artifacts
        {
            get
            {
                return dRNPC.Artifacts;
            }
        }

        public int[] Quests
        {
            get
            {
                return dRNPC.Quests;
            }
        }
        public string Description
        {
            get
            {
                return dRNPC.Description;
            }
        }

        public string XMLDialogSource
        {
            get
            {
                return dRNPC.XMLDialogSource;
            }
        }

        public string XMLQuizSource
        {
            get
            {
                return dRNPC.XMLQuizSource;
            }
        }

        // 构造方法
        public NPCData(DRNPC dRNPC)
        {
            this.dRNPC = dRNPC;
        }
/*        public NPCData(DRNPC dRNPC, QuestData[] quests)
        {
            this.dRNPC = dRNPC;
            this.quests = quests;
        }*/

    }

}
