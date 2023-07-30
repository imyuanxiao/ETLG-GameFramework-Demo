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
        public Button submitButton;
        public Button avatar1;
        public Button avatar2;
        public Button avatar3;
        public Button avatar4;
        public Button avatar5;
        public Button avatar6;

        private Button selectedButton;
        private int playerAvatarId;
        private Color normalColor;
        private Color selectedColor;
        public GameObject confirmPassword;
        public GameObject playerAvater;
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
            submitButton.onClick.AddListener(OnSubmitButtonClick);
            avatar1.onClick.AddListener(OnAvatar1ButtonClick);
            avatar2.onClick.AddListener(OnAvatar2ButtonClick);
            avatar3.onClick.AddListener(OnAvatar3ButtonClick);
            avatar4.onClick.AddListener(OnAvatar4ButtonClick);
            avatar5.onClick.AddListener(OnAvatar5ButtonClick);
            avatar6.onClick.AddListener(OnAvatar6ButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open login form");

            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            submitTitle.text = "Submit";
            normalColor = new Color32(55, 55, 55, 255);
            selectedColor = new Color32(249, 230, 196,255);
            playerAvatarId = 0;
            showContent();
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }
        private void showContent()
        {
            userName.text = null;
            pwd.text = null;
            confirmPwd.text = null;
            reminder.text = null;
            if (!isRegister)
            {
                titleName.text = "Login";
                switchTitle.text = "Register";
                confirmPassword.SetActive(false);
                playerAvater.SetActive(false);
            }
            else
            {
                titleName.text = "Register";
                switchTitle.text = "Login";
                confirmPassword.SetActive(true);
                playerAvater.SetActive(true);
            }
        }
        private void OnLogIn()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
            {
                if (PlayerPrefs.GetString(userName.text + "password") == pwd.text)
                {
                    reminder.text = "Login successful!";
                   // GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UILoginForm));
                }
                else
                {
                    reminder.text = "Incorrect password";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.pwd.text))
                {
                    reminder.text = "User does not exist.";
                }
                else
                {
                    reminder.text = "Please enter passward.";
                }
            }
        }
        private void OnRegister()
        {
            if(playerAvatarId==0)
            {
                reminder.text = "Please choose an avatar.";
            }
            else
            {
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
                {
                    if (pwd.text == confirmPwd.text)
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
           
        }
        private void OnSwitchButtonClick()
        {
            isRegister = !isRegister;
            playerAvatarId = 0;
            showContent();
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
        private void OnAvatar1ButtonClick()
        {
            playerAvatarId = 1000;
            SetSelectedButtonandColor(avatar1);
        }
        private void OnAvatar2ButtonClick()
        {
            playerAvatarId = 1001;
            SetSelectedButtonandColor(avatar2);
        }
        private void OnAvatar3ButtonClick()
        {
            playerAvatarId = 1002;
            SetSelectedButtonandColor(avatar3);
        }
        private void OnAvatar4ButtonClick()
        {
            playerAvatarId = 1003;
            SetSelectedButtonandColor(avatar4);
        }
        private void OnAvatar5ButtonClick()
        {
            playerAvatarId = 1004;
            SetSelectedButtonandColor(avatar5);
        }
        private void OnAvatar6ButtonClick()
        {
            playerAvatarId = 1005;
            SetSelectedButtonandColor(avatar6);
        }
        private void SetSelectedButtonandColor(Button button)
        {
            ColorBlock colorBlock;
            if (selectedButton != button)
            {
                if (selectedButton != null)
                {
                    colorBlock = selectedButton.colors;
                    colorBlock.normalColor = normalColor;
                    selectedButton.colors = colorBlock;
                }
            }
            selectedButton = button;
            colorBlock = selectedButton.colors;
            colorBlock.normalColor = selectedColor;
            selectedButton.colors = colorBlock;
        }
    }
}