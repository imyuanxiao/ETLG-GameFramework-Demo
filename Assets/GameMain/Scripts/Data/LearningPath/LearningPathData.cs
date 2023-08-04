namespace ETLG.Data
{
    public sealed class LearningPathData
    {
        private DRLearningPath dRLearningPath;

        public LearningPathData(DRLearningPath dRLearningPath)
        {
            this.dRLearningPath = dRLearningPath;
        }
       
        public int Id
        {
            get
            {
                return dRLearningPath.Id;
            }
        }

        public int NPCID
        {
            get
            {
                return dRLearningPath.NPCID;
            }
        }
    }
}

