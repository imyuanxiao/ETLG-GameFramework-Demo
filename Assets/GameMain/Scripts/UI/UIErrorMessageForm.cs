using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ETLG.Data;

namespace ETLG
{
    public class UIErrorMessageForm : UGuiFormEx
    {

        public Button returnButton;
        public TextMeshProUGUI errorTitle=null;
        public TextMeshProUGUI errorMessage=null;

        public RawImage icon;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Debug.Log("errorType: " + BackendDataManager.Instance.errorType);
            switch( BackendDataManager.Instance.errorType)
            {
                case Constant.Type.ERROR_NETWORK:
                    errorTitle.text = "Network error";
                    errorMessage.text = "Ops! Network connection failed, unable to load content. Please check your network connection and try again.";
                    break;
                case Constant.Type.ERROR_SERVER:
                    errorTitle.text = "Server error";
                    errorMessage.text = "Oops! Something went wrong on our server. Please try again later.";
                    break;
                case Constant.Type.ERROR_DATA:
                    errorTitle.text = "Data error";
                    errorMessage.text = "Oops! We encountered an issue while processing data. Please try again later.";
                    break;
            }
            

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnReturnButtonClick()
        {
            errorTitle = null;
            errorMessage = null;
            BackendDataManager.Instance.errorType=0;
            this.Close();
        }


    }
}
