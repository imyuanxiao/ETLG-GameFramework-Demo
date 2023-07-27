using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ETLG.Data
{
    public sealed class UINPCDialogNPCStatment
    {
        public UINPCDialogNPCStatment(XmlNode currentNPCNode,string NPCId)
        {
            NPCText = currentNPCNode.InnerText;

            if (currentNPCNode.Attributes["font"] != null)
            {
                fontSizeGap = int.Parse(currentNPCNode.Attributes["font"].Value) * 4;
            }
            else
            {
                fontSizeGap = 0;
            }

            if (currentNPCNode.Attributes["color"] != null)
            {
                textColor = UIHexColor.HexToColor(currentNPCNode.Attributes["color"].Value);
            }
            else
            {
                textColor = Color.white;
            }

            if (currentNPCNode.Attributes["image"] != null)
            {
                imagePath = AssetUtility.GetXMLImage(NPCId, currentNPCNode.Attributes["image"].Value.ToString());
            }
            else
            {
                imagePath = null;
            }

            if (currentNPCNode.Attributes["video"] != null)
            {
                videoPath = AssetUtility.GetXMLVideo(NPCId, currentNPCNode.Attributes["video"].Value.ToString());
                videoTexture = AssetUtility.GetXMLVideoRender(NPCId, currentNPCNode.Attributes["video"].Value.ToString());
            }
            else
            {
                videoPath = null;
                videoTexture = null;
            }

            if (currentNPCNode.Attributes["isBold"] != null)
            {
                isBold = true;
            }
            else
            {
                isBold = false;
            }
        }

        public int fontSizeGap = 0;
        public Color textColor = Color.white;
        public string imagePath = null;
        public string videoPath = null;
        public string videoTexture = null;
        public bool isBold = false;
        public string NPCText = null;

    }
}
