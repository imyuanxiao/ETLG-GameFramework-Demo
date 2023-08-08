using ETLG.Data;
using GameFramework.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemNPCSelect : ItemLogicEx
    {

        public TextMeshProUGUI npc_description = null;

        public RectTransform RewardIconContainer;


       // public GameObject RewardTick;
       // public GameObject FinishTick;

        public TextMeshProUGUI npc_name = null;

        public Button talkButton;
        public Button tradeButton;
        public Button quizButton;
        public HorizontalLayoutGroup horizontalLayoutGroup;

        private DataNPC dataNPC;
        private NPCData npcData;
        private DataLearningProgress dataLearningProgress;

        private DataPlayer dataPlayer;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataNPC = GameEntry.Data.GetData<DataNPC>();
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetNPCData(NPCData npcData)
        {
            this.npcData = npcData;

            npc_description.text = npcData.Chapter;
            npc_name.text = npcData.Name;

            talkButton.onClick.AddListener(OnTalkButtonClick);
            tradeButton.onClick.AddListener(OnTradeButtonClick);
            quizButton.onClick.AddListener(OnQuizButtonClick);

            talkButton.gameObject.SetActive(true);
            tradeButton.gameObject.SetActive(true);
            quizButton.gameObject.SetActive(true);


            if (npcData.DialogXML==0)
            {
                talkButton.gameObject.SetActive(false);
            }

            if (npcData.Artifacts.Length <= 1)
            {
                tradeButton.gameObject.SetActive(false);
            }

            if (npcData.QuizXML==0)
            {
                quizButton.gameObject.SetActive(false);
            }

            ShowItem<ItemRewardIcon>(EnumItem.ItemRewardIcon, (item) =>
            {
                item.transform.SetParent(RewardIconContainer, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = Vector3.zero;
                if (dataPlayer.GetPlayerData().getChapterFinish(npcData.Id))
                {
                    item.GetComponentInChildren<RawImage>().color = UIHexColor.HexToColor("990000");
                }
                else
                {
                    item.GetComponentInChildren<RawImage>().color = UIHexColor.HexToColor("FFFFFF");
                }
                item.GetComponent<ItemRewardIcon>().SetData(this.npcData.Id);
            });
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)horizontalLayoutGroup.transform);
            // get finished chapters from playerData
            //RewardTick.SetActive(false);
            //FinishTick.SetActive(false);

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            talkButton.onClick.RemoveAllListeners();
            tradeButton.onClick.RemoveAllListeners();
            quizButton.onClick.RemoveAllListeners();
        }

        public void OnTalkButtonClick()
        {
            Log.Debug("{0}", this.GetInstanceID());

            dataNPC.currentNPCId = npcData.Id;
            dataLearningProgress.open = false;

            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.NPC_UI_TALK_OPEN));

        }

        public void OnTradeButtonClick()
        {

            dataNPC.currentNPCId = npcData.Id;

            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.NPC_UI_TRADE_OPEN));

        }

        public void OnQuizButtonClick()
        {

            dataNPC.currentNPCId = npcData.Id;
            dataLearningProgress.open = false;
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.NPC_UI_QUIZ_OPEN));

        }

    /*    public void OnRewardButtonClick()
        {

            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(-10f, 0, 0);
            Vector3 newPosition = itemPosition + offset;

            dataNPC.currentNPCId = npcData.Id;
            dataNPC.RewardUIPosition = newPosition;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIRewardPreviewForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIRewardPreviewForm));
            }
            GameEntry.UI.OpenUIForm(EnumUIForm.UIRewardPreviewForm);
        }*/


    }
}


