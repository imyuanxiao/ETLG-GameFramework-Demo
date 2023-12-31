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
            returnButton.onClick.AddListener(OnReturnButtonClick);
            YesButton.onClick.AddListener(OnYesButtonClick);
            CancelButton.onClick.AddListener(OnCancelButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            base.OnOpen(userData);
            GameEntry.Sound.PlaySound(EnumSound.ui_Alert);
            dataAlert = GameEntry.Data.GetData<DataAlert>();
            alertType = dataAlert.AlertType;
            
            switch (alertType)
            {
                //if is NPC error
                case Constant.Type.ALERT_TRADE_MONEYNOTENOUGH:
                    errorTitle.text = "Trade failed!";
                    errorMessage.text = "Your funds are insufficient!" + "\n" + "\n" + "Please adjust the trade quantity again.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(false);
                    break;
                case Constant.Type.ALERT_DIALOG_QUIT:
                    errorTitle.text = "Confirm Exit";
                    errorMessage.text = "Are you sure you want to quit now? " + "\n" + "\n" + "You will NOT get any award! " + "\n" + "\n" + "Your conversation will be saved.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(true);
                    break;
                case Constant.Type.ALERT_DIALOG_QUIT_GOTTENAWARD:
                    errorTitle.text = "Confirm Exit";
                    errorMessage.text = "Are you sure you want to quit now? " + "\n" + "\n" + "Your conversation will be saved.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(true);
                    break;
                case Constant.Type.ALERT_QUIZ_QUIT:
                    errorTitle.text = "Confirm Exit";
                    errorMessage.text = "Are you sure you want to quit now? " + "\n" + "\n" + "You will NOT get any award! " + "\n" + "\n" + "Your quiz progress will be saved.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(true);
                    break;
                case Constant.Type.ALERT_QUIZ_QUIT_GOTTENAWARD:
                    errorTitle.text = "Confirm Exit";
                    errorMessage.text = "Are you sure you want to quit now? " + "\n" + "\n" + "Your quiz progress will be saved.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error"));
                    ButtonsContainer.SetActive(true);
                    break;
                    //if it is backend error
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
                    errorTitle.text = "Data Not Found";
                    errorMessage.text = "You have not uploaded your save data yet. Please upload it manually and try again.";
                    icon.texture = Resources.Load<Texture>(AssetUtility.GetErrorIcon("error_data"));
                    break;
                default:
                    break;
            }

           if(dataAlert.isFromBackend)
            {
                ButtonsContainer.SetActive(false);
            }

        }

        private void OnYesButtonClick()
        {
            //Prompt and Dialogue UI close together if click EXIT
            if (alertType == Constant.Type.ALERT_DIALOG_QUIT|| alertType == Constant.Type.ALERT_DIALOG_QUIT_GOTTENAWARD)
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogForm)); 
            }else if (alertType == Constant.Type.ALERT_QUIZ_QUIT|| alertType == Constant.Type.ALERT_QUIZ_QUIT_GOTTENAWARD)
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizForm));
            }
            OnReturnButtonClick();
        }
        private void OnCancelButtonClick()
        {
            if (alertType == Constant.Type.ALERT_DIALOG_QUIT || alertType == Constant.Type.ALERT_QUIZ_QUIT || alertType == Constant.Type.ALERT_QUIZ_QUIT_GOTTENAWARD || alertType == Constant.Type.ALERT_DIALOG_QUIT_GOTTENAWARD)
            {
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
            dataAlert.isFromBackend = false;
            base.OnClose(isShutdown, userData);

        }

        private void OnReturnButtonClick()
        {
            GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIErrorMessageForm));
        }

    }
}