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

            //if is backend error
            if (BackendDataManager.Instance.isNewFetch)
            {
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
            this.Close();
        }


    }
}