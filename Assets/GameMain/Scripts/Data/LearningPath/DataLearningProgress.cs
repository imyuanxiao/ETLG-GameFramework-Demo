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

        public void reset()
        {
            update = false;

        }
    }

}
