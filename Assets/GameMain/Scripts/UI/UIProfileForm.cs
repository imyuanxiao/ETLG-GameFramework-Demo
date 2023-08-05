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

        public TextMeshProUGUI reminder = null;
        public TextMeshProUGUI boss_1 = null;
        public TextMeshProUGUI boss_2 = null;
        public TextMeshProUGUI boss_3 = null;
        public TextMeshProUGUI boss_4 = null;
        public TextMeshProUGUI boss_5 = null;
        public TextMeshProUGUI boss_6 = null;
        public TextMeshProUGUI boss_7 = null;
        public TextMeshProUGUI achievementScore;
        public TextMeshProUGUI playerScore;
        public TextMeshProUGUI learningPath;
        public TextMeshProUGUI placeholder_userName;
        public TextMeshProUGUI placeholder_name;
        public TextMeshProUGUI placeholder_pwd;
        public Button editPlayerInfoButton;
        public Button editPwdButton;
        public Button showPwdButton;
        public Button cancelButton;
        public Button saveButton;
        public Button avatar1;
        public Button avatar2;
        public Button avatar3;
        public Button avatar4;
        public Button avatar5;
        public Button avatar6;
        public Button returnButton;

        private Button selectedButton;
        private string playerAvatarId;
        private string selectedId;
        private Color normalColor;
        private Color selectedColor;
        private string origionalName;
        private string origionalPwd;
        public GameObject newPasswords;
        public GameObject avatarChange;
        public GameObject editButtons;
        public GameObject submitButtons;
 
        public GameObject wholeContainer;

        public RawImage playerImage;

        private float shakeAmount = 5f;
        private float shakeDuration = 0.5f;
        //reminder position
        private Vector3 originalPosition;

        private bool isRefresh;
        private bool isEditInfo;
        private int fetchedType;
        private DataPlayer dataPlayer;
        //playerInfo
        [SerializeField]
        private TMP_InputField nickName;
        [SerializeField]
        private TMP_InputField pwd;
        //register
        [SerializeField]
        private TMP_InputField newPwd;
        [SerializeField]
        private TMP_InputField confirmPwd;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // 获取玩家数据管理器

            returnButton.onClick.AddListener(OnReturnButtonClick);
            editPlayerInfoButton.onClick.AddListener(OnEditPlayerInfoButtonClick);
            editPwdButton.onClick.AddListener(OnEditPwdButtonClick);
            cancelButton.onClick.AddListener(OnCancelButtonClick);
            saveButton.onClick.AddListener(OnSaveButtonClick);
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
            GameEntry.Event.Subscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
            BackendDataManager.Instance.HandleProfile();
            wholeContainer.SetActive(false);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (isRefresh)
            {
                if (fetchedType == Constant.Type.BACK_LOGIN_SUCCESS || fetchedType == Constant.Type.BACK_LOGED_IN)
                {
                    BackendDataManager.Instance.GetUserProfileByUserId();
                }
                if (fetchedType == Constant.Type.BACK_PROFILE_SUCCESS)
                {
                    wholeContainer.SetActive(true);
                    ShowContent();
                }
                isRefresh = !isRefresh;
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
        }
        private void ShowContent()
        {
            reminder.text = null;


            if (BackendDataManager.Instance.currentUser.avatar != "1")
            {
                playerAvatarId = BackendDataManager.Instance.currentUser.avatar;
            }
            else
            {
                playerAvatarId = "1000";
            }
            placeholder_userName.text = BackendDataManager.Instance.currentUser.username;
            placeholder_name.text = BackendDataManager.Instance.currentUser.nickName;
            achievementScore.text = BackendDataManager.Instance.userProfile.achievement;
            playerScore.text = BackendDataManager.Instance.userProfile.playerScore;
            learningPath.text = BackendDataManager.Instance.userProfile.learningProgress;
            originalPosition = reminder.rectTransform.localPosition;

            nickName.interactable = false;
            pwd.interactable = false;

            editButtons.SetActive(true);
            submitButtons.SetActive(false);
            newPasswords.SetActive(false);
            avatarChange.SetActive(false);
            SetPlayerAvatar();

            SetBossTime(boss_1, 3);  // AI
            SetBossTime(boss_2, 2);  // Data Science
            SetBossTime(boss_3, 5);  // IoT
            SetBossTime(boss_4, 1);  // Cybersecurity
            SetBossTime(boss_5, 0);  // Cloud Computing
            SetBossTime(boss_6, 4);  // Blockchain
            SetBossTime(boss_7, 6);  // Final
        }
        private void OnEditPlayerInfoButtonClick()
        {
            isEditInfo = true;
            nickName.interactable = true;

            origionalName = placeholder_name.text;

            nickName.text = BackendDataManager.Instance.currentUser.nickName;

            
            placeholder_name.fontStyle = FontStyles.Bold | FontStyles.Italic;
            
            editButtons.SetActive(false);
            submitButtons.SetActive(true);
            avatarChange.SetActive(true);
        }
        private void OnCancelButtonClick()
        {
            reminder.text = null;
            nickName.interactable = false;
            pwd.interactable = false;
            if(isEditInfo)
            {
                placeholder_name.text = origionalName;
                placeholder_name.fontStyle = FontStyles.Bold;
            }
            else
            {
                placeholder_pwd.text = origionalPwd;
                placeholder_pwd.fontStyle = FontStyles.Bold;
            }
            editButtons.SetActive(true);
            submitButtons.SetActive(false);
            avatarChange.SetActive(false);
            newPasswords.SetActive(false);
        }
        private void OnSaveButtonClick()
        {
            if(!newPwd.text.Equals(confirmPwd.text))
            {
                reminder.text = "New password and confirmed password don't match.";
                ShakeText(reminder, originalPosition);
                return;
            }
            BackendDataManager.Instance.HandleProfileUpdate();
            //if player info success: 重新getProfile
            //BackendDataManager.Instance.GetUserProfileByUserId();
            if(isEditInfo)
            {
                playerAvatarId = selectedId;
                avatarChange.SetActive(false);
                SetPlayerAvatar();
                nickName.interactable = false;
                placeholder_name.text = nickName.text;
                placeholder_name.fontStyle = FontStyles.Bold;
                avatarChange.SetActive(false);
            }
            else
            {
                pwd.interactable = false;
                placeholder_pwd.text = newPwd.text;
                placeholder_pwd.fontStyle = FontStyles.Bold;
                newPasswords.SetActive(false);
            }
            editButtons.SetActive(true);
            reminder.text = null;
            
            //if fail: reminder
          
        }

        private void OnEditPwdButtonClick()
        {
            isEditInfo = false;
            pwd.interactable = true;
            origionalPwd = placeholder_pwd.text;
            placeholder_pwd.text = "Please Enter Old Password.";
            placeholder_pwd.fontStyle = FontStyles.Bold | FontStyles.Italic;
            editButtons.SetActive(false);
            submitButtons.SetActive(true);
            newPasswords.SetActive(true);
        }

        private void OnReturnButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }
        private void OnAvatar1ButtonClick()
        {
            selectedId = "1000";
            SetSelectedButtonandColor(avatar1);
        }
        private void OnAvatar2ButtonClick()
        {
            selectedId = "1001";
            SetSelectedButtonandColor(avatar2);
        }
        private void OnAvatar3ButtonClick()
        {
            selectedId = "1002";
            SetSelectedButtonandColor(avatar3);
        }
        private void OnAvatar4ButtonClick()
        {
            selectedId = "1003";
            SetSelectedButtonandColor(avatar4);
        }
        private void OnAvatar5ButtonClick()
        {
            selectedId = "1004";
            SetSelectedButtonandColor(avatar5);
        }
        private void OnAvatar6ButtonClick()
        {
            selectedId = "1005";
            SetSelectedButtonandColor(avatar6);
        }
        private void OnAvatarCancelButtonClick()
        {
            avatarChange.SetActive(false);
           
        }
        private void OnAvatarSubmitButtonClick()
        {
            
        }
        private void SetPlayerAvatar()
        {
            if (playerAvatarId == null)
            {
                playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(BackendDataManager.Instance.currentUser.avatar));
            }
            else
            {
                playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(playerAvatarId));
                BackendDataManager.Instance.avatorId = playerAvatarId;
            }

        }
        private void SetBossTime(TextMeshProUGUI text, int bossId)
        {
            // Dictionary<int, float> bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            float[] bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            if (bossDefeatTime[bossId] == -1)
            {
                text.text = "Unchallenged";
            }
            else
            {
                text.text = ConvertFloatToTimeString(bossDefeatTime[bossId]);
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