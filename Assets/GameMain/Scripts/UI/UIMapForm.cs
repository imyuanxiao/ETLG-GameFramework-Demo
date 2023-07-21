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
       /* public Button playerMenuButton;
        public Button selectSpaceshipButton;
        public Button planetLandingPointButton;*/
        public Button saveGameButton;
        public Button settingButton;

       // public Button planetInfoButton;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            menuButton.onClick.AddListener(OnMenuButtonClick);
  /*          playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);
            selectSpaceshipButton.onClick.AddListener(OnSelectSpaceshipButtonClick);
            planetLandingPointButton.onClick.AddListener(OnPlanetLandingPointButtonClick);
            planetInfoButton.onClick.AddListener(OnPlanetInfoButtonClick);*/
            saveGameButton.onClick.AddListener(OnSaveGameButtonClick);
            continueButton.onClick.AddListener(OnContinueButtonClick);
            settingButton.onClick.AddListener( OnSettingsButtonClick);

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

    }
}


