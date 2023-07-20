using ETLG.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ETLG
{
    public class UITutorialForm : UGuiFormEx
    {
        public DataTutorial dataTutorial;

        public TextMeshProUGUI Title = null;

        public Button LeftButton = null;
        public Button RightButton = null;

        public Button CloseButton = null;

        public RectTransform ImgContainer;
        public RectTransform TextContainer;
        public TextMeshProUGUI Description;

        public RawImage TutorialImg;

        public bool refresh;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataTutorial = GameEntry.Data.GetData<DataTutorial>();

            LeftButton.onClick.AddListener(OnLeftButtonClick);
            RightButton.onClick.AddListener(OnRightButtonClick);
            CloseButton.onClick.AddListener(OnCloseButtonClick);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (refresh)
            {
                showContent();
                refresh = false;
            }

            LeftButton.interactable = dataTutorial.CurrentTutorialID <= 1001 ? false : true;

            // max tutorial ID
            RightButton.interactable = dataTutorial.CurrentTutorialID >= 1004 ? false : true;
            

            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            refresh = true;

        }
        
        public void showContent()
        {
            TutorialData currentTutorialData = dataTutorial.GetCurrentTutorialData();

            if(currentTutorialData == null)
            {
                currentTutorialData = dataTutorial.GetTutorialData(1001);
            }


            Title.text = currentTutorialData.Title;

            Vector2 sizeDelta = ImgContainer.sizeDelta;
            sizeDelta.y = 840f;

            if (currentTutorialData.HasText)
            {
                TextContainer.gameObject.SetActive(true);
                Description.text = currentTutorialData.Description;
                sizeDelta.y = 540f;
            }
            else
            {
                TextContainer.gameObject.SetActive(false);
            }

            ImgContainer.sizeDelta = sizeDelta;

            string texturePath = AssetUtility.GetTutorialImg(currentTutorialData.Id.ToString());

            Texture texture = Resources.Load<Texture>(texturePath);

            if (texture == null)
            {
                texturePath = AssetUtility.GetLostTutorialImg();
                texture = Resources.Load<Texture>(texturePath);
            }

            TutorialImg.texture = texture;

        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
        }

        public void OnLeftButtonClick()
        {
            dataTutorial.CurrentTutorialID--;
            refresh = true;
        }
        public void OnRightButtonClick()
        {
            dataTutorial.CurrentTutorialID++;
            refresh = true;
        }
        public void OnCloseButtonClick()
        {
            UnityGameFramework.Runtime.UIForm[] uiForms = GameEntry.UI.GetAllLoadedUIForms();

            foreach (var uiForm in uiForms)
            {
                uiForm.OnResume();
            }

            this.Close();
        }

    }
}


