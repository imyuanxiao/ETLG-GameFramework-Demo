using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemNPCSelect : ItemLogicEx
    {

        public TextMeshProUGUI npc_name = null;

        public Button talkButton;

        public Button tradeButton;

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

            npc_name.text = npcData.Name;

            talkButton.onClick.AddListener(OnTalkButtonClick);
            tradeButton.onClick.AddListener(OnTradeButtonClick);

        }



        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            talkButton.onClick.RemoveAllListeners();
            tradeButton.onClick.RemoveAllListeners();

        }

        public void OnTalkButtonClick()
        {

            dataNPC.currentNPCId = npcData.Id;

            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.NPC_UI_TALK_OPEN));

        }

        public void OnTradeButtonClick()
        {

            dataNPC.currentNPCId = npcData.Id;

            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.NPC_UI_TRADE_OPEN));

        }

    }
}


