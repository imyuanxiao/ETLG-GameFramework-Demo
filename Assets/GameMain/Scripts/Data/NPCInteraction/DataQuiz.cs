using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataQuiz : DataBase
    {
        public string accuracyText;
        public bool report = false;

        public string getAccuracyText()
        {
            return accuracyText + "%";
        }

        public void reset()
        {
            report = false;
        }
    }
}
