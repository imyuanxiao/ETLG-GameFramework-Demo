using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataAlert : DataBase
    {
        public int AlertType;
        public bool isFromBackend;

        public void clearData()
        {
            AlertType = -1;
        }

        public void setAlertData(int alertType)
        {
            AlertType = alertType;
        }
    }
}
