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
        //public Button missionButton;
        public Button achievementButton;
        //public Button knowledgeBaseButton;
        public Button leaderboardButton;


        private RawImage[] buttonImages;

        private Color selectedColor = new Color(1f, 1f, 1f, 1f);
        private Color deselectedColor = new Color(1f, 1f, 1f, 0.3f);


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            buttonImages = new RawImage[]
            {
                spaceshipButton.GetComponent<RawImage>(),
                skillButton.GetComponent<RawImage>(),
                //missionButton.GetComponent<RawImage>(),
                achievementButton.GetComponent<RawImage>(),
                //knowledgeBaseButton.GetComponent<RawImage>(),
                leaderboardButton.GetComponent<RawImage>()
            };
            spaceshipButton.onClick.AddListener(() => OnButtonClick(spaceshipButton, Constant.Type.PLAYERMENU_SPACESHIP));
            skillButton.onClick.AddListener(() => OnButtonClick(skillButton, Constant.Type.PLAYERMENU_SKILL));
           // missionButton.onClick.AddListener(() => OnButtonClick(missionButton, Constant.Type.PLAYERMENU_MISSION));
            achievementButton.onClick.AddListener(() => OnButtonClick(achievementButton, Constant.Type.PLAYERMENU_ACHIEVEMENT));
            //knowledgeBaseButton.onClick.AddListener(() => OnButtonClick(knowledgeBaseButton, Constant.Type.PLAYERMENU_KNOWLEDGE_BASE));
            leaderboardButton.onClick.AddListener(() => OnButtonClick(leaderboardButton, Constant.Type.PLAYERMENU_LEADERBOARD));
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

            if (selectedButton == (int)(int)EnumUIForm.UIAchievementForm)
            {
                index = 2;
            }

            if (selectedButton == (int)(int)EnumUIForm.UILeaderBoardForm)
            {
                index = 3;
            }

            foreach (var buttonImage in buttonImages)
            {
                buttonImage.color = (buttonImage == buttonImages[index].GetComponent<RawImage>()) ? selectedColor : deselectedColor;
            }
        }


    }
}
