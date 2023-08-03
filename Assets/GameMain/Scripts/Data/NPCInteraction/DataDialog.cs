using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataDialog : DataBase
    {
        public bool finish=false;
        public bool award=false;
        public bool clickGetButton=false;

        public void reset()
        {
            finish = false;
            award = false;
            clickGetButton = false;
        }
    }
}
