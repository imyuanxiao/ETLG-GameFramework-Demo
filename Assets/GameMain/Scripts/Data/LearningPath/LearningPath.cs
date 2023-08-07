namespace ETLG.Data
{
    public sealed class LearningPath
    {
        private int id { get; }
        public int NPCId { get; }
        private DataNPC dataNPC;
        private NPCData NPCData;
        public bool finish { get; set; }

        public LearningPath(DRLearningPath dRLearningPath)
        {
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            this.id = dRLearningPath.Id;
            this.NPCId = dRLearningPath.NPCID;
            this.NPCData = dataNPC.GetNPCData(dRLearningPath.NPCID);
            finish = false;        
        }

        public NPCData getPathNPCData()
        {
            return NPCData;
        }

        public string getCurrentType()
        {
            return NPCData.Type;
        }
    }
}

