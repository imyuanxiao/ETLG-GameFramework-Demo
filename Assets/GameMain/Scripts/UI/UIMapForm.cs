using ETLG.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIMapForm : UGuiFormEx
    {
        public Button menuButton;
        public Button continueButton;
        public Button saveGameButton;
        public Button settingButton;


        public Button exitToDesktopButton;

        public Button tutorialButton;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            menuButton.onClick.AddListener(OnMenuButtonClick);
            saveGameButton.onClick.AddListener(OnSaveGameButtonClick);
            continueButton.onClick.AddListener(OnContinueButtonClick);
            settingButton.onClick.AddListener( OnSettingsButtonClick);
            exitToDesktopButton.onClick.AddListener(OnExitToDesktopButtonClick);
            tutorialButton.onClick.AddListener(OnTutorialButtonClick);

        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
  

            base.OnClose(isShutdown, userData);


        }



        private void OnSaveGameButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            GameEntry.UI.OpenUIForm(EnumUIForm.UILoadGameForm);

        }

        private void OnContinueButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            this.Close();

        }
        private void OnSettingsButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            //Close();
            GameEntry.UI.OpenUIForm(EnumUIForm.UISettingsForm);
        }

        private void OnMenuButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));

        }

        private void OnExitToDesktopButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

        private void OnTutorialButtonClick()
        {
            //GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

           // GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(Constant.Type.TUTORIAL_NEW_GAME);
        }

    }
}


