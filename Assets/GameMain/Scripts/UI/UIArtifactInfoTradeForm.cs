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

        public delegate void TradeDataEventHandler(int tradeNum, int totalPrice);
        public static event TradeDataEventHandler OnTradeDataSent;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            tradeButton.onClick.AddListener(tradeClick);
            CloseButton.onClick.AddListener(closeButtonClick);
            subNum.onClick.AddListener(minusNumClick);
            plusNum.onClick.AddListener(plusNumClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            artifactDataBase = dataArtifact.GetCurrentShowArtifactData();

            UIContainer.position = dataArtifact.artifactInfoPosition;

            ArtifactName.text = artifactDataBase.NameID;
            ArtifactType.text = GameEntry.Localization.GetString(Constant.Type.ARTIFACT_TYPE + artifactDataBase.Type);
            ArtifactTradeable.text = artifactDataBase.Tradeable ? "Tradeable" : "Untradeable";
            singleValue = artifactDataBase.Value;
            ArtifactValue.text = singleValue.ToString();

            limitNum = artifactDataBase.MaxNumber;

            ArtifactNumber.text = dataPlayer.GetPlayerData().GetArtifactNumById(artifactDataBase.Id).ToString();
            ArtifactDescription.text = artifactDataBase.Description;

            Transform InteractContainerRectTransform = InteractContainer.GetComponent<Transform>();
            InteractContainerRectTransform.gameObject.SetActive(false);
            Transform ClickHintRectTransform = ClickHint.GetComponent<Transform>();
            ClickHintRectTransform.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)verticalLayoutGroup.transform);

            UINpcTradeForm.OnTradeConditionSent += HandleTradeCondition;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            artifactDataBase.isTrade = false;
            UINpcTradeForm.isTrade = false;
            artifactDataBase = null;
            UINpcTradeForm.OnTradeConditionSent -= HandleTradeCondition;
            base.OnClose(isShutdown, userData);

        }

        //卖家拥有的item总数量
        private void HandleTradeCondition(int totalNum, int ownedMoney)
        {
            this.maxNum = totalNum;
            this.ownedMoney = ownedMoney;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (artifactDataBase.isTrade)
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

            testTradeNum();
            updateInputNumAndTotalPrice();
        }

        private void testTradeNum()
        {
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
            //如果买家余额充足，则购买并关闭此UI
            if (ownedMoney >= totalPrice)
            {
                //输入量和总金额
                OnTradeDataSent?.Invoke(inputNum, totalPrice);

                closeButtonClick();
            }
            else
            {
                //如果不充足点击trade无反应，是否跳出提示余额不足？
            }

        }

        private void closeButtonClick()
        {
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


