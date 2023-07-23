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

        private bool refresh;

        //可买卖数量
        private int totalNum;
        //输入的数量
        private int tradeNum;
        private int npcMoney;
        private int playerMoney;
        //总消费金额
        private int totalPrice;
        //道具数量上限
        private int limitNum;
        private int receivedArtifactID;
        //交易买卖方类型
        private int receivedType;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            
            base.OnOpen(userData);
            closeButton.onClick.AddListener(OnCloseButtonClick);

            loadArtifactsData();
            refresh = true;
        }

        private void loadArtifactsData()
        {
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            playerArtifacts = dataPlayer.GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_TRADE);
            npcArtifacts = dataPlayer.GetPlayerData().GetNpcArtifactsByNpcId(dataNPC.currentNPCId);
            npcMoney = dataPlayer.GetPlayerData().GetNpcDataById(dataNPC.currentNPCId).Money;
            playerMoney = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (dataPlayer.GetPlayerData().UI_tradeData!=null&& dataPlayer.GetPlayerData().UI_tradeData.clickTradeButton)
            {
                HandleTradeData();
            }
            if (refresh)
            {
                HideAllItem();
                showContent();
                refresh = false;
                updateArtifactData();
            }
        }

        //触发trade按钮后交易，刷新item和money
        private void HandleTradeData()
        {
            tradeNum = dataPlayer.GetPlayerData().UI_tradeData.inputNum;
            totalPrice = dataPlayer.GetPlayerData().UI_tradeData.totalPrice;
            receivedArtifactID= dataPlayer.GetPlayerData().UI_tradeData.artifactID;
            receivedType = dataPlayer.GetPlayerData().UI_tradeData.tradeType;
            totalNum = dataPlayer.GetPlayerData().UI_tradeData.artifactNum;
            dataPlayer.GetPlayerData().UI_tradeData.save = true;

            tradeArtifact();
            refresh = true;

        }

        private void showContent()
        {
            npc_name.text = dataNPC.GetCurrentNPCData().Name;

            npc_money.text = npcMoney.ToString();
            player_money.text = playerMoney.ToString();

            ShowPlayerArtifactIcons();
            ShowNPCArtifactIcons();
        }

        private void updateArtifactData()
        {
            //更新玩家数据
            dataPlayer.GetPlayerData().updateArtifact(playerArtifacts);
            dataPlayer.GetPlayerData().SetArtifactNumById((int)EnumArtifact.Money, playerMoney);

            dataPlayer.GetPlayerData().setNpcArtifactsByNpcId(dataNPC.currentNPCId, npcArtifacts);
            dataPlayer.GetPlayerData().GetNpcDataById(dataNPC.currentNPCId).Money = npcMoney;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, ArtifactInfoTradeUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
            base.OnClose(isShutdown, userData);

        }

        private void OnCloseButtonClick()
        {
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        private void ShowPlayerArtifactIcons()
        {
            foreach (KeyValuePair<int, int> kvp in playerArtifacts)
            {
                int ArtifactID = kvp.Key;
                int Num = kvp.Value;

                if (ArtifactID == (int)EnumArtifact.Money || Num == 0)
                {
                    continue;
                }
                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(PlayerContainer, false);
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(ArtifactID, Num, Constant.Type.TRADE_PLAYER_NPC);
                    item.GetComponent<Button>().enabled = false;
                });
            }
        }

        private void tradeArtifact()
        {
            if (receivedType == Constant.Type.TRADE_NPC_PLAYER)
            {
                if (!testArtifactExist(playerArtifacts, receivedArtifactID, Constant.Type.ADD))
                {
                    playerArtifacts.Add(receivedArtifactID, tradeNum);
                }
                playerMoney -= totalPrice;
                npcMoney += totalPrice;
                testArtifactExist(npcArtifacts, receivedArtifactID, Constant.Type.SUB);

            }
            else
            {
                if (!testArtifactExist(npcArtifacts, receivedArtifactID, Constant.Type.ADD))
                {
                    npcArtifacts.Add(receivedArtifactID, tradeNum);
                }
                playerMoney += totalPrice;
                npcMoney -= totalPrice;
                testArtifactExist(playerArtifacts, receivedArtifactID, Constant.Type.SUB);
            }
        }

        private bool testArtifactExist(Dictionary<int, int> artifacts, int artifactID, int calculateType)
        {
            bool isExist = false;
            foreach (KeyValuePair<int, int> kvp in artifacts)
            {
                if (kvp.Key == artifactID)
                {
                    int oldNum = kvp.Value;
                    int newNum;
                    if (calculateType == Constant.Type.ADD)
                    {
                        newNum = oldNum + tradeNum;
                    }
                    else
                    {
                        newNum = oldNum - tradeNum;
                    }
                    artifacts[artifactID] = newNum;
                    isExist = true;
                    return isExist;
                }
            }
            return isExist;
        }

        private void ShowNPCArtifactIcons()
        {
            foreach (KeyValuePair<int, int> kvp in npcArtifacts)
            {
                int ArtifactID = kvp.Key;
                int Num = kvp.Value;

                if (ArtifactID == (int)EnumArtifact.Money || Num == 0)
                {
                    continue;
                }

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(NpcContainer, false);
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(ArtifactID, Num, Constant.Type.TRADE_NPC_PLAYER);
                });
            }
        }
    }
}

