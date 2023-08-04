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
        public Button editNameButton;
        public Button editAvatarButton;
        public Button editsPwdButton;
        public Button showPwdButton;
        public Button avatarCancelButton;
        public Button avatarSubmitButton;
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
        public GameObject confirmPassword;
        public GameObject avatarChange;
        public GameObject editNameButtons;
        public GameObject editPwdButtons;
        public GameObject playerInfo;

        public RawImage playerImage;

        private float shakeAmount = 5f;
        private float shakeDuration = 0.5f;
        private Vector3 originalPosition1;
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
            // 获取玩家数据管理器

            editNameButton.onClick.AddListener(OnEditNameButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);
            editAvatarButton.onClick.AddListener(OneEditAvatarButtonClick);
            avatarCancelButton.onClick.AddListener(OnAvatarCancelButtonClick);
            avatarSubmitButton.onClick.AddListener(OnAvatarSubmitButtonClick);
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

            GameEntry.UI.OpenUIForm(EnumUIForm.UINavigationForm);
            PwdReminder.text = null;
            NameReminder.text = null;
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

            SetBossTime(boss_1, 1100);
            SetBossTime(boss_2, 1101);
            SetBossTime(boss_3, 1102);
            SetBossTime(boss_4, 1103);
            SetBossTime(boss_5, 1104);
            SetBossTime(boss_6, 1105);
            SetBossTime(boss_7, 1106);
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

        }
        private void OneEditAvatarButtonClick()
        {
            playerInfo.SetActive(false);
            avatarChange.SetActive(true);
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
            selectedId = 1003;
            SetSelectedButtonandColor(avatar3);
        }
        private void OnAvatar4ButtonClick()
        {
            selectedId = 1004;
            SetSelectedButtonandColor(avatar4);
        }
        private void OnAvatar5ButtonClick()
        {
            selectedId = 1005;
            SetSelectedButtonandColor(avatar5);
        }
        private void OnAvatar6ButtonClick()
        {
            selectedId = 1006;
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
            Dictionary<int, float> bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            if(bossDefeatTime[bossId] == -1)
            {
                text.text = "Unchallenged";
            }
            else
            {
                text.text = ConvertFloatToTimeString( bossDefeatTime[bossId]);
            }
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