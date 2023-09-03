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
        public TextMeshProUGUI QuestionType;

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
        public Transform AnalysisContainer;
        public Canvas AnalysisPrefab;
        public VerticalLayoutGroup ContentVerticalLayoutGroup;
        public RectTransform ContainerRectTransform;
        public Transform Pool;

        private UIQuiz currentQuiz;
        private Vector2 currentPosition;
        private bool isToggling = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
            LastButton.onClick.AddListener(OnLastButtonClick);
            NextButton.onClick.AddListener(OnNextButtonClick);
            SubmitButton.onClick.AddListener(OnSubmitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(Constant.Type.TUTORIAL_NPC_QUIZ);
            GameEntry.Sound.StopMusic();

            UIQuizManager = null;
            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAlert = GameEntry.Data.GetData<DataAlert>();
            dataQuizReport = GameEntry.Data.GetData<DataQuiz>();
            dataQuizReport.reset();
            dataLearningProgress = GameEntry.Data.GetData<DataLearningProgress>();

            if (dataLearningProgress.open)
            {
                ModifyPositionX(Constant.Type.POSITION_X_RIGHT);
            }

            npc_name.text = npcData.Name;
            npcAvatarPath = AssetUtility.GetNPCAvatar(npcData.Id.ToString());
            npc_description.text = npcData.Domain + "\n" + npcData.Course + "\n" + npcData.Chapter;

            loadAvatar();
            setUIQuizManager();
            dataQuizReport.boss = UIQuizManager.boss;
            dataQuizReport.award = UIQuizManager.award;

            loadQuestions();
            updateProgress();
            updateAccuracy();
        }

        private void setUIQuizManager()
        {
            UIQuizManager tempUIQuizManager = dataPlayer.GetPlayerData().getUIQuizManager(npcData.Id);
            if (tempUIQuizManager == null)
            {
                XMLPath = AssetUtility.GetQuizXML(npcData.Id.ToString());
                UIQuizManager = new UIQuizManager(XMLPath, dataPlayer.GetPlayerData().getChapterFinish(npcData.Id));
                dataPlayer.GetPlayerData().setUIQuizManagerById(npcData.Id, UIQuizManager);
                int[] newIntArray = { 0, 0 };
                dataPlayer.GetPlayerData().QuizesSaveData[npcData.Id] = newIntArray;
            }
            else
            {
                UIQuizManager = tempUIQuizManager;
            }
        }

        private void ModifyPositionX(float newX)
        {
            currentPosition.x = currentPosition.x + newX;
            ContainerRectTransform.anchoredPosition = currentPosition;
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
                UIQuizManager.reset();
                int[] newIntArray = { 0, 0 };
                dataPlayer.GetPlayerData().QuizesSaveData[npcData.Id] = newIntArray;
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
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);
            if (dataLearningProgress.open)
            {
                ModifyPositionX(Constant.Type.POSITION_X_LEFT);
            }
            base.OnClose(isShutdown, userData);
        }

        private void OnCloseButtonClick()
        {
            if (!dataPlayer.GetPlayerData().getChapterFinish(npcData.Id))
            {
                dataAlert.AlertType = Constant.Type.ALERT_QUIZ_QUIT;
                openErrorMessage();
                return;
            }
            else if (!(dataPlayer.GetPlayerData().getChapterFinish(npcData.Id) && dataQuizReport.report))
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
            currentQuiz = UIQuizManager.quizArray[UIQuizManager.currentQuizIndex];
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
            destroyAnalysisContainer();
            if (currentQuiz.OptionsCanvas.Count == 0)
            {
                instantiateChoicesPrefab(currentQuiz.type);

            }
            else
            {
                instantiateShownChoices();
                if (currentQuiz.analysisShown)
                {
                    setAnalysisPrefab();
                }
            }
            showQuestionType();
        }

        private void showQuestionType()
        {
            if (currentQuiz.type == Constant.Type.QUIZ_MULTIPLE_ANSWERS_CHOICES)
            {
                QuestionType.text = "Multiple-choice with multiple answers";
            }
            else if (currentQuiz.type == Constant.Type.QUIZ_SINGLE_ANSWERS_CHOICES)
            {
                QuestionType.text = "Multiple-choice with single answer";
            }
            else if (currentQuiz.type == Constant.Type.QUIZ_TRUEORFALSE)
            {
                QuestionType.text = "True or False";
            }
        }

        private void destroyAnalysisContainer()
        {
            for (int i = AnalysisContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(AnalysisContainer.GetChild(i).gameObject);
            }
        }

        private void instantiateChoicesPrefab(string type)
        {
            foreach (KeyValuePair<string, string> option in currentQuiz.Options)
            {
                Canvas newChoicePrefab = Instantiate(ChoicePrefab, ChoicesContainer);
                Toggle toggle = newChoicePrefab.GetComponentInChildren<Toggle>();
                toggle.isOn = false;

                if (type == Constant.Type.QUIZ_SINGLE_ANSWERS_CHOICES|| type == Constant.Type.QUIZ_TRUEORFALSE)
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
                //option.SetParent(ChoicesContainer);
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
            dataLearningProgress.getAward();

            dataPlayer.GetPlayerData().getLearningPath().finishLeantPathByNPCId(npcData.Id);
            dataPlayer.GetPlayerData().setFinishChapter(npcData.Id);
            dataPlayer.GetPlayerData().getPassQuizAndFinishDialog();
            dataPlayer.GetPlayerData().updateAchievement_QuizNumber();
        }

        private void setAnalysisPrefab()
        {
            if (!string.IsNullOrWhiteSpace(currentQuiz.analysis))
            {
                Canvas analysisPrefab = Instantiate(AnalysisPrefab, AnalysisContainer);
                TextMeshProUGUI analysisText = analysisPrefab.GetComponentInChildren<TextMeshProUGUI>();
                analysisText.text = currentQuiz.analysis;
                currentQuiz.analysisShown = true;
            }
        }

        private void OnSubmitButtonClick()
        {
            destroyAnalysisContainer();
            setAnalysisPrefab();
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
                dataQuizReport.report = true;
            }
        }

        private void OnLastButtonClick()
        {
            if (LastButton.enabled)
            {
                UIQuizManager.currentQuizIndex--;
                loadQuestions();
            }
        }

        private void updateLastButtonStatus()
        {
            if (UIQuizManager.currentQuizIndex > 0)
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
                UIQuizManager.currentQuizIndex++;
                loadQuestions();
            }
        }

        private void updateNextButtonStatus()
        {
            if (UIQuizManager.currentQuizIndex < UIQuizManager.totalQuestion - 1)
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
            for (int i = ChoicesContainer.childCount-1; i >=0; i--)
            {
                Transform option = ChoicesContainer.GetChild(i);
                option.gameObject.SetActive(false);
                //option.SetParent(Pool);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChoicesContainerverticalLayoutGroup.transform);

        }

        private void destroyAllOptions()
        {
            removeAllOptions();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ChoicesContainerverticalLayoutGroup.transform);
        }

    }
}
