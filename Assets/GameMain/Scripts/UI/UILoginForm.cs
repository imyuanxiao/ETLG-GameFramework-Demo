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
        public TextMeshProUGUI reminder = null;
        public Button switchButton;
        public Button returnButton;
        public Button submitButton;
        public GameObject confirmPassword;
        bool isRegister;
        //login
        [SerializeField]
        private TMP_InputField userName;
        [SerializeField]
        private TMP_InputField pwd;
        //register
        [SerializeField]
        private TMP_InputField confirmPwd;
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
        private void OnLogIn()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
            {
                if (PlayerPrefs.GetString(userName.text + "password") == pwd.text)
                {
                    reminder.text = "Login successful!";
                    //load player data.......
                }
                else
                {
                    reminder.text = "Incorrect password";
                }
            }
            else
            {
                reminder.text = "User does not exist.";
            }
        }
        private void OnRegister()
        {
            if(string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
            {
                if(pwd.text==confirmPwd.text)
                {
                    PlayerPrefs.SetString(userName.text, userName.text);
                    PlayerPrefs.SetString(userName.text + "password", pwd.text);
                    reminder.text = "Register successful! Please login.";
                }
                else
                {
                    reminder.text = "Passwords do not match.";
                }
            }
            else
            {
                reminder.text = "Username exists.";
            }
        }
        private void OnSwitchButtonClick()
        {
            isRegister = !isRegister;
            showContent();
        }
        private void OnReturnButtonClick()
        {

        }
        private void OnSubmitButtonClick()
        {
            if(isRegister)
            {
                OnRegister();
            }
            else
            {
                OnLogIn();
            }
        }
    }
}