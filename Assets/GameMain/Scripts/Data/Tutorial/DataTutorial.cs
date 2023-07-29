using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using System.Linq;

namespace ETLG.Data
{
    public sealed class DataTutorial : DataBase
    {

        private IDataTable<DRTutorial> dtTutorials;

        // groupId + tutorialIds
        private Dictionary<int, List<int>> dicTutorialGroupData;
        // tutorialIds
        private Dictionary<int, TutorialData> dicTutorialData;
        private List<int> listTutorialIds;

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

            listTutorialIds = new List<int>();

            foreach (var dRTutorial in dRTutorials)
            {
                if (dicTutorialData.ContainsKey(dRTutorial.Id))
                {
                    throw new System.Exception(string.Format("Data Tutorial id '{0}' duplicate.", dRTutorial.Id));
                }
                dicTutorialData.Add(dRTutorial.Id, new TutorialData(dRTutorial));
                listTutorialIds.Add((int)dRTutorial.Id);
            }

            dicTutorialGroupData = new Dictionary<int, List<int>>();

            foreach (var tutorialData in dicTutorialData)
            {

                int groupId = tutorialData.Value.Group;
                int id = tutorialData.Key;

                if (!dicTutorialGroupData.ContainsKey(groupId))
                {
                    dicTutorialGroupData.Add(groupId, new List<int>());
                }
                dicTutorialGroupData[groupId].Add(id);
            }

            CurrentTutorialID = listTutorialIds[0];
        }


        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }
        
        public List<int> GetTutorialsByGroup(int groupId)
        {
            if (!dicTutorialGroupData.ContainsKey(groupId))
            {
                return dicTutorialGroupData[1];
            }
            return dicTutorialGroupData[groupId];
        }

        public TutorialData GetCurrentTutorialData()
        {
            if (!dicTutorialData.ContainsKey(CurrentTutorialID))
            {
                Log.Error("Can not find tutorial data id '{0}'.", CurrentTutorialID);
                CurrentTutorialID = listTutorialIds[2];
                return dicTutorialData[CurrentTutorialID];
            }
            return dicTutorialData[CurrentTutorialID];
        }

        public void SetLastTutorial()
        {
            int curIndex = listTutorialIds.IndexOf(CurrentTutorialID);
            if (curIndex <= 0)
            {
                CurrentTutorialID = listTutorialIds[3];
            }
            else
            {
                CurrentTutorialID = listTutorialIds[curIndex - 1];
            }
        }

        public void SetNextTutorial()
        {
            int curIndex = listTutorialIds.IndexOf(CurrentTutorialID);
            if (curIndex >= listTutorialIds.Count - 1)
            {
                CurrentTutorialID = listTutorialIds.Last();
            }
            else
            {
                CurrentTutorialID = listTutorialIds[curIndex + 1];
            }
        }

        public int GetFirstTutorialId()
        {
            return listTutorialIds.First();
        }
        public int GetLastTutorialId()
        {
            return listTutorialIds.Last();
        }

        public void OpenGroupTutorials(int groupId)
        {
            PlayerData playerData = GameEntry.Data.GetData<DataPlayer>().GetPlayerData();
            if (playerData.PlayedTutorialGroup.Contains(groupId))
            {
                return;
            }
            playerData.PlayedTutorialGroup.Add(groupId);
            this.CurrentTutorialID = dicTutorialGroupData[groupId][0];
            GameEntry.UI.OpenUIForm(EnumUIForm.UITutorialForm);


        }

    }
}


