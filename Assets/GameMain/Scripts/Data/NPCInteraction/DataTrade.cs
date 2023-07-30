using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
namespace ETLG.Data
{
    public class DataTrade : DataBase
    {
        public int artifactID;
        //¿ÉÂôÊýÁ¿
        public int artifactNum;

        public int tradeType;
        public int inputNum;
        public int totalPrice;
        public bool clickTradeButton;
        public bool save;
        public bool clickItemIcon;

        public void clearData()
        {
            artifactID = -1;
            artifactNum = 0;
            tradeType = -1;
            inputNum = 0;
            totalPrice = 0;
            clickTradeButton = false;
            save = false;
            clickItemIcon = false;
        }

        public void setArtifactData(int artifactID,int artifactNum,int tradeType)
        {
            this.artifactID = artifactID;
            this.artifactNum = artifactNum;
            this.tradeType = tradeType;
            clickItemIcon = true; 
        }

        public void setTradeData(int inputNum, int totalPrice)
        {
            this.inputNum = inputNum;
            this.totalPrice = totalPrice;
            this.save = true;
            this.clickTradeButton = true;
        }
    }
}
