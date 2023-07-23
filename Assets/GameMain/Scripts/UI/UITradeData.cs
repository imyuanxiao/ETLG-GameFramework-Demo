using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;
using System.Linq;

namespace ETLG.Data
{
    public sealed class UITradeData
    {
        public int artifactID { get;  }
        //¿ÉÂôÊýÁ¿
        public int artifactNum { get;  }
        public int tradeType { get; }

        public UITradeData(int artifactID,int artifactNum,int tradeType)
        {
            this.artifactID = artifactID;
            this.artifactNum = artifactNum;
            this.tradeType = tradeType;
        }

        public int inputNum { get; set; }
        public int totalPrice { get; set; }
        public bool clickTradeButton { get; set; }
        public bool save { get; set; }



    }


}