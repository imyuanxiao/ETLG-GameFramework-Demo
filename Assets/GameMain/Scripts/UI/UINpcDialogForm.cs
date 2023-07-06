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
    public class UINpcDialogForm : UGuiFormEx
    {
        public Image dialogModulePrefab;
        public Button playerButtonPrefab;
        public Transform dialogScrollContent;
        public Transform buttonScrollContent;

        public Button closeButton;
        public Button maxButton;
        public RawImage dialogBg;

        public TextMeshProUGUI npc_name;
        public TextMeshProUGUI npc_description;
        public RawImage npc_avatar;
        public VerticalLayoutGroup verticalLayoutGroup;

        private Sprite NPCSprite;
        private Sprite playerSprite;
        private NPCData npcData;
        private string npcAvatarPath;
        private string XMLPath;
        private Dictionary<string, XmlNode> dialogueNodes;
        private XmlNode currentNode = null;
        private string nextNodeID = null;
        private string NPCText;
        private string playerText;
        private XmlNodeList playerResponses;
        private Dictionary<string, string> playerButtons;
        private bool isNext = false;
        private float playerInputBoxOriginalHeight;
        private RectTransform buttonScrollContentRectTransform;

        private const float min_contentWidth = 1100f;
        private const float min_prefabWidth = 1040f;
        private const float min_textWidth = 820f;
        private const float max_contentWidth = 1830f;
        private const float max_prefabWidth = 1760f;
        private const float max_textWidth = 1540f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
            maxButton.onClick.AddListener(OnMaxButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();

            npc_name.text = npcData.Name;
            npcAvatarPath = npcData.Avatar;
            npc_description.text = npcData.Description;
            XMLPath = npcData.XMLSource;

            buttonScrollContentRectTransform = buttonScrollContent.GetComponent<RectTransform>();

            loadAvatar();
            parseXMLFile();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            RectTransform dialogBGTransfrom = dialogBg.GetComponent<RectTransform>();
            float currentdialogUIWidth = dialogBGTransfrom.sizeDelta.x;
            resizeDialog(currentdialogUIWidth);

            if ((nextNodeID != "end" && isNext) || (nextNodeID != "end" && currentNode == null))
            {
                getCurrentNode();
                showText(currentdialogUIWidth);
            }
            else if (nextNodeID == "end" && isNext)
            {
                removePlayerResponseInput();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
        }

        //读取头像数据
        private void loadAvatar()
        {
            Texture2D NPCTexture = Resources.Load<Texture2D>(npcAvatarPath);
            NPCSprite = Sprite.Create(NPCTexture, new Rect(0, 0, NPCTexture.width, NPCTexture.height), Vector2.one * 0.5f);
            npc_avatar.texture = NPCTexture;

            Texture2D playerTexture = Resources.Load<Texture2D>("Avatar/1012");
            playerSprite = Sprite.Create(playerTexture, new Rect(0, 0, playerTexture.width, playerTexture.height), Vector2.one * 0.5f);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }


        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, NPCUICloseEventArgs.Create());
            this.Close();
        }

        //页面最大化
        private void OnMaxButtonClick()
        {
            Debug.Log("点击放大");
            //bgm
            RectTransform dialogBGTransfrom = dialogBg.GetComponent<RectTransform>();
            float currentdialogUIWidth = dialogBGTransfrom.sizeDelta.x;
            float currentdialogUIHeight = dialogBGTransfrom.sizeDelta.y;
            //1830f 1760 1540
            if (currentdialogUIWidth == min_contentWidth)
            {
                dialogBGTransfrom.sizeDelta = new Vector2(max_contentWidth, currentdialogUIHeight);
                resizeDialog(max_prefabWidth, max_textWidth);
            }
            //1100 1040 820
            else
            {
                dialogBGTransfrom.sizeDelta = new Vector2(min_contentWidth, currentdialogUIHeight);
                resizeDialog(min_prefabWidth, min_textWidth);
            }
        }

        //修改聊天记录宽度
        private void resizeDialog(float prefabWidth, float textWidth)
        {
            foreach (Transform dialogModule in dialogScrollContent)
            {
                RectTransform dialogModuleRectTransform = dialogModule.GetComponent<RectTransform>();
                if (dialogModuleRectTransform != null)
                {
                    Vector2 newContentSize = dialogModuleRectTransform.sizeDelta;
                    newContentSize.x = prefabWidth;
                    dialogModuleRectTransform.sizeDelta = newContentSize;

                    TextMeshProUGUI dialogTextUI = dialogModuleRectTransform.GetComponentInChildren<TextMeshProUGUI>();
                    RectTransform dialogTextUIRectTransform = dialogTextUI.GetComponent<RectTransform>();
                    dialogTextUIRectTransform.sizeDelta = new Vector2(textWidth, dialogTextUIRectTransform.sizeDelta.y);
                }
            }
        }
        //根据当前页面大小，统一聊天记录大小
        private void resizeDialog(float currentWidth)
        {
            float correct_prefabWidth;
            float correct_textWidth;
            if (currentWidth == min_contentWidth)
            {
                correct_prefabWidth = min_prefabWidth;
                correct_textWidth = min_textWidth;
            }
            else
            {
                correct_prefabWidth = max_prefabWidth;
                correct_textWidth = max_textWidth;
            }
            resizeDialog(correct_prefabWidth, correct_textWidth);
        }


        //读取对话XML文件
        private void parseXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLPath);

            dialogueNodes = new Dictionary<string, XmlNode>();

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("node");

            foreach (XmlNode node in nodes)
            {
                string nodeId = node.Attributes["id"].Value;
                dialogueNodes.Add(nodeId, node);
            }
        }

        private void getCurrentNode()
        {
            isNext = false;

            if (currentNode == null)
            {
                currentNode = dialogueNodes["1"];
            }
            else
            {
                currentNode = dialogueNodes[nextNodeID];
            }

            NPCText = currentNode.SelectSingleNode("npc").InnerText;

            playerResponses = currentNode.SelectNodes("player/response");

            playerButtons = new Dictionary<string, string>();
            removePlayerResponseInput();

            foreach (XmlNode responseNode in playerResponses)
            {
                playerText = responseNode.InnerText;
                nextNodeID = responseNode.Attributes["nextnode"].Value;
                playerButtons.Add(playerText, nextNodeID);
            }
        }

        private void showText(float width)
        {
            Image NPCModule = instantiatePrefab(NPCText, "NPC");

            foreach (KeyValuePair<string, string> data in playerButtons)
            {
                Button playerButton = Instantiate(playerButtonPrefab, buttonScrollContent);
                RectTransform buttonRectTransform = playerButton.GetComponent<RectTransform>();

                playerButton.onClick.AddListener(() =>
                {
                    nextNodeID = data.Value;
                    isNext = true;
                    instantiatePrefab(data.Key, "player");
                });

                //选项文本加载
                TextMeshProUGUI buttonText = playerButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = data.Key;
            }
        }

        //清除玩家输入框所有按钮
        private void removePlayerResponseInput()
        {
            buttonScrollContentRectTransform.sizeDelta = new Vector2(buttonScrollContentRectTransform.sizeDelta.x, playerInputBoxOriginalHeight);
            for (int i = buttonScrollContent.childCount - 1; i >= 0; i--)
            {
                Destroy(buttonScrollContent.GetChild(i).gameObject);
            }

        }

        //分别实例化NPC和玩家的聊天记录prefab,并加载文本
        private Image instantiatePrefab(string text, string type)
        {
            Image textModule;
            RectTransform textModuleRectTransform;
            TextMeshProUGUI dialogText;
            if (type == "NPC")
            {
                textModule = Instantiate(dialogModulePrefab, dialogScrollContent);
                textModuleRectTransform = textModule.GetComponent<RectTransform>();
                setColorAlpha(textModuleRectTransform, "NPC");
                dialogText = textModule.GetComponentInChildren<TextMeshProUGUI>();
                dialogText.text = text;
                dialogText.alignment = TextAlignmentOptions.Left;
            }
            else
            {
                textModule = Instantiate(dialogModulePrefab, dialogScrollContent);
                textModuleRectTransform = textModule.GetComponent<RectTransform>();
                setColorAlpha(textModuleRectTransform, "player");
                dialogText = textModule.GetComponentInChildren<TextMeshProUGUI>();
                dialogText.text = text;
                dialogText.alignment = TextAlignmentOptions.Right;
            }
            //加载文本,设置对齐方式



            return textModule;
        }

        //加载对话模块对应头像
        private void setColorAlpha(RectTransform textModuleRectTransform, string type)
        {
            Image NPCImage = textModuleRectTransform.Find("LeftAvatar").GetComponent<Image>();
            Image playerImage = textModuleRectTransform.Find("RightAvatar").GetComponent<Image>();
            Image shownImage;
            Image notShownImage;
            Color tempColor;
            if (type == "NPC")
            {
                shownImage = NPCImage;
                notShownImage = playerImage;
                shownImage.sprite = NPCSprite;
            }
            else
            {
                shownImage = playerImage;
                notShownImage = NPCImage;
                shownImage.sprite = playerSprite;
            }

            tempColor = notShownImage.color;
            tempColor.a = 0f;
            notShownImage.color = tempColor;
        }
    }
}


