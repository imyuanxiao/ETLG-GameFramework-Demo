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
        public Button resetButton;
        public Button returnButton;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            spaceshipCheckButton.onClick.AddListener(OnSpaceshipButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            resetButton.onClick.AddListener(OnResetButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            // 打开SkillTreeMap
            //skillTreeMapUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UISkillTreeMap);


        }

        protected override void OnClose(bool isShutdown, object userData)
        {
    /*        // 关闭SkillTreeMap
            if (skillTreeMapUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)EnumUIForm.UISkillTreeMap);
            }

            skillTreeMapUIID = null;*/

            base.OnClose(isShutdown, userData);


        }

        private void OnSpaceshipButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, SpaceshipCheckEventArgs.Create());

        }

        private void OnReturnButtonClick()
        {
            Log.Debug("Return to Map");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }

        private void OnResetButtonClick()
        {
            Log.Debug("Reset skill data");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // reset skill data，此处等实现clone方法后再完善

        }

    }
}


