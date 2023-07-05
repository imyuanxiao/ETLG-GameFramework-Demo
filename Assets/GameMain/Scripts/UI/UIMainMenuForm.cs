using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIMainMenuForm : UGuiFormEx
    {
        public Button newGameButton;
        public Button loadButton;
        public Button settingsButton;
        public Button quitButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            newGameButton.onClick.AddListener(OnNewGameButtonClick);
            loadButton.onClick.AddListener(OnLoadGameButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        protected override void OnOpen(object userData)
        {

            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnNewGameButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.NewGame")));

        }

        private void OnLoadGameButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.UI.OpenUIForm(EnumUIForm.UILoadGameForm);
            Close();

        }

        private void OnSettingsButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            Close();
            GameEntry.UI.OpenUIForm(EnumUIForm.UISettingsForm);
        }

        private void OnQuitButtonClick()
        {

            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

    }
}


