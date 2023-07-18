using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Xml;

namespace ETLG
{
    public class UINpcQuizForm : UGuiFormEx
    {

        private NPCData npcData;
        private string npcAvatarPath;
        private string XMLPath= "Assets/GameMain/Res/QuizXML/CloudComputing/Basic/B.The History and Evolution of the Cloud.xml";
        private Dictionary<string, XmlNode> quizNodes;

        public TextMeshProUGUI npc_name;
        public TextMeshProUGUI npc_description;
        public RawImage npc_avatar;
        public TextMeshProUGUI statement;

        public Button closeButton;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();

            npc_name.text = npcData.Name;
            npcAvatarPath = npcData.Avatar;
            npc_description.text = npcData.Description;
            XMLPath = npcData.XMLQuizSource;

            loadAvatar();
            parseXMLFile();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        private void loadAvatar()
        {
            Texture2D NPCTexture = Resources.Load<Texture2D>(npcAvatarPath);
            npc_avatar.texture = NPCTexture;
        }

        private void parseXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLPath);

            quizNodes = new Dictionary<string, XmlNode>();

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("question");

            foreach (XmlNode node in nodes)
            {
                string nodeId = node.Attributes["id"].Value;
                quizNodes.Add(nodeId, node);
            }
        }
    }
}
