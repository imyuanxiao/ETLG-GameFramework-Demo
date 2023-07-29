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


        private DataNPC dataNPC;
        private NPCData npcData;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataNPC = GameEntry.Data.GetData<DataNPC>();
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


            if (npcData.NoDialogXML)
            {
                talkButton.gameObject.SetActive(false);
            }

            if (npcData.Artifacts.Length <= 1)
            {
                tradeButton.gameObject.SetActive(false);
            }

            if (npcData.NoQuizXML)
            {
                quizButton.gameObject.SetActive(false);
            }

            ShowItem<ItemRewardIcon>(EnumItem.ItemRewardIcon, (item) =>
            {
                item.transform.SetParent(RewardIconContainer, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = Vector3.zero;
                item.GetComponent<ItemRewardIcon>().SetData(this.npcData.Id);
            });

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


