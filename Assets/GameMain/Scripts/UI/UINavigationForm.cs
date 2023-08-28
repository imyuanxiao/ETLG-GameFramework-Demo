using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ETLG.Data;

namespace ETLG
{
    public class UINavigationForm : UGuiFormEx
    {
        public Button spaceshipButton;
        public Button skillButton;
        public Button achievementButton;
        public Button leaderboardButton;
        public Button profileButton;

        private RawImage[] buttonImages;

        private Color selectedColor = new Color(1f, 1f, 1f, 1f);
        private Color deselectedColor = new Color(1f, 1f, 1f, 0.7f);


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            buttonImages = new RawImage[]
            {
                spaceshipButton.GetComponent<RawImage>(),
                skillButton.GetComponent<RawImage>(),
                achievementButton.GetComponent<RawImage>(),
                leaderboardButton.GetComponent<RawImage>(),
                profileButton.GetComponent<RawImage>()
            };
            spaceshipButton.onClick.AddListener(() => OnButtonClick(spaceshipButton, (int)EnumUIForm.UISpaceshipCheckForm));
            skillButton.onClick.AddListener(() => OnButtonClick(skillButton, (int)EnumUIForm.UISkillTreeForm));
            achievementButton.onClick.AddListener(() => OnButtonClick(achievementButton, (int)EnumUIForm.UIAchievementForm));
            leaderboardButton.onClick.AddListener(() => OnButtonClick(leaderboardButton, (int)EnumUIForm.UILeaderboardForm));
            profileButton.onClick.AddListener(() => OnButtonClick(leaderboardButton, (int)EnumUIForm.UIProfileForm));
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetButtonColors(GameEntry.Data.GetData<DataPlayer>().currentSelectedPlayerMenu);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnButtonClick(Button clickedButton, int playerMenuConstant)
        {
            GameEntry.Event.Fire(this, ChangePlayerMenuEventArgs.Create(playerMenuConstant));
        }


        private void SetButtonColors(int selectedButton)
        {
            //Log.Debug("selectedButton {0}",selectedButton);

            int index = 0;
            if (selectedButton == (int)EnumUIForm.UISkillTreeForm)
            {
                index = 1;
            }

            if (selectedButton == (int)EnumUIForm.UIAchievementForm)
            {
                index = 2;
            }

            if (selectedButton == (int)EnumUIForm.UILeaderboardForm)
            {
                index = 3;
            }

            if(selectedButton == (int)EnumUIForm.UIProfileForm)
            {
                index = 4;
            }


            foreach (var buttonImage in buttonImages)
            {
                buttonImage.color = (buttonImage == buttonImages[index].GetComponent<RawImage>()) ? selectedColor : deselectedColor;
            }
        }


    }
}
