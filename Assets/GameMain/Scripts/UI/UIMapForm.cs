using ETLG.Data;
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
        public Button playerMenuButton;
        public Button selectSpaceshipButton;
        public Button planetLandingPointButton;

        public Button planetInfoButton;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            menuButton.onClick.AddListener(OnMenuButtonClick);
            playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);
            selectSpaceshipButton.onClick.AddListener(OnSelectSpaceshipButtonClick);
            planetLandingPointButton.onClick.AddListener(OnPlanetLandingPointButtonClick);
            planetInfoButton.onClick.AddListener(OnPlanetInfoButtonClick);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnPlayerMenuButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.PlayerMenu")));

        }

        private void OnSelectSpaceshipButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.NewGame")));

        }

        private void OnMenuButtonClick()
        {
            Log.Debug("返回主菜菜单");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));

        }

        private void OnPlanetLandingPointButtonClick()
        {
            Log.Debug("调出星球登录点UI");

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());

        }

        private void OnPlanetInfoButtonClick()
        {
            Log.Debug("调出星球信息UI");

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            // 需要传入鼠标点击的星球的ID
            GameEntry.Event.Fire(this, PlanetInfoEventArgs.Create(101));



        }

    }
}


