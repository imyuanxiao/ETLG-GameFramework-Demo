using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataTutorial : DataBase
    {

        private IDataTable<DRTutorial> dtTutorials;

        private Dictionary<int, TutorialData> dicTutorialData;

        public int CurrentTutorialID { get; set; }

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
          LoadDataTable("Tutorial");
        }

        protected override void OnLoad()
        {

            dtTutorials = GameEntry.DataTable.GetDataTable<DRTutorial>();

            if (dtTutorials == null)
                throw new System.Exception("Can not get data table Tutorial");

            dicTutorialData = new Dictionary<int, TutorialData>();

            DRTutorial[] dRTutorials = dtTutorials.GetAllDataRows();

            foreach (var dRTutorial in dRTutorials)
            {
                if (dicTutorialData.ContainsKey(dRTutorial.Id))
                {
                    throw new System.Exception(string.Format("Data Tutorial id '{0}' duplicate.", dRTutorial.Id));
                }
                dicTutorialData.Add(dRTutorial.Id, new TutorialData(dRTutorial));
            }

        }



        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }

        public TutorialData GetCurrentTutorialData()
        {
            if (!dicTutorialData.ContainsKey(CurrentTutorialID))
            {
                Log.Error("Can not find tutorial data id '{0}'.", CurrentTutorialID);
                return null;
            }

            return dicTutorialData[CurrentTutorialID];
        }

        public TutorialData GetTutorialData(int id)
        {
            if (!dicTutorialData.ContainsKey(id))
            {
                Log.Error("Can not find tutorial data id '{0}'.", id);
                return null;
            }

            return dicTutorialData[id];
        }

    }
}


