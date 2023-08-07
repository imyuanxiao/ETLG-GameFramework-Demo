using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace ETLG.Data
{
    public sealed class UINPCDialogManager
    {
        public bool award = false;

        public List<Image> textModules = null;
        public XmlNode currentNode = null;
        public Dictionary<string, XmlNode> dialogueNodes = null;
        public string nextNodeID = null;
        public XmlNodeList playerResponses;
        public XmlNodeList NPCStatements;
        public UINPCDialogNPCStatment UI_NPCDialogNPCStatment;
        public List<UINPCDialogPlayerButton> playerButtons = null;

        public bool isNext = false;

        public void reset()
        {
            textModules = new List<Image>();
            currentNode = null;
            nextNodeID = null;
            playerResponses = null;
            NPCStatements = null;
            UI_NPCDialogNPCStatment = null;
            playerButtons = null;
            isNext = false;
        }
        public UINPCDialogManager(string XMLPath,bool award)
        {
            this.award = award;
            textModules = new List<Image>();
            parseXMLFile(XMLPath);
        }
        //读取对话XML文件
        private void parseXMLFile(string XMLPath)
        {
            TextAsset xmlFile = Resources.Load<TextAsset>(XMLPath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);

            dialogueNodes = new Dictionary<string, XmlNode>();

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("node");

            foreach (XmlNode node in nodes)
            {
                string nodeId = node.Attributes["id"].Value;
                dialogueNodes.Add(nodeId, node);
            }
        }
    }
}
