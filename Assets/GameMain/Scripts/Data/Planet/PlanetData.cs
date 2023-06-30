
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


        public string Type
        {
            get
            {
                return dRPlanet.Type;
            }
        }

        public string Description
        {
            get
            {
                return dRPlanet.Description;
            }
        }

        // 构造方法
        public PlanetData(DRPlanet dRPlanet, LandingpointData[] landingpoints)
        {
            this.dRPlanet = dRPlanet;
            this.landingpoints = landingpoints;
        }

    }

}
