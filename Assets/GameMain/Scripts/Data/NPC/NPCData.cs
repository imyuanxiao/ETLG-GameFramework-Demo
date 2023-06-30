
namespace ETLG.Data
{
    public sealed class NPCData
    {
        private DRNPC dRNPC;
        private QuestData[] quests;
        
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

        public string Type
        {
            get
            {
                return dRNPC.Type;
            }
        }

        public string Description
        {
            get
            {
                return dRNPC.Description;
            }
        }

        public int PlanetId
        {
            get
            {
                return dRNPC.PlanetId;
            }
        }


        public int[] Quests
        {
            get
            {
                return dRNPC.Quests;
            }
        }


        // 构造方法
        public NPCData(DRNPC dRNPC, QuestData[] quests)
        {
            this.dRNPC = dRNPC;
            this.quests = quests;
        }

    }

}
