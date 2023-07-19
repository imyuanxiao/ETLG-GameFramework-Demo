
namespace ETLG.Data
{
    public sealed class PlanetData
    {
        private DRPlanet dRPlanet;
        private LandingpointData[] landingpoints;
        
        public int Id
        {
            get
            {
                return dRPlanet.Id;
            }
        }

        public string Name
        {
            get
            {
                return dRPlanet.Name;
            }
        }

        public string TypeStr
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_DOMAIN + Type);
            }
        }

        public int Type
        {
            get
            {
                return dRPlanet.Type;
            }
        }

        public int[] Coordinates
        {
            get
            {
                return dRPlanet.Coordinates;
            }
        }

        public int AssetID
        {
            get
            {
                return dRPlanet.AssetID;
            }
        }
        
        // This will be shown on PlanetInfo UI
        public string Description
        {
            get
            {
                // return dRPlanet.Description;
                return GameEntry.Localization.GetString(Name + Constant.Key.POST_DESC);
            }
        }

        public int[] LandingPoints
        {
            get
            {
                return dRPlanet.LandingPoints;
            }
        }
/*        public LandingpointData[] LandingPoints
        {
            get
            {
                return landingpoints;
            }
        }
*/
        // 构造方法
    /*    public PlanetData(DRPlanet dRPlanet, LandingpointData[] landingpoints)
        {
            this.dRPlanet = dRPlanet;
            this.landingpoints = landingpoints;
        }*/
        public PlanetData(DRPlanet dRPlanet)
        {
            this.dRPlanet = dRPlanet;
        }
    }

}
