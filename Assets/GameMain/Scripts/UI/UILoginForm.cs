using ETLG.Data;
using GameFramework.Event;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace ETLG
{
    public class UILoginForm : UGuiFormEx
    {
        // Type name
        public TextMeshProUGUI titleName = null;
        public TextMeshProUGUI switchTitle = null;
        public TextMeshProUGUI submitTitle = null;
        public Button switchButton;
        public Button returnButton;
        public Button submitButton;
        public GameObject confirmPassword;
        bool isRegister;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器

            switchButton.onClick.AddListener(OnSwitchButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            submitButton.onClick.AddListener(OnSubmitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open login form");
            showContent();
            LogIn();
           
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }
        private void showContent()
        {
            if(!isRegister)
            {
                titleName.text = "Login";
                switchTitle.text = "Register";
                submitTitle.text = "Play";
                confirmPassword.SetActive(false);
            }
            else
            {
                titleName.text = "Register";
                switchTitle.text = "Login";
                submitTitle.text = "Submit";
                confirmPassword.SetActive(true);
            }
        }
        private void LogIn()
        {
           
        }
        private void Register()
        {
           
        }
        private void OnSwitchButtonClick()
        {
            isRegister = !isRegister;
            showContent();
            if(isRegister)
            {
                Register();
            }
            else
            {
                LogIn();
            }
        }
        private void OnReturnButtonClick()
        {

        }
        private void OnSubmitButtonClick()
        {
            if(isRegister)
            {

            }
            else
            {

            }
        }
    }
}