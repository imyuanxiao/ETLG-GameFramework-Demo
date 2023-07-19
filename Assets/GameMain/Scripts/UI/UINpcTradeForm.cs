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
        private bool isTrade=false;

        public delegate void TradeConditionEventHandler(int totalNum,int ownedMoney);
        public static event TradeConditionEventHandler OnTradeConditionSent;

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
        private int artifactID;
        //交易买卖方类型
        private int type;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            closeButton.onClick.AddListener(OnCloseButtonClick);

            loadArtifactsData();

            UIArtifactInfoTradeForm.OnTradeDataSent += HandleTradeData;
            refresh = true;

            npcMoney = dataPlayer.GetPlayerData().GetNpcDataById(dataNPC.currentNPCId).Money;
            playerMoney = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money);

        }

        private void loadArtifactsData()
        {
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            playerArtifacts = dataPlayer.GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_TRADE);
            npcArtifacts = dataPlayer.GetPlayerData().GetNpcArtifactsByNpcId(dataNPC.currentNPCId);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (refresh)
            {
                HideAllItem();
                showContent();
                refresh = false;
                updateArtifactData();
            }
        }

        //触发trade按钮后交易，刷新item和money
        private void HandleTradeData(int inputNum, int totalPrice)
        {
            isTrade = false;
            refresh = true;
            this.tradeNum = inputNum;
            this.totalPrice = totalPrice;
            if (inputNum < 0)
            {
                isTrade = false;
            }
            else
            {
                tradeArtifact(artifactID, type);
            }
            
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

            dataPlayer.GetPlayerData().setNpcArtifactsByNpcId(dataNPC.currentNPCId,npcArtifacts);
            dataPlayer.GetPlayerData().GetNpcDataById(dataNPC.currentNPCId).Money = npcMoney;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            UIArtifactInfoTradeForm.OnTradeDataSent -= HandleTradeData;
            base.OnClose(isShutdown, userData);
            
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, ArtifactInfoTradeUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
            this.Close();

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
                    item.GetComponent<ItemArtifactIcon>().OnItemClicked += OnItemClickedFromIcon;
                });
            }
        }

        private void tradeArtifact(int artifactID, int type)
        {
            if (type == Constant.Type.TRADE_NPC_PLAYER)
            {
                if (!testArtifactExist(playerArtifacts, artifactID, Constant.Type.ADD))
                {
                    playerArtifacts.Add(artifactID, tradeNum);
                }
                playerMoney -= totalPrice;
                npcMoney += totalPrice;
                testArtifactExist(npcArtifacts, artifactID, Constant.Type.SUB);
                
            }
            else
            {
                if (!testArtifactExist(npcArtifacts, artifactID, Constant.Type.ADD))
                {
                    npcArtifacts.Add(artifactID, tradeNum);
                }
                playerMoney += totalPrice;
                npcMoney -= totalPrice;
                testArtifactExist(playerArtifacts, artifactID, Constant.Type.SUB);
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

        private void OnItemClickedFromIcon(int artifactID, int totalNum, int type)
        {
            //if click other icon when trading, ignore the click
            if (!isTrade)
            {
                isTrade = true;

                this.totalNum = totalNum;
                this.artifactID = artifactID;
                this.type = type;

                //npc买东西需要花钱吗？
                if (type == Constant.Type.TRADE_NPC_PLAYER)
                {
                    OnTradeConditionSent?.Invoke(totalNum, playerMoney);
                }
                else
                {
                    OnTradeConditionSent?.Invoke(totalNum, npcMoney);
                }
            }
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
                    item.GetComponent<ItemArtifactIcon>().OnItemClicked += OnItemClickedFromIcon;
                });
            }
        }
    }
}


