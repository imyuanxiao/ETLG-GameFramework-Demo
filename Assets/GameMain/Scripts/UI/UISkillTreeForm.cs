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
    public class UISkillTreeForm : UGuiFormEx
    {

        public Button spaceshipCheckButton;



        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            spaceshipCheckButton.onClick.AddListener(OnSpaceshipButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

 
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnSpaceshipButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, SpaceshipCheckEventArgs.Create());

        }


    }
}


