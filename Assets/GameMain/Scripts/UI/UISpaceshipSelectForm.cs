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
    public class UISpaceshipSelectForm : UGuiFormEx
    {
        public Button startButton;
        public Button leftButton;
        public Button rightButton;
        public Button returnButton;

        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_description = null;


        // 需要通过数据管理器获取数据
        private DataSpaceship dataSpaceship = null;

        // 当前展示的飞船信息
        private SpaceshipData currentSpaceshipData = null;

        // 当前展示飞船的ID
        private int currentIndex = (int)EnumEntity.InterstellarExplorer;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            // 绑定按钮点击事件
            startButton.onClick.AddListener(OnStartButtonClick);
            leftButton.onClick.AddListener(OnLeftButtonClick);
            rightButton.onClick.AddListener(OnRightButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);

            // 获取数据管理器
            dataSpaceship = GameEntry.Data.GetData<DataSpaceship>();

            ShowSpaceshipSelect();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnStartButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }

        private void OnLeftButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            currentIndex--;
            if(currentIndex < (int)EnumEntity.InterstellarExplorer)
            {
                currentIndex = (int)EnumEntity.Guardian;
            }
            ShowSpaceshipSelect();
        }
        private void OnRightButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            currentIndex++;
            if (currentIndex > (int)EnumEntity.Guardian)
            {
                currentIndex = (int)EnumEntity.InterstellarExplorer;
            }
            ShowSpaceshipSelect();
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("返回主菜菜单");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));

        }


        public void ShowSpaceshipSelect()
        {

            // 通过数据管理器的方法初始化当前飞船信息
            currentSpaceshipData = dataSpaceship.GetSpaceshipData(currentIndex);

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by id '{0}'.", currentIndex);
                return;
            }

            // 修改UI显示值
            s_name.text = currentSpaceshipData.NameId;
            s_description.text = currentSpaceshipData.Description;


        }



    }
}


