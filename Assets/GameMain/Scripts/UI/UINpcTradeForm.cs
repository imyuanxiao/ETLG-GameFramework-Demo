using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UINpcTradeForm : UGuiFormEx
    {

        public Button closeButton;

        public Transform NpcContainer;
        public Transform PlayerContainer;

        public TextMeshProUGUI npc_name = null;
        public TextMeshProUGUI npc_money = null;

        public TextMeshProUGUI player_money = null;


        private DataPlayer dataPlayer;
        private DataNPC dataNPC;

        private bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();

            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            refresh = true;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (refresh)
            {
                showContent();
                refresh = false;
            }
        }

        private void showContent()
        {
            npc_name.text = dataNPC.GetCurrentNPCData().Name;

            // need method Update()
            npc_money.text = dataPlayer.GetPlayerData().getNpcDataById(dataNPC.currentNPCId).Money.ToString();

            player_money.text = dataPlayer.GetPlayerData().getArtifactNumById((int)EnumArtifact.Money).ToString();

            ShowPlayerArtifactIcons(PlayerContainer, Constant.Type.ARTIFACT_TRADE);
            ShowNPCArtifactIcons(NpcContainer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
            this.Close();

        }


        private void ShowPlayerArtifactIcons(Transform container, int type)
        {

            List<PlayerArtifactData> playerArtifacts = dataPlayer.GetPlayerData().getArtifactsByType(type);

            for (int i = 0; i < playerArtifacts.Count; i++)
            {

                Vector3 offset = new Vector3((i % 4) * 100f, (i / 4) * (-110f), 0f);

                PlayerArtifactData playerArtifact = playerArtifacts[i];

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(playerArtifact, Constant.Type.TRADE_PLAYER_NPC);
                });

            }
        }

        private void ShowNPCArtifactIcons(Transform container)
        {

            List<PlayerArtifactData> npcArtifacts = dataPlayer.GetPlayerData().getNpcArtifacts(dataNPC.currentNPCId);

            for (int i = 0; i < npcArtifacts.Count; i++)
            {

                Vector3 offset = new Vector3((i % 4) * 100f, (i / 4) * (-110f), 0f);

                PlayerArtifactData playerArtifact = npcArtifacts[i];

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(playerArtifact, Constant.Type.TRADE_NPC_PLAYER);
                });

            }
        }

    }
}


