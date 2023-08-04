using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Xml;

namespace ETLG
{
    public class UINpcQuizForm : UGuiFormEx
    {
        private NPCData npcData;
        private DataAlert dataAlert;
        private DataLearningProgress dataLearningProgress;
        private DataQuiz dataQuizReport;
        private DataPlayer dataPlayer;
        private string npcAvatarPath;
        private string XMLPath;
        private string rate;
        private UIQuizManager UIQuizManager = null;
        public VerticalLayoutGroup ChoicesContainerverticalLayoutGroup;
        public VerticalLayoutGroup ContentverticalLayoutGroup;

        public TextMeshProUGUI npc_name;
        public TextMeshProUGUI npc_description;
        public RawImage npc_avatar;
        public TextMeshProUGUI statement;
        public Transform ChoicesContainer;
        public Canvas ChoicePrefab;
        public Button LastButton;
        public Button NextButton;
        public Button SubmitButton;
        public Button closeButton;
        public Slider ProgressSlider;
        public Slider AccuracySlider;
        public TextMeshProUGUI LeftQuestionsText;
        public TextMeshProUGUI AccuracyRate;
        public TextMeshProUGUI Analysis;
        public Canvas AnalysisContainer;
        public VerticalLayoutGroup ContentVerticalLayoutGroup;

        private int currentQuizIndex = 0;
        private UIQuiz currentQuiz;

        private bool isToggling = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
            LastButton.onClick.AddListener(OnLastButtonClick);
            NextButton.onClick.AddListener(OnNextButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Sound.StopMusic();

            UIQuizManager = null;
            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAlert = GameEntry.Data.GetData<DataAlert>();
            dataQuizReport = GameEntry.Data.GetData<DataQuiz>();
            dataQuizReport.reset();
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();

            npc_name.text = npcData.Name;
            npcAvatarPath = AssetUtility.GetNPCAvatar(npcData.Id.ToString());
            npc_description.text = npcData.Domain + "\n" + npcData.Course + "\n" + npcData.Chapter;

            SubmitButton.onClick.AddListener(OnSubmitButtonClick);

            loadAvatar();
            UIQuizManager tempUIQuizManager = dataPlayer.GetPlayerData().getUIQuizManager(npcData.Id);
            if (tempUIQuizManager == null)
            {
                XMLPath = AssetUtility.GetQuizXML(npcData.Id.ToString());
                UIQuizManager = new UIQuizManager(XMLPath);
                dataPlayer.GetPlayerData().setUIQuizManagerById(npcData.Id, UIQuizManager);
            }
            else
            {
                UIQuizManager = tempUIQuizManager;
            }
            dataQuizReport.boss = UIQuizManager.boss;
            dataQuizReport.award = UIQuizManager.award;

            loadQuestions();
            updateProgress();
            updateAccuracy();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            updateSubmitButtonStatus();
            updateLastButtonStatus();
            updateNextButtonStatus();
            if (UIQuizManager.TotalSubmitQuestions == UIQuizManager.totalQuestion)
            {
                SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "REPORT";
                SubmitButton.enabled = true;
            }
            else
            {
                SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "SUBMIT";
            }
            if (dataQuizReport.again)
            {
                currentQuizIndex = 0;
                UIQuizManager.reset();
                dataQuizReport.reset();
                destroyAllOptions();
                loadQuestions();
                updateProgress();
                updateAccuracy();
                dataQuizReport.again = false;
            }
            if (dataQuizReport.clickGetButton)
            {
                getAward();
                dataQuizReport.clickGetButton = false;
            }
            dataPlayer.GetPlayerData().setUIQuizManagerById(npcData.Id, UIQuizManager);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ContentverticalLayoutGroup.transform);
        }

        private void updateProgress()
        {
            string progress = UIQuizManager.TotalSubmitQuestions.ToString() + " / " + UIQuizManager.totalQuestion.ToString();
            LeftQuestionsText.text = progress;
            ProgressSlider.value = UIQuizManager.progressFloat();
        }

        private void updateAccuracy()
        {
            rate = (UIQuizManager.calculateAccuracy() * 100).ToString("F0");
            AccuracyRate.text = rate + "%";
            AccuracySlider.value = UIQuizManager.calculateAccuracy();

        }

        private void updateSubmitButtonStatus()
        {
            if (!currentQuiz.testAllSwitchOff())
            {
                if (!currentQuiz.haveSubmitted)
                {
                    SubmitButton.enabled = true;
                }
                currentQuiz.haveShown = true;
            }
            else if (UIQuizManager.TotalSubmitQuestions == UIQuizManager.totalQuestion && SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text == "REPORT")
            {
                SubmitButton.enabled = true;
            }
            else
            {
                SubmitButton.enabled = false;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            base.OnClose(isShutdown, userData);
        }

        private void OnCloseButtonClick()
        {
            if (!UIQuizManager.award)
            {
                dataAlert.AlertType = Constant.Type.ALERT_QUIZ_QUIT;
                openErrorMessage();
                return;
            }
            else if(!(UIQuizManager.award&& dataQuizReport.report))
            {
                dataAlert.AlertType = Constant.Type.ALERT_QUIZ_QUIT_GOTTENAWARD;
                openErrorMessage();
                return;
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizForm));
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
            }
        }

        private void openRewardForm()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
            }
        }

        private void openErrorMessage()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIErrorMessageForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIErrorMessageForm));
            }
            GameEntry.UI.OpenUIForm(EnumUIForm.UIErrorMessageForm);
        }

        private void loadAvatar()
        {
            Texture2D NPCTexture = Resources.Load<Texture2D>(npcAvatarPath);
            if (NPCTexture == null)
            {
                NPCTexture = Resources.Load<Texture2D>(AssetUtility.GetAvatarMissing());
            }
            npc_avatar.texture = NPCTexture;
        }

        private void getCurrentQuiz()
        {
            currentQuiz = UIQuizManager.quizArray[currentQuizIndex];
        }

        private void loadQuestions()
        {
            removeAllOptions();
            getCurrentQuiz();
            statement.text = currentQuiz.statement;
            multipleChoicesQuestionLoad();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChoicesContainerverticalLayoutGroup.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ContentverticalLayoutGroup.transform);
        }

        private void multipleChoicesQuestionLoad()
        {
            if (currentQuiz.OptionsCanvas.Count == 0)
            {
                instantiateChoicesPrefab(currentQuiz.type);
            }
            else
            {
                instantiateShownChoices();
            }
            Analysis.text = currentQuiz.analysis;
            AnalysisContainer.gameObject.SetActive(false);
        }
        private void instantiateChoicesPrefab(string type)
        {
            foreach (KeyValuePair<string, string> option in currentQuiz.Options)
            {
                Canvas newChoicePrefab = Instantiate(ChoicePrefab, ChoicesContainer);
                Toggle toggle = newChoicePrefab.GetComponentInChildren<Toggle>();
                toggle.isOn = false;

                if (type == Constant.Type.QUIZ_SINGLE_ANSWERS_CHOICES)
                {
                    toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
                }

                TextMeshProUGUI choiceText = newChoicePrefab.GetComponentInChildren<TextMeshProUGUI>();
                choiceText.text = option.Value;

                currentQuiz.addOptionsCanvas(option.Key, newChoicePrefab);
                currentQuiz.getToggleArray();
            }
        }

        private void instantiateShownChoices()
        {
            foreach (KeyValuePair<string, Canvas> optionCanvas in currentQuiz.OptionsCanvas)
            {
                Transform option = optionCanvas.Value.transform;
                option.gameObject.SetActive(true);
            }
        }

        private void OnToggleValueChanged(Toggle selectedToggle)
        {
            if (!isToggling)
            {
                isToggling = true;
                // make only the selected one isOn=true, and others remain isOn=false
                foreach (Toggle toggle in currentQuiz.toggleDic.Values)
                {
                    if (toggle != selectedToggle)
                    {
                        toggle.isOn = false;
                    }
                }
                selectedToggle.isOn = true;
                isToggling = false;
            }
        }

        private void getAward()
        {
            UIQuizManager.award = true;
            GameEntry.Sound.PlaySound(EnumSound.ui_Award);
            if (npcData.RewardArtifacts.Length > 1)
            {
                int[] rewardArtifacts = npcData.RewardArtifacts;
                for (int i = 0; i < rewardArtifacts.Length; i += 2)
                {
                    int id = rewardArtifacts[i];
                    int num = rewardArtifacts[i + 1];
                    dataPlayer.GetPlayerData().AddArtifact(id, num);
                }
            }
            if (npcData.RewardSkill != 0)
            {
                int id = npcData.RewardSkill;
                dataPlayer.GetPlayerData().AddSkill(id);
            }
            dataLearningProgress.update = true;
            dataPlayer.GetPlayerData().getLearningPath().finishLeantPathByNPCId(npcData.Id);
        }

        private void OnSubmitButtonClick()
        {
            if (!string.IsNullOrWhiteSpace(currentQuiz.analysis))
            {
                AnalysisContainer.gameObject.SetActive(true);
            }
            if (SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text != "REPORT")
            {
                currentQuiz.testOnToggleMCM();
                currentQuiz.haveSubmitted = true;
                updateProgress();
                updateAccuracy();
                SubmitButton.enabled = false;
            }
            else
            {
                if (UIQuizManager.calculateAccuracy() >= 0.8f)
                {
                    dataQuizReport.pass = true;
                }
                else
                {
                    dataQuizReport.pass = false;
                }
                dataQuizReport.accuracyText = rate;


                if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
                {
                    GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
                }
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCQuizRewardForm);

                //����
                dataQuizReport.report = true;
            }
        }

        private void OnLastButtonClick()
        {
            if (LastButton.enabled)
            {
                currentQuizIndex--;
                loadQuestions();
            }
        }

        private void updateLastButtonStatus()
        {
            if (currentQuizIndex > 0)
            {
                LastButton.enabled = true;
            }
            else
            {
                LastButton.enabled = false;
            }
        }

        private void OnNextButtonClick()
        {
            if (NextButton.enabled)
            {
                currentQuizIndex++;
                loadQuestions();
            }
        }

        private void updateNextButtonStatus()
        {
            if (currentQuizIndex < UIQuizManager.totalQuestion - 1)
            {
                NextButton.enabled = true;
            }
            else
            {
                NextButton.enabled = false;
            }
        }

        private void removeAllOptions()
        {
            for (int i = 0; i < ChoicesContainer.childCount; i++)
            {
                Transform option = ChoicesContainer.GetChild(i);
                option.gameObject.SetActive(false);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChoicesContainerverticalLayoutGroup.transform);

        }

        private void destroyAllOptions()
        {
            for (int i = 0; i < ChoicesContainer.childCount; i++)
            {
                Canvas canvasComponent = ChoicesContainer.GetChild(i).GetComponentInChildren<Canvas>();
                if (canvasComponent != null)
                {
                    Transform option = ChoicesContainer.GetChild(i);
                    Destroy(option.gameObject);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChoicesContainerverticalLayoutGroup.transform);
        }

    }
}
