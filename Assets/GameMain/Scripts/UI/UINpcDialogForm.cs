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
        private XmlNodeList playerResponses;
        private UINPCDialogNPCStatment UI_NPCDialogNPCStatment;
        private XmlNodeList NPCStatements;
        private List<UINPCDialogPlayerButton> playerButtons=new List<UINPCDialogPlayerButton>();
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
        private int fontsizeChangePlusValue = 0;
        private int fontsizeChangeSubValue = 0;
        private int fontsizeStandardOffset = 2;

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
        }

        private void OnOpenAlertForm()
        {
            GameEntry.Event.Fire(this, UIAlertTriggerEventArgs.Create(Constant.Type.UI_OPEN));

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
                        float videoWidth = video.GetComponentInChildren<VideoPlayer>().targetTexture.width;
                        float videoHeight = video.GetComponentInChildren<VideoPlayer>().targetTexture.height;

                        float videoRatio = videoHeight / videoWidth;
                        RectTransform dialogVideoRectTransform = video.GetComponent<RectTransform>();
                        dialogVideoRectTransform.sizeDelta = new Vector2(textWidth, textWidth * videoRatio);
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

            playerButtons = new List<UINPCDialogPlayerButton>();
            removePlayerResponseInput();

            foreach (XmlNode responseNode in playerResponses)
            {
                UINPCDialogPlayerButton newUIPlayerButton= new UINPCDialogPlayerButton();
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
                UI_NPCDialogNPCStatment = new UINPCDialogNPCStatment(node, npcData.Id.ToString());
                Image NPCModule = instantiatePrefab(UI_NPCDialogNPCStatment.NPCText, "NPC");
            }
            
            foreach (UINPCDialogPlayerButton button in playerButtons)
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
            dialogText.fontSize = default_fontsize + fontsizeChangePlusValue+fontsizeChangeSubValue;
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
            dialogText.fontSize = default_fontsize + UI_NPCDialogNPCStatment.fontSizeGap + fontsizeChangePlusValue + fontsizeChangeSubValue;
            dialogText.ForceMeshUpdate();

            dialogText.alignment = TextAlignmentOptions.Left;
            Transform contentContainer = textModuleRectTransform.Find("ContentContainer");

            if (UI_NPCDialogNPCStatment.isBold)
            {
                dialogText.fontStyle = FontStyles.Bold;
            }
            if (UI_NPCDialogNPCStatment.textColor != Color.white)
            {
                dialogText.color = UI_NPCDialogNPCStatment.textColor;
            }
            if (UI_NPCDialogNPCStatment.imagePath != null)
            {
                instantiateImage(contentContainer);
            }
            if (UI_NPCDialogNPCStatment.videoPath != null)
            {
                instantiateVideo(contentContainer);
            }

            
            return textModule;
        }

        private Canvas instantiateImage(Transform contentContainer)
        {
            Canvas imageModule = Instantiate(ImageContainerPrefab, contentContainer);
            Image image = imageModule.GetComponentInChildren<Image>();
            Sprite imageSprite = Resources.Load<Sprite>(UI_NPCDialogNPCStatment.imagePath);
            if (imageSprite != null)
            {
                image.sprite = imageSprite;
            }
            image.SetNativeSize();
            return imageModule;
        }

        private Canvas instantiateVideo(Transform contentContainer)
        {
            Canvas videoModule = Instantiate(VideoContainerPrefab, contentContainer);
            VideoPlayer videoPlayer = videoModule.GetComponentInChildren<VideoPlayer>();
            if (videoPlayer != null)
            {
                Debug.Log(UI_NPCDialogNPCStatment.videoPath);
                VideoClip videoClip = Resources.Load<VideoClip>(UI_NPCDialogNPCStatment.videoPath);

                // Load the video from the specified path
                videoPlayer.clip = videoClip;
            }
            else
            {
                Debug.Log("");
                Debug.LogError("VideoPlayer component not found on the GameObject named \"Video\".");
            }
            RawImage rawImage = videoModule.GetComponentInChildren<RawImage>();
            RenderTexture renderTexture = Resources.Load<RenderTexture>(UI_NPCDialogNPCStatment.videoTexture);
            rawImage.texture = renderTexture;
            videoPlayer.targetTexture = renderTexture;
            videoPlayer.Play();
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
            if (fontsizeChangePlusValue+ fontsizeChangeSubValue < 30)
            {
                fontsizeChangePlusValue += 2;
                updateFontSize(fontsizeStandardOffset);
            }
        }

        private void OnfontSub()
        {
            if (fontsizeChangePlusValue + fontsizeChangeSubValue > -10)
            {
                fontsizeChangeSubValue -= 2;
                updateFontSize(fontsizeStandardOffset*-1);
            }
        }

        private void updateFontSize(int changeValue)
        {
            Debug.Log("plus"+fontsizeChangePlusValue);
            Debug.Log("sub"+ fontsizeChangeSubValue);
            //Image singleTextModule in textModules
            foreach (Transform dialogModule in dialogScrollContent)
            {
                TextMeshProUGUI text = dialogModule.GetComponentInChildren<TextMeshProUGUI>();
                text.fontSize = text.fontSize + changeValue;
                VerticalLayoutGroup verticalLayoutGroup = dialogModule.GetComponentInChildren<VerticalLayoutGroup>();
                HorizontalLayoutGroup horizontalLayoutGroup = dialogModule.GetComponentInChildren<HorizontalLayoutGroup>();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)horizontalLayoutGroup.transform);
            }
            
        }

    }
}


