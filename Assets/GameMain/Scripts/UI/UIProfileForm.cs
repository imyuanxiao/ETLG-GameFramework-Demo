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

        public GameObject unkownPanel;

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
            normalColor = new Color32(55, 55, 55, 255);
            selectedColor = new Color32(249, 230, 196, 255);
            BackendDataManager.Instance.HandleProfile();
            wholeContainer.SetActive(false);
            origionalPwd = "******";
            
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (isRefresh)
            {
                if (fetchedType == Constant.Type.BACK_PROFILE_SUCCESS)
                {
                    wholeContainer.SetActive(true);
                    unkownPanel.SetActive(false);
                    ShowContent();
                }
                if (fetchedType == Constant.Type.BACK_PROFILE_FAILED)
                {
                    wholeContainer.SetActive(true);
                    ShowPlayerInfo();
                    achievementScore.text = "Unknown";
                    playerScore.text = "Unknown";
                    learningPath.text = "Unknown";
                    unkownPanel.SetActive(true);
                }
                else if (fetchedType == Constant.Type.BACK_PROFILE_UPDATE_SUCCESS)
                {
                    origionalName = nickName.text;
                    playerAvatarId = selectedId;
                    avatarChange.SetActive(false);
                    SetPlayerAvatar();
                    nickName.interactable = false;
                    avatarChange.SetActive(false);
                    editButtons.SetActive(true);
                    submitButtons.SetActive(false);
                    //update info
                    GameEntry.Data.GetData<DataBackend>().currentUser.avatar = playerAvatarId;
                    GameEntry.Data.GetData<DataBackend>().currentUser.nickName = nickName.text;

                }
                else if (fetchedType == Constant.Type.BACK_PROFILE_UPDATE_FAILED)
                {
                    reminder.text = GameEntry.Data.GetData<DataBackend>().message;
                    ShakeText(reminder,originalPosition);
                }
                else if (fetchedType == Constant.Type.BACK_PROFILE_PASSWORD_SUCCESS)
                {
                    pwd.interactable = false;
                    origionalPwd = newPwd.text;
                    pwd.text = origionalPwd;
                    newPasswords.SetActive(false);
                    editButtons.SetActive(true);
                    submitButtons.SetActive(false);
                }
                else if (fetchedType == Constant.Type.BACK_PROFILE_PASSWORD_FAILED)
                {
                    reminder.text = GameEntry.Data.GetData<DataBackend>().message;
                    ShakeText(reminder, originalPosition);
                }
                isRefresh = !isRefresh;
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(BackendFetchedEventArgs.EventId, OnBackendFetchedEventArgs);
        }
        private void ShowPlayerInfo()
        {
            reminder.text = null;

            
            if (int.Parse( GameEntry.Data.GetData<DataBackend>().currentUser.avatar)>=1000)
            {
                playerAvatarId = GameEntry.Data.GetData<DataBackend>().currentUser.avatar;
            }
            else
            {
                playerAvatarId = "1000";
            }
            placeholder_userName.text = GameEntry.Data.GetData<DataBackend>().currentUser.username;
            origionalName = GameEntry.Data.GetData<DataBackend>().currentUser.nickName;
            originalPosition = reminder.rectTransform.localPosition;
            nickName.text = GameEntry.Data.GetData<DataBackend>().currentUser.nickName;
            nickName.interactable = false;
            pwd.interactable = false;

            pwd.text = origionalPwd;

            editButtons.SetActive(true);
            submitButtons.SetActive(false);
            newPasswords.SetActive(false);
            avatarChange.SetActive(false);
            SetPlayerAvatar();
        }
        private void ShowProfileInfo()
        {
            achievementScore.text = GameEntry.Data.GetData<DataBackend>().userProfile.achievement;
            playerScore.text = GameEntry.Data.GetData<DataBackend>().userProfile.playerScore;
            learningPath.text = GameEntry.Data.GetData<DataBackend>().userProfile.learningProgress;
            SetBossTime(boss_1, GameEntry.Data.GetData<DataBackend>().userProfile.boss1);  // AI
            SetBossTime(boss_2, GameEntry.Data.GetData<DataBackend>().userProfile.boss2);  // Data Science
            SetBossTime(boss_3, GameEntry.Data.GetData<DataBackend>().userProfile.boss3);  // IoT
            SetBossTime(boss_4, GameEntry.Data.GetData<DataBackend>().userProfile.boss4);  // Cybersecurity
            SetBossTime(boss_5, GameEntry.Data.GetData<DataBackend>().userProfile.boss5);  // Cloud Computing
            SetBossTime(boss_6, GameEntry.Data.GetData<DataBackend>().userProfile.boss6);  // Blockchain
            SetBossTime(boss_7, GameEntry.Data.GetData<DataBackend>().userProfile.boss7);  // Final
        }
        private void ShowContent()
        {

            ShowPlayerInfo();
            ShowProfileInfo();
           
        }
        private void OnEditPlayerInfoButtonClick()
        {
            isEditInfo = true;
            nickName.interactable = true;

            nickName.text = GameEntry.Data.GetData<DataBackend>().currentUser.nickName;
            
            editButtons.SetActive(false);
            submitButtons.SetActive(true);
            avatarChange.SetActive(true);
        }
        private void OnCancelButtonClick()
        {
            reminder.text = null;
            if(isEditInfo)
            {
                nickName.interactable = false;
                Debug.Log(origionalName);
                nickName.text = origionalName;
                avatarChange.SetActive(false);
            }
            else
            {
                pwd.interactable = false;
                pwd.text = origionalPwd;
                newPasswords.SetActive(false);
            }
            
            editButtons.SetActive(true);
            submitButtons.SetActive(false);
        }
        private void OnSaveButtonClick()
        {
            
            if(isEditInfo)
            {
                //if nothing changed return
                if(selectedButton == null && nickName.text == origionalName)
                {
                    avatarChange.SetActive(false);
                    editButtons.SetActive(true);
                    submitButtons.SetActive(false);
                    return;
                }
                if(selectedButton == null)
                {
                    if(int.Parse(GameEntry.Data.GetData<DataBackend>().currentUser.avatar)>=1000)
                    selectedId = GameEntry.Data.GetData<DataBackend>().currentUser.avatar;
                    else
                    {
                        selectedId = "1000";
                    }
                }
                BackendDataManager.Instance.HandleProfileUpdate(int.Parse(selectedId), nickName.text);
            }
            else
            {
                if (!newPwd.text.Equals(confirmPwd.text))
                {
                    reminder.text = "New password and confirmed password don't match.";
                    ShakeText(reminder, originalPosition);
                    return;
                }
                if(newPwd.text.Length<4 || newPwd.text.Length>20)
                {
                    reminder.text = "Password should between 4-20 length.";
                    ShakeText(reminder, originalPosition);
                    return;
                }
                BackendDataManager.Instance.HandleProfilePassword(pwd.text, newPwd.text);
            }
        }

        private void OnEditPwdButtonClick()
        {
            isEditInfo = false;
            
            pwd.interactable = true;
            
            pwd.text = null;
            newPwd.text = null;
            confirmPwd.text = null;
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
        private void SetPlayerAvatar()
        {
            if (playerAvatarId == null)
            {
                playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(GameEntry.Data.GetData<DataBackend>().currentUser.avatar));
            }
            else
            {
                playerImage.texture = Resources.Load<Texture>(AssetUtility.GetPlayerAvatar(playerAvatarId));
                GameEntry.Data.GetData<DataBackend>().avatorId = playerAvatarId;
            }

        }
        private void SetBossTime(TextMeshProUGUI text, float bossDefeatTime)
        {
            // Dictionary<int, float> bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            //float[] bossDefeatTime = dataPlayer.GetPlayerData().bossDefeatTime;
            if (bossDefeatTime == -1)
            {
                text.text = "Unchallenged";
            }
            else
            {
                text.text = ConvertFloatToTimeString(bossDefeatTime);
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