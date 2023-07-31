﻿using ETLG.Data;
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
        public Button closeButton;
        public Button maxButton;
        public Button FontPlusButton;
        public Button FontSubButton;
        public RawImage dialogBg;
        public TextMeshProUGUI npc_name;
        public TextMeshProUGUI npc_description;
        public RawImage npc_avatar;
        public VerticalLayoutGroup verticalLayoutGroup;

        private UINPCDialogManager UI_NPCDialogManager;
        private Sprite NPCSprite;
        private Sprite playerSprite;
        private NPCData npcData;
        private DataPlayer dataPlayer;
        private DataAlert dataAlert;
        private string npcAvatarPath;
        private string XMLPath;
        private float playerInputBoxOriginalHeight=160f;
        private float dialogScrollContentOriginalHeight=600f;
        private RectTransform buttonScrollContentRectTransform;
        private RectTransform dialogScrollContentRectTransform;

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
            closeButton.onClick.AddListener(OnCloseButtonClick);
            maxButton.onClick.AddListener(OnMaxButtonClick);
            FontPlusButton.onClick.AddListener(OnfontPlus);
            FontSubButton.onClick.AddListener(OnfontSub);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();
            dataAlert = GameEntry.Data.GetData<DataAlert>();

            npc_name.text = npcData.Name;
            npcAvatarPath = AssetUtility.GetNPCAvatar(npcData.Id.ToString());
            npc_description.text = npcData.Domain + "\n" + npcData.Course + "\n" + npcData.Chapter;

            buttonScrollContentRectTransform = buttonScrollContent.GetComponent<RectTransform>();
            dialogScrollContentRectTransform = dialogScrollContent.GetComponent<RectTransform>();

            loadAvatar();
            UINPCDialogManager tempDialogManager = dataPlayer.GetPlayerData().getUINPCDialogById(npcData.Id);
            if (tempDialogManager == null)
            {
                XMLPath = npcData.DialogXML;
                UI_NPCDialogManager = new UINPCDialogManager(XMLPath);
                dataPlayer.GetPlayerData().setUINPCDialogById(npcData.Id, UI_NPCDialogManager);
            }
            else
            {
                UI_NPCDialogManager = tempDialogManager;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            RectTransform dialogBGTransfrom = dialogBg.GetComponent<RectTransform>();
            float currentdialogUIWidth = dialogBGTransfrom.sizeDelta.x;
            resizeDialog(currentdialogUIWidth);

            if ((UI_NPCDialogManager.nextNodeID != "end" && UI_NPCDialogManager.isNext) || (UI_NPCDialogManager.nextNodeID != "end" && UI_NPCDialogManager.currentNode == null))
            {
                if (UI_NPCDialogManager.dialogueNodes != null)
                {
                    getCurrentNode();
                    showText();
                }
                dataPlayer.GetPlayerData().setUINPCDialogById(npcData.Id,UI_NPCDialogManager);
            }
            else if (UI_NPCDialogManager.nextNodeID == "end" && UI_NPCDialogManager.isNext)
            {
                Debug.Log(UI_NPCDialogManager.nextNodeID);
                Debug.Log(buttonScrollContent.childCount);
                Debug.Log(UI_NPCDialogManager.isNext);
                removePlayerResponseInput();
                //检测有没有得过奖励
                if (!UI_NPCDialogManager.award)
                {
                    getAward();
                }
                UI_NPCDialogManager.isNext = false;
            }
            else if(UI_NPCDialogManager.nextNodeID == "end"&& buttonScrollContent.childCount==0)
            {
                againButton();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
            base.OnUpdate(elapseSeconds, realElapseSeconds);
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
            if (buttonScrollContent.childCount != 0)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIErrorMessageForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIErrorMessageForm));
                }
                dataAlert.AlertType = Constant.Type.ALERT_DIALOG_QUIT;
                GameEntry.UI.OpenUIForm(EnumUIForm.UIErrorMessageForm);
                return;
            }
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

        public void getCurrentNode()
        {
            UI_NPCDialogManager.isNext = false;

            if (UI_NPCDialogManager.currentNode == null)
            {
                UI_NPCDialogManager.currentNode = UI_NPCDialogManager.dialogueNodes["1"];
            }
            else
            {
                UI_NPCDialogManager.currentNode = UI_NPCDialogManager.dialogueNodes[UI_NPCDialogManager.nextNodeID];
            }

            UI_NPCDialogManager.NPCStatements = UI_NPCDialogManager.currentNode.SelectNodes("npc/statement");
            UI_NPCDialogManager.playerResponses = UI_NPCDialogManager.currentNode.SelectNodes("player/response");
            UI_NPCDialogManager.playerButtons = new List<UINPCDialogPlayerButton>();
            removePlayerResponseInput();

            foreach (XmlNode responseNode in UI_NPCDialogManager.playerResponses)
            {
                UINPCDialogPlayerButton newUIPlayerButton = new UINPCDialogPlayerButton();
                newUIPlayerButton.buttonText = responseNode.InnerText;
                newUIPlayerButton.nextNodeID = responseNode.Attributes["nextnode"].Value;

                if (responseNode.Attributes["isShown"] != null)
                {
                    newUIPlayerButton.isShown = false;
                }
                else
                {
                    newUIPlayerButton.isShown = true;
                }
                UI_NPCDialogManager.playerButtons.Add(newUIPlayerButton);
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

                    Transform dialogContainer = dialogModuleRectTransform.Find("ContentContainer");
                    RectTransform dialogContainerRectTransform = dialogContainer.GetComponent<RectTransform>();
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

        private void againButton()
        {
            Button playerButton = Instantiate(playerButtonPrefab, buttonScrollContent);
            RectTransform buttonRectTransform = playerButton.GetComponent<RectTransform>();

            playerButton.onClick.AddListener(() =>
            {
                Debug.Log("点击again");
                UI_NPCDialogManager.reset();
                removePlayerResponseInput();
                removeConversations();
                getCurrentNode();
                showText();
            });

            //选项文本加载
            TextMeshProUGUI buttonText = playerButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Start Again";
        }

 

        private void showText()
        {
            foreach (XmlNode node in UI_NPCDialogManager.NPCStatements)
            {
                UI_NPCDialogManager.UI_NPCDialogNPCStatment = new UINPCDialogNPCStatment(node, npcData.Id.ToString());
                Image NPCModule = instantiatePrefab(UI_NPCDialogManager.UI_NPCDialogNPCStatment.NPCText, "NPC");
            }

            foreach (UINPCDialogPlayerButton button in UI_NPCDialogManager.playerButtons)
            {
                Button playerButton = Instantiate(playerButtonPrefab, buttonScrollContent);
                RectTransform buttonRectTransform = playerButton.GetComponent<RectTransform>();

                playerButton.onClick.AddListener(() =>
                {
                    UI_NPCDialogManager.nextNodeID = button.nextNodeID;
                    UI_NPCDialogManager.isNext = true;
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
            for (int i = buttonScrollContent.childCount - 1; i >= 0; i--)
            {
                Destroy(buttonScrollContent.GetChild(i).gameObject);
            }
            buttonScrollContentRectTransform.sizeDelta = new Vector2(buttonScrollContentRectTransform.sizeDelta.x, playerInputBoxOriginalHeight);
        }

        //清除所有聊天记录
        private void removeConversations()
        {
            for (int i = dialogScrollContentRectTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(dialogScrollContentRectTransform.GetChild(i).gameObject);
            }
            dialogScrollContentRectTransform.sizeDelta = new Vector2(dialogScrollContentRectTransform.sizeDelta.x, dialogScrollContentOriginalHeight);
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
            UI_NPCDialogManager.textModules.Add(textModule);
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
            dialogText.fontSize = default_fontsize + fontsizeChangePlusValue + fontsizeChangeSubValue;
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
            dialogText.fontSize = default_fontsize + UI_NPCDialogManager.UI_NPCDialogNPCStatment.fontSizeGap + fontsizeChangePlusValue + fontsizeChangeSubValue;
            dialogText.ForceMeshUpdate();

            dialogText.alignment = TextAlignmentOptions.Left;
            Transform contentContainer = textModuleRectTransform.Find("ContentContainer");

            if (UI_NPCDialogManager.UI_NPCDialogNPCStatment.isBold)
            {
                dialogText.fontStyle = FontStyles.Bold;
            }
            if (UI_NPCDialogManager.UI_NPCDialogNPCStatment.textColor != Color.white)
            {
                dialogText.color = UI_NPCDialogManager.UI_NPCDialogNPCStatment.textColor;
            }
            if (UI_NPCDialogManager.UI_NPCDialogNPCStatment.imagePath != null)
            {
                instantiateImage(contentContainer);
            }
            if (UI_NPCDialogManager.UI_NPCDialogNPCStatment.videoPath != null)
            {
                instantiateVideo(contentContainer);
            }
            return textModule;
        }

        private Canvas instantiateImage(Transform contentContainer)
        {
            Canvas imageModule = Instantiate(ImageContainerPrefab, contentContainer);
            Image image = imageModule.GetComponentInChildren<Image>();
            Sprite imageSprite = Resources.Load<Sprite>(UI_NPCDialogManager.UI_NPCDialogNPCStatment.imagePath);
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
                Debug.Log(UI_NPCDialogManager.UI_NPCDialogNPCStatment.videoPath);
                VideoClip videoClip = Resources.Load<VideoClip>(UI_NPCDialogManager.UI_NPCDialogNPCStatment.videoPath);

                // Load the video from the specified path
                videoPlayer.clip = videoClip;
            }
            else
            {
                Debug.LogError("VideoPlayer component not found on the GameObject named \"Video\".");
            }
            RawImage rawImage = videoModule.GetComponentInChildren<RawImage>();
            RenderTexture renderTexture = Resources.Load<RenderTexture>(UI_NPCDialogManager.UI_NPCDialogNPCStatment.videoTexture);
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
            UI_NPCDialogManager.award = true;
        }

        private void OnfontPlus()
        {
            if (fontsizeChangePlusValue + fontsizeChangeSubValue < 30)
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
                updateFontSize(fontsizeStandardOffset * -1);
            }
        }

        private void updateFontSize(int changeValue)
        {
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


