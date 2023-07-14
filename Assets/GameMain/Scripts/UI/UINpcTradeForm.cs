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

        private Dictionary<int, int> playerArtifacts;
        private Dictionary<int, int> npcArtifacts;


        private DataPlayer dataPlayer;
        private DataNPC dataNPC;

        private int tradeNum;

        private bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            playerArtifacts = dataPlayer.GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_TRADE);
            npcArtifacts = dataPlayer.GetPlayerData().GetNpcArtifactsByNpcId(dataNPC.currentNPCId);

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
                clearAllArtifacts();
                showContent();
                refresh = false;
            }
        }

        private void showContent()
        {
            npc_name.text = dataNPC.GetCurrentNPCData().Name;

            // need method Update()
            npc_money.text = dataPlayer.GetPlayerData().GetNpcDataById(dataNPC.currentNPCId).Money.ToString();

            player_money.text = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money).ToString();

            ShowPlayerArtifactIcons();
            ShowNPCArtifactIcons();
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


        private void ShowPlayerArtifactIcons()
        {
            foreach (KeyValuePair<int, int> kvp in playerArtifacts)
            {
                int ArtifactID = kvp.Key;
                int Num = kvp.Value;

                if (ArtifactID == (int)EnumArtifact.Money|| Num == 0)
                {
                    continue;
                }
                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(PlayerContainer, false);
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(ArtifactID, Num, Constant.Type.TRADE_PLAYER_NPC);
                    item.GetComponent<ItemArtifactIcon>().OnItemClicked += OnItemClickedFromIcon;
                });
            }
        }

        private void tradeArtifact(int artifactID, int tradeNum, int type)
        {
            if (type == Constant.Type.TRADE_NPC_PLAYER)
            {
                if(!testArtifactExist(playerArtifacts, artifactID, tradeNum, Constant.Type.ADD))
                {
                    playerArtifacts.Add(artifactID, tradeNum);
                }
                testArtifactExist(npcArtifacts, artifactID, tradeNum, Constant.Type.SUB);
            }
            else
            {
                if (!testArtifactExist(npcArtifacts, artifactID, tradeNum, Constant.Type.ADD))
                {
                    npcArtifacts.Add(artifactID, tradeNum);
                }
                testArtifactExist(playerArtifacts, artifactID, tradeNum, Constant.Type.SUB);
            }
        }

        private bool testArtifactExist(Dictionary<int, int> artifacts, int artifactID, int tradeNum, int calculateType)
        {
            bool isExist = false;
            foreach (KeyValuePair<int, int> kvp in artifacts)
            {
                if (kvp.Key == artifactID)
                {
                    int oldNum = kvp.Value;
                    if (calculateType == Constant.Type.ADD)
                    {
                        artifacts[artifactID] = oldNum + tradeNum;
                    }
                    else
                    {
                        artifacts[artifactID] = oldNum - tradeNum;
                    }
                    isExist = true;
                    return isExist;
                }
            }
            return isExist;
        }

        private void OnItemClickedFromIcon(int artifactID, int num, int type)
        {
            refresh = true;
            //改值
            tradeNum = num;

            //数量UI不能超过最大值
            tradeArtifact(artifactID, tradeNum, type);


        }

        private void ShowNPCArtifactIcons()
        {
            foreach (KeyValuePair<int, int> kvp in npcArtifacts)
            {
                int ArtifactID = kvp.Key;
                int Num = kvp.Value;

                if (ArtifactID == (int)EnumArtifact.Money|| Num == 0)
                {
                    continue;
                }

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(NpcContainer, false);
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(ArtifactID, Num, Constant.Type.TRADE_NPC_PLAYER);
                    item.GetComponent<ItemArtifactIcon>().OnItemClicked += OnItemClickedFromIcon;
                });
            }
        }
        private void clearAllArtifacts()
        {
            for (int i = NpcContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(NpcContainer.GetChild(i).gameObject);
            }

            for (int i = PlayerContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(PlayerContainer.GetChild(i).gameObject);
            }

        }
    }
}


