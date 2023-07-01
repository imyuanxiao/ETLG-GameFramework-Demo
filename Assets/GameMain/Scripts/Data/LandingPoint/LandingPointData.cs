
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

        public string Location
        {
            get
            {
                return dRLandingPoint.Location;
            }
        }

        public string Type
        {
            get
            {
                return dRLandingPoint.Type;
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
