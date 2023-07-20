
using Unity.Burst.CompilerServices;

namespace ETLG.Data
{
    public sealed class PlanetData
    {
        private DRPlanet dRPlanet;

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

        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_PLANET + Id + Constant.Key.POST_DESC);
            }
        }

        public int[] LandingPoints
        {
            get
            {
                return dRPlanet.LandingPoints;
            }
        }

        public PlanetData(DRPlanet dRPlanet)
        {
            this.dRPlanet = dRPlanet;
        }
    }

}
