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
        //register success
        public TextMeshProUGUI p_playerUserName = null;
        public TextMeshProUGUI p_playerPasswaord = null;
        public Button switchButton;
        public Button submitButton;
        public Button closeButton;

        private bool isRefresh;

        public GameObject confirmPassword;
        public GameObject registerSuccess;
        public GameObject submitB;
        public GameObject middleContainer;
        public RawImage playerImage;
        bool isRegister;

        private int RED = 0;
        private int GOLD = 1;
        private int fetchedType;
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
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open login form");

            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);

            GameEntry.Event.Subscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
            registerSuccess.SetActive(false);
            middleContainer.SetActive(true);
            submitTitle.text = "Submit";
            originalPosition = reminder.rectTransform.localPosition;
            showContent();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            KeyboardControl();
            //if success
            if (isRefresh)
            {
                if(!isRegister)
                {
                    OnLogIn();
                }
                else
                {
                    OnRegister();
                }
                isRefresh = !isRefresh;
                fetchedType = 0;
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);

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
            }
            else
            {
                titleName.text = "Register";
                switchTitle.text = "Login";
                confirmPassword.SetActive(true);
            }
        }
        private void OnLogIn()
        {
            
                if (fetchedType == Constant.Type.BACK_LOGIN_SUCCESS)
                {
                    
                    SetReminderTextandColor("Login successful!", GOLD);
                //登录成功之后....关闭？
                this.Close();
                }
                else if(fetchedType == Constant.Type.BACK_LOGIN_FAILED)
                {
                if (string.IsNullOrEmpty(this.pwd.text))
                {
                    //
                    SetReminderTextandColor("Please enter passward.", RED);
                }
                else
                {
                    SetReminderTextandColor(GameEntry.Data.GetData<DataBackend>().message, RED);
                }
                userName.text = null;
                pwd.text = null;
                }
                
        }
        private void OnRegister()
        {
                //if success
                if(fetchedType == Constant.Type.BACK_REGISTER_SUCCESS)
                {
                    SetReminderTextandColor("Register successful! Please login.", GOLD);

                    SetRegisterSeccessPanel();
                }
            //if failed
                else if (fetchedType == Constant.Type.BACK_REGISTER_FAILED)
                {
                    SetReminderTextandColor(GameEntry.Data.GetData<DataBackend>().message, RED);
                }

        }
        private void OnSwitchButtonClick()
        {
            middleContainer.SetActive(true);
            registerSuccess.SetActive(false);
            submitB.SetActive(true);
            isRegister = !isRegister;
            showContent();
        }
        private void OnSubmitButtonClick()
        {
            if (isRegister)
            {
                if(string.IsNullOrEmpty( userName.text))
                {
                    SetReminderTextandColor("Please enter user name.", RED);
                    return;
                }
                //只能不能包含特殊字符和字数限制？？
                /*
                if (userName.text.Length>16))
                {
                    SetReminderTextandColor("The username can not longer than 16 characters.", RED);
                    return;
                }
                if(!IsStringValid(userName.text))
                {
                    SetReminderTextandColor("The username can only contain letters and numbers.", RED);
                    return;
                }
                
                */
                if (string.IsNullOrEmpty(pwd.text))
                {
                    SetReminderTextandColor("Please enter password.", RED);
                    return;
                }
                if(pwd.text != confirmPwd.text )
                {
                    SetReminderTextandColor("Passwords do not match.", RED);
                    ShakeText();
                    return;
                }
                BackendDataManager.Instance.HandleRegister(userName.text, pwd.text);
            }
            else
            {
                BackendDataManager.Instance.HandleLogIn(userName.text, pwd.text);
            }
        }
      
        private void OnCloseButtonClick()
        {
            this.Close();
        }
        private void SetRegisterSeccessPanel()
        {
            middleContainer.SetActive(false);
            registerSuccess.SetActive(true);
            submitB.SetActive(false);
            p_playerPasswaord.text = pwd.text;
            p_playerUserName.text = userName.text;
            playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar("1000"));
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
                reminder.color = gold;
            }
        }
        public void ShakeText()
        {
            StartCoroutine(ShakeTextCoroutine());
        }

        private IEnumerator ShakeTextCoroutine()
        {
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                reminder.rectTransform.localPosition = originalPosition + new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            reminder.rectTransform.localPosition = originalPosition;
        }
        private static bool IsStringValid(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        }
        private void KeyboardControl()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnSubmitButtonClick();
            }
        }
        public void OnBackendFetchedEventArgs(object sender, GameEventArgs e)
        {
            BackendFetchedEventArgs ne = (BackendFetchedEventArgs)e;
            if (ne == null)
                return;
            isRefresh = true;
            fetchedType = ne.Type;

        }
    }
}