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
        public Button optionButton;
        public Button quitButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            newGameButton.onClick.AddListener(OnNewGameButtonClick);
            optionButton.onClick.AddListener(OnOptionButtonClick);
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

        private void OnOptionButtonClick()
        {
            Log.Debug("进入设置菜单");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.UI.OpenUIForm(EnumUIForm.UIOptionsForm);
        }

        private void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

    }
}


