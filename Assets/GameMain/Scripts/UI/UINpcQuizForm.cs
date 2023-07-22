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
        private string npcAvatarPath;
        private string XMLPath;
        private UIQuizManager UIQuizManager=null;
        private List<UIQuiz> quizArray;
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

            npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();

            npc_name.text = npcData.Name;
            npcAvatarPath = AssetUtility.GetNPCAvatar(npcData.Id.ToString());
            npc_description.text = npcData.Description;
            XMLPath = npcData.QuizXML;

            SubmitButton.onClick.AddListener(OnSubmitButtonClick);

            loadAvatar();
            
            if (UIQuizManager==null)
            {
                parseXMLFile();
            }
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

        }

        private void updateProgress()
        {
            string progress = UIQuizManager.TotalSubmitQuestions.ToString() + " / " + UIQuizManager.totalQuestion.ToString();
            LeftQuestionsText.text = progress;
            ProgressSlider.value = UIQuizManager.progressFloat();
        }

        private void updateAccuracy()
        {
            string rate= (UIQuizManager.calculateAccuracy() * 100).ToString("F0");
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
            else
            {
                SubmitButton.enabled = false;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            GameEntry.Event.Fire(this, NPCUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
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

        private void parseXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLPath);
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("question");
            UIQuizManager = new UIQuizManager();
            foreach (XmlNode node in nodes)
            {
                UIQuizManager.addQuiz(new UIQuiz(node));
            }
            quizArray = UIQuizManager.quizArray;
            UIQuizManager.totalQuestion = quizArray.Count;
        }

        private void getCurrentQuiz()
        {
            currentQuiz = quizArray[currentQuizIndex];
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

        private void OnSubmitButtonClick()
        {
            currentQuiz.testOnToggleMCM();
            currentQuiz.haveSubmitted = true;
            updateProgress();
            updateAccuracy();
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

    }
}
