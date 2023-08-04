using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataLearningPath : DataBase
    {
        private IDataTable<DRLearningPath> dtlearningPaths;
        private SortedDictionary<int, LearningPath> learningPathDic = new SortedDictionary<int, LearningPath>();
        private LearningPath currentPath;

        protected override void OnPreload()
        {
            LoadDataTable("LearningPath");

        }

        protected override void OnLoad()
        {
            dtlearningPaths = GameEntry.DataTable.GetDataTable<DRLearningPath>();

            DRLearningPath[] dRlearningPaths = dtlearningPaths.GetAllDataRows();

            foreach (var path in dRlearningPaths)
            {
                learningPathDic.Add(path.Id, new LearningPath(path));
            }
        }
        protected override void OnInit()
        {
           
        }

        public void finishLeantPath(int pathId)
        {
            if (learningPathDic.ContainsKey(pathId))
            {
                learningPathDic[pathId].finish=true;
            }
        }

        public void finishLeantPathByNPCId(int npcId)
        {
            foreach(LearningPath path in learningPathDic.Values)
            {
                if(path.NPCId== npcId)
                {
                    path.finish = true;
                }
            }
        }

        public LearningPath getCurrentPath()
        {
            foreach (LearningPath path in learningPathDic.Values)
            {
                if (!path.finish)
                {
                    currentPath = path;
                    return currentPath;
                }
            }
            currentPath = null;
            return currentPath;
        }

    }
}
