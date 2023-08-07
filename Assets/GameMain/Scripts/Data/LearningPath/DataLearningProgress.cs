using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataLearningProgress : DataBase
    {
        public bool update;
        public bool UIPlanetLandingPointsUpdate;
        public bool UIPlanetOverviewUpdate;
        public bool PlanetsUpdate;
        public bool ItemPlanetSelect;
        public bool open;

        public void reset()
        {
            update = false;

        }

        public void getAward()
        {
            update = true;
            UIPlanetLandingPointsUpdate = true;
            UIPlanetOverviewUpdate = true;
            PlanetsUpdate = true;
            ItemPlanetSelect = true;
        }
    }

}
