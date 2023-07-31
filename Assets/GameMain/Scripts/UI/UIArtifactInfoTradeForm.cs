using ETLG.Data;
using GameFramework.Event;
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
    public class UIArtifactInfoTradeForm : UGuiFormEx
    {
        public DataArtifact dataArtifact;
        public DataPlayer dataPlayer;
        private DataNPC dataNPC;
        private DataTrade dataTrade;

        private ArtifactDataBase artifactDataBase;

        public Transform UIContainer;

        public TextMeshProUGUI ArtifactName = null;
        public TextMeshProUGUI ArtifactType = null;
        public TextMeshProUGUI ArtifactTradeable = null;
        public TextMeshProUGUI ArtifactValue = null;
        public TextMeshProUGUI ArtifactNumber = null;
        public TextMeshProUGUI ArtifactDescription = null;
        public TextMeshProUGUI TotalPrice = null;
        public Canvas InteractContainer;
        public Canvas ClickHint;
        public Button subNum;
        public Button plusNum;
        public TMP_InputField InputField;
        public Button tradeButton;
        public Button CloseButton;
        public VerticalLayoutGroup verticalLayoutGroup;

        private int inputNum;
        private int limitNum;
        private int maxNum;
        private int singleValue;
        private int totalPrice;
        private int ownedMoney;

        private bool needClose = false;

        //public delegate void TradeDataEventHandler(int tradeNum, int totalPrice);
        //public static event TradeDataEventHandler OnTradeDataSent;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();

            tradeButton.onClick.AddListener(tradeClick);
            CloseButton.onClick.AddListener(closeButtonClick);
            subNum.onClick.AddListener(minusNumClick);
            plusNum.onClick.AddListener(plusNumClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dataTrade = GameEntry.Data.GetData<DataTrade>();
            InputField.text = (0).ToString();

            artifactDataBase = dataArtifact.GetCurrentShowArtifactData();

            UIContainer.position = dataArtifact.artifactInfoPosition;

            ArtifactName.text = artifactDataBase.NameID;
            ArtifactType.text = GameEntry.Localization.GetString(Constant.Type.ARTIFACT_TYPE + artifactDataBase.Type);
            ArtifactTradeable.text = artifactDataBase.Tradeable ? "Tradeable" : "Untradeable";
            singleValue = artifactDataBase.Value;
            ArtifactValue.text = singleValue.ToString();

            //limitNum = artifactDataBase.MaxNumber;

            ArtifactNumber.text = dataPlayer.GetPlayerData().GetArtifactNumById(artifactDataBase.Id).ToString();
            ArtifactDescription.text = artifactDataBase.Description;

            Transform InteractContainerRectTransform = InteractContainer.GetComponent<Transform>();
            InteractContainerRectTransform.gameObject.SetActive(false);
            Transform ClickHintRectTransform = ClickHint.GetComponent<Transform>();
            ClickHintRectTransform.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            needClose = false;
            dataTrade.clearData();
            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (dataTrade.clickItemIcon)
            {
                Transform InteractContainerRectTransform = InteractContainer.GetComponent<Transform>();
                InteractContainerRectTransform.gameObject.SetActive(true);
                Transform ClickHintRectTransform = ClickHint.GetComponent<Transform>();
                ClickHintRectTransform.gameObject.SetActive(false);
            }
            else
            {
                Transform InteractContainerRectTransform = InteractContainer.GetComponent<Transform>();
                InteractContainerRectTransform.gameObject.SetActive(false);
                Transform ClickHintRectTransform = ClickHint.GetComponent<Transform>();
                ClickHintRectTransform.gameObject.SetActive(true);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);

            InputField.onValidateInput += ValidateNumericInput;
            bool success = int.TryParse(InputField.text, out inputNum);

            updateInputNumAndTotalPrice();
            
            if (dataTrade.clickTradeButton)
            {
                testTradeNum();
                if (dataTrade.save && needClose)
                {
                    closeButtonClick();
                }
            }
        }

        private void testTradeNum()
        {
            maxNum = dataTrade.artifactNum;
            //输入数量不能大于可买数量
            if (inputNum > limitNum || inputNum > maxNum)
            {
                inputNum = maxNum;
            }
            else if (inputNum < 0)
            {
                inputNum = 0;
            }
            updateInputNumAndTotalPrice();

        }

        //只对数字输入有反应，输错是否提示？
        private char ValidateNumericInput(string text, int charIndex, char addedChar)
        {
            if (char.IsDigit(addedChar))
            {
                return addedChar;
            }
            return '\0';
        }

        private void tradeClick()
        {
            
            bool enoughPlayerMoney = dataPlayer.GetPlayerData().GetArtifactNumById((int)EnumArtifact.Money) >= dataTrade.totalPrice;
            //如果买家余额充足，则购买并关闭此UI
            if (dataTrade.tradeType == Constant.Type.TRADE_PLAYER_NPC || (dataTrade.tradeType == Constant.Type.TRADE_NPC_PLAYER && enoughPlayerMoney))
            {
                //输入量和总金额
                dataTrade.setTradeData(inputNum, totalPrice);
                needClose = true;
            } 
            else
            {
                //钱不够点击trade无反应，跳出提示余额不足
                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCAlertForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCAlertForm));
                }

                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCAlertForm);
            }

        }

        private void closeButtonClick()
        {
            needClose = false;
            GameEntry.Event.Fire(this, ArtifactInfoTradeUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }

        private void minusNumClick()
        {
            //加减数量为10
            inputNum -= 10;
            testTradeNum();
        }

        private void plusNumClick()
        {
            inputNum += 10;
            testTradeNum();
        }

        private void updateInputNumAndTotalPrice()
        {
            totalPrice = singleValue * inputNum;
            //显示修改后数量
            InputField.text = inputNum.ToString();
            //显示总价
            TotalPrice.text = (singleValue * inputNum).ToString();
        }

    }
}


