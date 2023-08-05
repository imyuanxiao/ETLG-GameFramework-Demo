
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

        public string Title
        {
            get
            {
                return dRLandingPoint.Course;
               // return GameEntry.Localization.GetString(Constant.Key.PRE_LANDING_POINT + Id + Constant.Key.POST_TITLE);
            }
        }

        public string Description
        {
            get
            {
                // return dRPlanet.Description;
                return GameEntry.Localization.GetString(Constant.Key.PRE_LANDING_POINT + Id + Constant.Key.POST_DESC);
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
