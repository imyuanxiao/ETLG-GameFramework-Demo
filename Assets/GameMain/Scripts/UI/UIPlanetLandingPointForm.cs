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
    public class UIPlanetLandingPointForm : UGuiFormEx
    {

        public Button npcTalkButton;
        public Button closeButton;
       

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            npcTalkButton.onClick.AddListener(OnNPCButtonClick);
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnNPCButtonClick()
        {

            GameEntry.Event.Fire(this, NPCDialogOpenEventArgs.Create(Constant.Event.NPC_TALK));

            // 存入当前点击NPC信息（通过ID）到DataNPC数据管理脚本


        }


        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            this.Close();

        }
    }
}


