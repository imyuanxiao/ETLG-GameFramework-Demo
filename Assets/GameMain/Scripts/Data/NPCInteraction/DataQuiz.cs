using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataQuiz : DataBase
    {
        public string accuracyText;
        public bool pass = false;

        public string getAccuracyText()
        {
            return accuracyText + "%";
        }
    }
}
