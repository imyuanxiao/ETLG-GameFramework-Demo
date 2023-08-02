using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataQuiz : DataBase
    {
        public bool pass=false;
        public string accuracyText;
        public bool report = false;
        public bool again = false;
        public bool award = false;
        public bool clickGetButton = false;

        public string getAccuracyText()
        {
            return accuracyText + "%";
        }

        public void reset()
        {
            report = false;
            pass = false;
            award = false;
        }
    }
}
