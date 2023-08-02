using ETLG.Data;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public TextMeshProUGUI placeholderContent = null;
        //register success
        public TextMeshProUGUI placeholder_playerId = null;
        public TextMeshProUGUI placeholder_playerPasswaord = null;
        public TextMeshProUGUI placeholder_playerName = null;
        public Button switchButton;
        public Button submitButton;
        public Button closeButton;
        public Button avatar1;
        public Button avatar2;
        public Button avatar3;
        public Button avatar4;
        public Button avatar5;
        public Button avatar6;

        private Button selectedButton;
        private string playerAvatarId;
        private Color normalColor;
        private Color selectedColor;
        public GameObject confirmPassword;
        public GameObject playerAvater;
        public GameObject registerSuccess;
        public GameObject submitB;
        public RawImage playerImage;
        bool isRegister;

        private int RED = 0;
        private int GOLD = 1;
        private float shakeAmount = 5f;
        private float shakeDuration = 0.5f;
        private Vector3 originalPosition;
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
            closeButton.onClick.AddListener(OnCloseButtonClick);
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
            registerSuccess.SetActive(false);
            submitTitle.text = "Submit";
            normalColor = new Color32(55, 55, 55, 255);
            selectedColor = new Color32(249, 230, 196, 255);
            playerAvatarId = null;
            originalPosition = reminder.rectTransform.localPosition;
            showContent();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            KeyboardControl();
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }
        private void showContent()
        {
            playerAvatarId = null;
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
                placeholderContent.text = "Please Enter ID";
            }
            else
            {
                titleName.text = "Register";
                switchTitle.text = "Login";
                confirmPassword.SetActive(true);
                playerAvater.SetActive(true);
                placeholderContent.text = "Please Enter UserName";
            }
        }
        private void OnLogIn()
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
            {
                if (PlayerPrefs.GetString(userName.text + "password") == pwd.text)
                {
                    SetReminderTextandColor("Login successful!", GOLD);
                }
                else
                {
                    SetReminderTextandColor("Incorrect password", RED);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.pwd.text))
                {
                    SetReminderTextandColor("User does not exist.", RED);
                }
                else
                {
                    SetReminderTextandColor("Please enter passward.", RED);
                }
            }
        }
        private void OnRegister()
        {
            if (playerAvatarId == null)
            {
                SetReminderTextandColor("Please choose an avatar.", RED);
            }
            else
            {
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(userName.text)))
                {
                    if (pwd.text == confirmPwd.text && IsValidPassword())
                    {
                        PlayerPrefs.SetString(userName.text, userName.text);
                        PlayerPrefs.SetString(userName.text + "password", pwd.text);
                        SetReminderTextandColor("Register successful! Please login.", GOLD);
                        SetRegisterSeccessPanel();
                    }
                    else
                    {
                        if (!IsValidPassword())
                        {
                            SetReminderTextandColor("password must contain uppercase letters, lowercase letters, numbers, and special characters, and its length should be at least 8 characters.", RED);
                        }
                        else
                        {
                            SetReminderTextandColor("Passwords do not match.", RED);
                        }
                    }
                }
                else
                {
                    SetReminderTextandColor("Username exists.", RED);
                }
            }

        }
        private void OnSwitchButtonClick()
        {
            registerSuccess.SetActive(false);
            submitB.SetActive(true);
            isRegister = !isRegister;
            playerAvatarId = null;
            showContent();
        }
        private void OnSubmitButtonClick()
        {
            if (isRegister)
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
            playerAvatarId = "1000";
            SetSelectedButtonandColor(avatar1);
        }
        private void OnAvatar2ButtonClick()
        {
            playerAvatarId = "1001";
            SetSelectedButtonandColor(avatar2);
        }
        private void OnAvatar3ButtonClick()
        {
            playerAvatarId = "1003";
            SetSelectedButtonandColor(avatar3);
        }
        private void OnAvatar4ButtonClick()
        {
            playerAvatarId = "1004";
            SetSelectedButtonandColor(avatar4);
        }
        private void OnAvatar5ButtonClick()
        {
            playerAvatarId = "1005";
            SetSelectedButtonandColor(avatar5);
        }
        private void OnAvatar6ButtonClick()
        {
            playerAvatarId = "1006";
            SetSelectedButtonandColor(avatar6);
        }
        private void OnCloseButtonClick()
        {
            this.Close();
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
        private void SetRegisterSeccessPanel()
        {
            Debug.Log("playerAvatarId: " + playerAvatarId);
            registerSuccess.SetActive(true);
            submitB.SetActive(false);
            placeholder_playerName.text = userName.text;
            placeholder_playerPasswaord.text = pwd.text;
            placeholder_playerId.text = pwd.text;
            playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(playerAvatarId));
        }
        private bool IsValidPassword()
        {
            Regex regex = new Regex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$");

            return  regex.IsMatch(pwd.text);
        }
        private void SetReminderTextandColor(string text, int color)
        {
            reminder.text = text;
            if(color==RED)
            {
                reminder.color = Color.red;
                ShakeText();
            }
            if(color==GOLD)
            {
                Color gold = new Color(220f, 203f, 173f, 255f);
            }
        }
        public void ShakeText()
        {
            // 开始协程来实现文本的震动效果
            StartCoroutine(ShakeTextCoroutine());
        }

        private IEnumerator ShakeTextCoroutine()
        {
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                // 随机生成一个震动偏移
                float offsetX = Random.Range(-shakeAmount, shakeAmount);
                float offsetY = Random.Range(-shakeAmount, shakeAmount);

                // 将文本组件的位置偏移一段距离，实现震动效果
                reminder.rectTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // 恢复文本组件的原始位置
            reminder.rectTransform.localPosition = originalPosition;
        }
        private void KeyboardControl()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnSubmitButtonClick();
            }
        }
    }
}