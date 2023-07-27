using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Xml;
using UnityEngine.Video;
using UnityEditor.Rendering;

namespace ETLG
{
    public class UINpcDialogForm : UGuiFormEx
    {
        public Image dialogModulePrefab;
        public Canvas ImageContainerPrefab;
        public Canvas VideoContainerPrefab;
        public Button playerButtonPrefab;
        public Transform dialogScrollContent;
        public Transform buttonScrollContent;
        private List<Image> textModules = new List<Image>();

        public Button closeButton;
        public Button maxButton;
        public Button FontPlusButton;
        public Button FontSubButton;
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
        private XmlNodeList playerResponses;
        private XmlNodeList NPCStatements;
        private List<UIPlayerButton> playerButtons=new List<UIPlayerButton>();
        private bool isNext = false;
        private float playerInputBoxOriginalHeight;
        private RectTransform buttonScrollContentRectTransform;

        private const float min_contentWidth = 1100f;
        private const float min_prefabWidth = 1040f;
        private const float min_textWidth = 820f;
        private const float max_contentWidth = 1830f;
        private const float max_prefabWidth = 1760f;
        private const float max_textWidth = 1540f;
        private int default_fontsize = 20;
        private int fontsizeChangeValue = 0;

        private int fontSizeGap = 0;
        private Color textColor = Color.white;
        private string imagePath = null;
        private string videoPath = null;
        private string videoTexture = null;
        private bool isBold = false;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
            maxButton.onClick.AddListener(OnMaxButtonClick);
            FontPlusButton.onClick.AddListener(OnfontPlus);
            FontSubButton.onClick.AddListener(OnfontSub);

            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();

            npc_name.text = npcData.Name;
            npcAvatarPath = AssetUtility.GetNPCAvatar(npcData.Id.ToString());
            npc_description.text = npcData.Domain+"\n"+npcData.Course+"\n"+npcData.Chapter;
            XMLPath = npcData.DialogXML;

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
                showText();
            }
            else if (nextNodeID == "end" && isNext)
            {
                removePlayerResponseInput();

                //检测有没有得过奖励
                //if (!award)
                //{
                //    getAward();
                //}
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
            updateFontSize();
        }

        //读取头像数据
        private void loadAvatar()
        {
            Texture2D NPCTexture = Resources.Load<Texture2D>(npcAvatarPath);
            if (NPCTexture == null)
            {
                NPCTexture = Resources.Load<Texture2D>(AssetUtility.GetAvatarMissing());
            }
            NPCSprite = Sprite.Create(NPCTexture, new Rect(0, 0, NPCTexture.width, NPCTexture.height), Vector2.one * 0.5f);
            npc_avatar.texture = NPCTexture;

            Texture2D playerTexture = Resources.Load<Texture2D>(AssetUtility.GetPlayerAvatar());
            if (playerTexture == null)
            {
                playerTexture = Resources.Load<Texture2D>(AssetUtility.GetAvatarMissing());
            }
            playerSprite = Sprite.Create(playerTexture, new Rect(0, 0, playerTexture.width, playerTexture.height), Vector2.one * 0.5f);
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

        //页面最大化
        private void OnMaxButtonClick()
        {
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

                    Transform dialogContainer=dialogModuleRectTransform.Find("ContentContainer");
                    RectTransform dialogContainerRectTransform= dialogContainer.GetComponent<RectTransform>();
                    dialogContainerRectTransform.sizeDelta = new Vector2(textWidth, dialogContainerRectTransform.sizeDelta.y);

                    TextMeshProUGUI dialogTextUI = dialogModuleRectTransform.GetComponentInChildren<TextMeshProUGUI>();
                    RectTransform dialogTextUIRectTransform = dialogTextUI.GetComponent<RectTransform>();
                    dialogTextUIRectTransform.sizeDelta = new Vector2(textWidth, dialogTextUIRectTransform.sizeDelta.y);

                    Image image = dialogContainer.GetComponentInChildren<Image>();
                    if (image != null)
                    {
                        RectTransform dialogImageRectTransform = image.GetComponent<RectTransform>();
                        float ratio = dialogImageRectTransform.sizeDelta.y / dialogImageRectTransform.sizeDelta.x;
                        dialogImageRectTransform.sizeDelta = new Vector2(textWidth, textWidth * ratio);
                    }

                    VideoPlayer video = dialogContainer.GetComponentInChildren<VideoPlayer>();
                    if (video != null)
                    {
                        RectTransform dialogVideoRectTransform = video.GetComponent<RectTransform>();
                        dialogVideoRectTransform.sizeDelta = new Vector2(textWidth, textWidth * 0.75f);
                    }
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

        private void getFeatures(XmlNode currentNPCNode)
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
                imagePath = AssetUtility.GetXMLImage(npcData.Id.ToString(), currentNPCNode.Attributes["image"].Value.ToString());
            }
            else
            {
                imagePath = null;
            }

            if (currentNPCNode.Attributes["video"] != null)
            {
                videoPath = AssetUtility.GetXMLVideo(currentNPCNode.Attributes["video"].Value.ToString());
                videoTexture = AssetUtility.GetXMLVideoRender(currentNPCNode.Attributes["video"].Value.ToString());
            }
            else
            {
                videoPath = null;
                videoTexture = null;
            }

            if(currentNPCNode.Attributes["isBold"] != null)
            {
                isBold = true;
            }
            else
            {
                isBold = false;
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

            NPCStatements = currentNode.SelectNodes("npc/statement");

            playerResponses = currentNode.SelectNodes("player/response");

            playerButtons = new List<UIPlayerButton>();
            removePlayerResponseInput();

            foreach (XmlNode responseNode in playerResponses)
            {
                UIPlayerButton newUIPlayerButton= new UIPlayerButton();
                newUIPlayerButton.buttonText= responseNode.InnerText;
                newUIPlayerButton.nextNodeID = responseNode.Attributes["nextnode"].Value;

                if (responseNode.Attributes["isShown"] != null)
                {
                    newUIPlayerButton.isShown = false;
                }
                else
                {
                    newUIPlayerButton.isShown = true;
                }
                playerButtons.Add(newUIPlayerButton);
            }
        }

        private void showText()
        {
            foreach (XmlNode node in NPCStatements)
            {
                getFeatures(node);
                Image NPCModule = instantiatePrefab(NPCText, "NPC");
            }
            
            foreach (UIPlayerButton button in playerButtons)
            {
                Button playerButton = Instantiate(playerButtonPrefab, buttonScrollContent);
                RectTransform buttonRectTransform = playerButton.GetComponent<RectTransform>();

                playerButton.onClick.AddListener(() =>
                {
                    nextNodeID = button.nextNodeID;
                    isNext = true;
                    if (button.isShown)
                    {
                        instantiatePrefab(button.buttonText, "player");
                    }
                });

                //选项文本加载
                TextMeshProUGUI buttonText = playerButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = button.buttonText;
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
            if (type == "NPC")
            {
                textModule = NPCModuleSet(text);
            }
            else
            {
                textModule = playerModuleSet(text);
            }
            textModules.Add(textModule);
            //加载文本,设置对齐方式
            return textModule;
        }

        private Image playerModuleSet(string text)
        {
            Image textModule = Instantiate(dialogModulePrefab, dialogScrollContent);
            Color color = UIHexColor.HexToColor("383838");
            textModule.color = color;
            RectTransform textModuleRectTransform = textModule.GetComponent<RectTransform>();
            setColorAlpha(textModuleRectTransform, "player");
            TextMeshProUGUI dialogText = textModule.GetComponentInChildren<TextMeshProUGUI>();
            dialogText.fontSize = default_fontsize + fontsizeChangeValue;
            dialogText.text = text;
            dialogText.alignment = TextAlignmentOptions.Right;
            return textModule;
        }

        private Image NPCModuleSet(string text)
        {
            Image textModule = Instantiate(dialogModulePrefab, dialogScrollContent);
            Color color = UIHexColor.HexToColor("4E4E4E");
            textModule.color = color;
            RectTransform textModuleRectTransform = textModule.GetComponent<RectTransform>();
            setColorAlpha(textModuleRectTransform, "NPC");
            TextMeshProUGUI dialogText = textModule.GetComponentInChildren<TextMeshProUGUI>();
            dialogText.text = text;
            dialogText.fontSize = default_fontsize + fontSizeGap + fontsizeChangeValue;
            dialogText.ForceMeshUpdate();

            Debug.Log(dialogText.fontSize);
            dialogText.alignment = TextAlignmentOptions.Left;
            Transform contentContainer = textModuleRectTransform.Find("ContentContainer");

            if (isBold)
            {
                dialogText.fontStyle = FontStyles.Bold;
            }
            if (textColor != Color.white)
            {
                dialogText.color = textColor;
            }
            if (imagePath != null)
            {
                instantiateImage(contentContainer);
            }
            if (videoPath != null)
            {
                instantiateVideo(contentContainer);
            }

            
            return textModule;
        }

        private Canvas instantiateImage(Transform contentContainer)
        {
            Canvas imageModule = Instantiate(ImageContainerPrefab, contentContainer);
            Image image = imageModule.GetComponentInChildren<Image>();
            Texture2D imageTexture = Resources.Load<Texture2D>(imagePath);

            //set original ratio
            float aspectRatio = (float)imageTexture.width / imageTexture.height;
            float fixedHeight = min_textWidth / aspectRatio;

            Sprite sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));
            if (imageTexture != null)
            {
                // Set the loaded sprite to the target Image component
                image.sprite = sprite;
                RectTransform imageTransform = image.GetComponent<RectTransform>();
                imageTransform.sizeDelta = new Vector2(min_textWidth, fixedHeight);
            }
            return imageModule;
        }

        private Canvas instantiateVideo(Transform contentContainer)
        {
            Canvas videoModule = Instantiate(VideoContainerPrefab, contentContainer);
            VideoPlayer videoPlayer = videoModule.GetComponentInChildren<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPath = "Assets/Resources/" + videoPath;
                // Load the video from the specified path
                videoPlayer.url = videoPath;
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("VideoPlayer component not found on the GameObject named \"Video\".");
            }
            RawImage rawImage = videoModule.GetComponentInChildren<RawImage>();
            RenderTexture renderTexture = Resources.Load<RenderTexture>(videoTexture);
            rawImage.texture = renderTexture;
            videoPlayer.targetTexture = renderTexture;
            return videoModule;
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

        private void getAward()
        {

        }

        private void OnfontPlus()
        {
            if (default_fontsize+ fontsizeChangeValue < 50)
            {
                fontsizeChangeValue += 2;
            }
        }

        private void OnfontSub()
        {
            if (default_fontsize+ fontsizeChangeValue > 10)
            {
                fontsizeChangeValue -= 2;
            }
        }

        private void updateFontSize()
        {
            //Image singleTextModule in textModules
            
            foreach (Transform dialogModule in dialogScrollContent)
            {
                TextMeshProUGUI text = dialogModule.GetComponentInChildren<TextMeshProUGUI>();
                text.fontSize = text.fontSize + fontsizeChangeValue;
                VerticalLayoutGroup verticalLayoutGroup = dialogModule.GetComponentInChildren<VerticalLayoutGroup>();
                HorizontalLayoutGroup horizontalLayoutGroup = dialogModule.GetComponentInChildren<HorizontalLayoutGroup>();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)horizontalLayoutGroup.transform);
            }
            
        }

    }
}


