using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ETLG.Data;

namespace ETLG
{
    public class UIErrorMessageForm : UGuiFormEx
    {

        public Button returnButton;
        public TextMeshProUGUI errorTitle = null;
        public TextMeshProUGUI errorMessage = null;
        public Button YesButton;
        public Button CancelButton;
        public GameObject ButtonsContainer;

        public RawImage icon;

        private DataAlert dataAlert;
        private int alertType;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            base.OnOpen(userData);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            YesButton.onClick.AddListener(OnYesButtonClick);
            CancelButton.onClick.AddListener(OnCancelButtonClick);

            dataAlert = GameEntry.Data.GetData<DataAlert>();
            alertType = dataAlert.AlertType;

            //if is NPC error
            switch (alertType)
            {
                case Constant.Type.ALERT_TRADE_MONEYNOTENOUGH:
                    errorTitle.text = "Trade failed!";
                    errorMessage.text = "Your funds are insufficient!" + "\n" + "\n"+"Please adjust the trade quantity again.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(false);
                    break;
                case Constant.Type.ALERT_DIALOG_QUIT:
                    errorTitle.text = "Confirm Exit";
                    errorMessage.text = "Are you sure you want to quit now? " + "\n" + "\n" + "Your conversation will be saved.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(true);
                    break;
            }

            Debug.Log("errorType: " + BackendDataManager.Instance.errorType);
            //if is backend error
            if (BackendDataManager.Instance.isNewFetch)
            {
                ButtonsContainer.SetActive(false);
                switch (BackendDataManager.Instance.errorType)
                {
                    case Constant.Type.ERROR_NETWORK:
                        errorTitle.text = "Network error";
                        errorMessage.text = "Ops! Network connection failed, unable to load content. Please check your network connection and try again.";
                        icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error_network"));
                        break;
                    case Constant.Type.ERROR_SERVER:
                        errorTitle.text = "Server error";
                        errorMessage.text = "Oops! Something went wrong on our server. Please try again later.";
                        icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error_server"));
                        break;
                    case Constant.Type.ERROR_DATA:
                        errorTitle.text = "Data error";
                        errorMessage.text = "Oops! We encountered an issue while processing data. Please try again later.";
                        icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error_data"));
                        break;
                    default:
                        break;
                }
                BackendDataManager.Instance.isNewFetch = false;
            }

        }

        private void OnYesButtonClick()
        {
            //如果点击Yes，当前提示框和对话页面一起关闭
            if (alertType == Constant.Type.ALERT_DIALOG_QUIT)
            {
                GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
                OnReturnButtonClick();
            }
        }
        private void OnCancelButtonClick()
        {
            if (alertType == Constant.Type.ALERT_DIALOG_QUIT)
            {
                //Cancel仅仅起到对比Yes的作用，与直接close当前提示框作用相同
                OnReturnButtonClick();
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            dataAlert.AlertType = -1;
            base.OnClose(isShutdown, userData);
            
        }

        private void OnReturnButtonClick()
        {
            this.Close();
        }

    }
}