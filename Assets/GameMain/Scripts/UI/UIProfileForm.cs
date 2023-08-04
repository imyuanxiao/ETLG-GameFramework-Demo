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
    public class UIProfileForm : UGuiFormEx
    {

        public TextMeshProUGUI PwdReminder = null;
        public TextMeshProUGUI NameReminder = null;
        public TextMeshProUGUI boss_1 = null;
        public TextMeshProUGUI boss_2 = null;
        public TextMeshProUGUI boss_3 = null;
        public TextMeshProUGUI boss_4 = null;
        public TextMeshProUGUI boss_5 = null;
        public TextMeshProUGUI boss_6 = null;
        public TextMeshProUGUI boss_7 = null;
        public TextMeshProUGUI placeholder_name;
        public Button editNameButton;
        public Button editAvatarButton;
        public Button editPwdButton;
        public Button showPwdButton;
        public Button avatarCancelButton;
        public Button avatarSubmitButton;
        public Button nameCancelButton;
        public Button nameSubmitButton;
        public Button pwdCancelButton;
        public Button pwdSubmitButton;
        public Button avatar1;
        public Button avatar2;
        public Button avatar3;
        public Button avatar4;
        public Button avatar5;
        public Button avatar6;
        public Button returnButton;

        private Button selectedButton;
        private int playerAvatarId;
        private int selectedId;
        private Color normalColor;
        private Color selectedColor;
        private string origionalName;
        public GameObject confirmPassword;
        public GameObject avatarChange;
        public GameObject editNameButtons;
        public GameObject editPwdButtons;
        public GameObject editNameButtonObj;
        public GameObject editPwdButtonObj;
        public GameObject playerInfo;

        public RawImage playerImage;

        private float shakeAmount = 5f;
        private float shakeDuration = 0.5f;
        //nameReminder position
        private Vector3 originalPosition1;
        //pwd reminder position
        private Vector3 originalPosition2;

        private DataPlayer dataPlayer;
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
            // ��ȡ������ݹ�����

            editNameButton.onClick.AddListener(OnEditNameButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            editAvatarButton.onClick.AddListener(OneEditAvatarButtonClick);
            avatarCancelButton.onClick.AddListener(OnAvatarCancelButtonClick);
            avatarSubmitButton.onClick.AddListener(OnAvatarSubmitButtonClick);
            nameCancelButton.onClick.AddListener(OnNameCancelButtonClick);
            nameSubmitButton.onClick.AddListener(OnNameSubmitButtonClick);
            editPwdButton.onClick.AddListener(OnEditPwdButtonClick);
            pwdCancelButton.onClick.AddListener(OnPwdCancelButtonClick);
            pwdSubmitButton.onClick.AddListener(OnPwdSubmitButtonClick);
            avatar1.onClick.AddListener(OnAvatar1ButtonClick);
            avatar2.onClick.AddListener(OnAvatar2ButtonClick);
            avatar3.onClick.AddListener(OnAvatar3ButtonClick);
            avatar4.onClick.AddListener(OnAvatar4ButtonClick);
            avatar5.onClick.AddListener(OnAvatar5ButtonClick);
            avatar6.onClick.AddListener(OnAvatar6ButtonClick);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("Open profile form");
            //GameEntry.UI.OpenUIForm(EnumUIForm.UILoginForm);
            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            PwdReminder.text = null;
            NameReminder.text = null;

            //placeholder_name.text = 

            if(BackendDataManager.Instance.avatorId!=0)
            {
                playerAvatarId = BackendDataManager.Instance.avatorId;
            }
            else
            {
                playerAvatarId = 1000;
            }

            originalPosition1 = NameReminder.rectTransform.localPosition;
            originalPosition2 = PwdReminder.rectTransform.localPosition;

            userName.interactable = false;
            pwd.interactable = false;

            editNameButtons.SetActive(false);
            editPwdButtons.SetActive(false);
            confirmPassword.SetActive(false);
            avatarChange.SetActive(false);
            playerInfo.SetActive(true);
            SetPlayerAvatar();

            SetBossTime(boss_1, 3);  // AI
            SetBossTime(boss_2, 2);  // Data Science
            SetBossTime(boss_3, 5);  // IoT
            SetBossTime(boss_4, 1);  // Cybersecurity
            SetBossTime(boss_5, 0);  // Cloud Computing
            SetBossTime(boss_6, 4);  // Blockchain
            SetBossTime(boss_7, 6);  // Final
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void OnEditNameButtonClick()
        {
            userName.interactable = true;
            origionalName = placeholder_name.text;
            SetTextMessage(placeholder_name, "Please Enter User Name");
            placeholder_name.fontStyle = FontStyles.Bold | FontStyles.Italic;
            editNameButtonObj.SetActive(false);
            editNameButtons.SetActive(true);
        }
        private void OnNameCancelButtonClick()
        {
            NameReminder.text = null;
            userName.interactable = false;
            placeholder_name.text = origionalName;
            placeholder_name.fontStyle = FontStyles.Bold;
            editNameButtonObj.SetActive(true);
            editNameButtons.SetActive(false);
        }
        private void OnNameSubmitButtonClick()
        {
            NameReminder.text = null;
            userName.interactable = false;
            placeholder_name.fontStyle = FontStyles.Bold;
            editNameButtonObj.SetActive(true);
            editNameButtons.SetActive(false);
            //if it is unique
            placeholder_name.text = userName.text;
            //else
            //SetTextMessage(NameReminder, "Name exists");
           // ShakeText(NameReminder, originalPosition1);
        }
        private void OneEditAvatarButtonClick()
        {
            playerInfo.SetActive(false);
            avatarChange.SetActive(true);
        }
        private void OnEditPwdButtonClick()
        {
            pwd.interactable = true;
            editPwdButtonObj.SetActive(false);
            editPwdButtons.SetActive(true);
            confirmPassword.SetActive(true);
        }
        private void OnPwdCancelButtonClick()
        {
            PwdReminder.text = null;
            pwd.interactable = false;
            editPwdButtonObj.SetActive(true);
            editPwdButtons.SetActive(false);
            confirmPassword.SetActive(false);
        }
        private void OnPwdSubmitButtonClick()
        {
            PwdReminder.text = null;
            //&& != last passward && is valid
            if(pwd.text == confirmPwd.text)
            {
                pwd.interactable = false;
                editPwdButtonObj.SetActive(true);
                editPwdButtons.SetActive(false);
                confirmPassword.SetActive(false);
            }
            else
            {
                SetTextMessage(NameReminder, "Name exists");
                ShakeText(PwdReminder, originalPosition2);
            }
            
        }
        private void OnReturnButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }
        private void OnAvatar1ButtonClick()
        {
            selectedId = 1000;
            SetSelectedButtonandColor(avatar1);
        }
        private void OnAvatar2ButtonClick()
        {
            selectedId = 1001;
            SetSelectedButtonandColor(avatar2);
        }
        private void OnAvatar3ButtonClick()
        {
            selectedId = 1002;
            SetSelectedButtonandColor(avatar3);
        }
        private void OnAvatar4ButtonClick()
        {
            selectedId = 1003;
            SetSelectedButtonandColor(avatar4);
        }
        private void OnAvatar5ButtonClick()
        {
            selectedId = 1004;
            SetSelectedButtonandColor(avatar5);
        }
        private void OnAvatar6ButtonClick()
        {
            selectedId = 1005;
            SetSelectedButtonandColor(avatar6);
        }
        private void OnAvatarCancelButtonClick()
        {
            avatarChange.SetActive(false);
        }
        private void OnAvatarSubmitButtonClick()
        {
            playerAvatarId = selectedId;
            playerInfo.SetActive(true);
            avatarChange.SetActive(false);
            SetPlayerAvatar();
        }
        private void SetPlayerAvatar()
        {
            playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(playerAvatarId.ToString()));
            BackendDataManager.Instance.avatorId = playerAvatarId;
        }
        private void SetBossTime(TextMeshProUGUI text , int bossId)
        {
            // Dictionary<int, float> bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            float[] bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            if(bossDefeatTime[bossId] == -1)
            {
                text.text = "Unchallenged";
            }
            else
            {
                text.text = ConvertFloatToTimeString( bossDefeatTime[bossId]);
            }
        }
        private void SetTextMessage(TextMeshProUGUI text, string message)
        {
            text.text = message;
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
        private string ConvertFloatToTimeString(float seconds)
        {
            int totalSeconds = Mathf.FloorToInt(seconds);
            int hours = totalSeconds / 3600;
            int remainingSeconds = totalSeconds % 3600;
            int minutes = remainingSeconds / 60;
            int remainingMinutes = remainingSeconds % 60;

            int milliseconds = Mathf.FloorToInt((seconds - totalSeconds) * 1000);

            string hoursString = hours.ToString().PadLeft(2, '0');
            string minutesString = minutes.ToString().PadLeft(2, '0');
            string secondsString = remainingMinutes.ToString().PadLeft(2, '0');
            string millisecondsString = milliseconds.ToString().PadLeft(3, '0');

            return $"{hoursString}:{minutesString}:{secondsString}.{millisecondsString}";

        }
        private void ShakeText(TextMeshProUGUI reminder, Vector3 originalPosition)
        {
            StartCoroutine(ShakeTextCoroutine(reminder, originalPosition));
        }

        private IEnumerator ShakeTextCoroutine(TextMeshProUGUI reminder, Vector3 originalPosition)
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
    }
}