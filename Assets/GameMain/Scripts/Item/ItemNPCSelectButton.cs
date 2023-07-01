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
    public class ItemNPCSelectButton : ItemLogicEx
    {

        public TextMeshProUGUI npc_name = null;

        public Button talkButton;

        public Button tradeButton;

        private NPCData npcData;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
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

            GameEntry.Data.GetData<DataNPC>().currentNPC = npcData.Id;

            // 点击事件
            Log.Debug("点击对话按钮");
            GameEntry.Event.Fire(this, NPCDialogOpenEventArgs.Create(Constant.Event.NPC_TALK));

        }

        public void OnTradeButtonClick()
        {

            GameEntry.Data.GetData<DataNPC>().currentNPC = npcData.Id;

            // 点击事件
            Log.Debug("点击交易按钮");
            GameEntry.Event.Fire(this, NPCDialogOpenEventArgs.Create(Constant.Event.NPC_TRADE));

        }

    }
}


