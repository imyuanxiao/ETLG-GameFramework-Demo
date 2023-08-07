
namespace ETLG.Data
{
    public sealed class LandingpointData
    {
        private DRLandingPoint dRLandingPoint;
        public NPCData[] npcs;
        
        public int Id
        {
            get
            {
                return dRLandingPoint.Id;
            }
        }

        public int PlanetId
        {
            get
            {
                return dRLandingPoint.PlanetId;
            }
        }

        public string Domain
        {
            get
            {
                return dRLandingPoint.Domain;
            }
        }

        public string Title
        {
            get
            {
                return dRLandingPoint.Course;
            }
        }

        public int[] NPCsID
        {
            get
            {
                return dRLandingPoint.NPCsID;
            }
        }

        public string Description
        {
            get
            {
                return dRLandingPoint.Description;
            }
        }


        // 构造方法
        public LandingpointData(DRLandingPoint dRLandingPoint, NPCData[] npcs)
        {
            this.dRLandingPoint = dRLandingPoint;
            this.npcs = npcs;
        }

        

    }

}
