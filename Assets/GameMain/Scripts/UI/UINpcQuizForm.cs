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
        private UIQuizManager UIQuizManager = new UIQuizManager();
        private List<UIQuiz> quizArray;

        public TextMeshProUGUI npc_name;
        public TextMeshProUGUI npc_description;
        public RawImage npc_avatar;
        public TextMeshProUGUI statement;
        public Transform ChoicesContainer;
        public Canvas ChoicePrefab;
        public Button LastButton;
        public Button NextButton;
        public Button SubmitButton;

        private int currentQuizIndex = 0;
        private UIQuiz currentQuiz;

        public Button closeButton;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);
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
            parseXMLFile();
            loadQuestions();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


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

            foreach (XmlNode node in nodes)
            {
                UIQuizManager.addQuiz(new UIQuiz(node));
            }
            quizArray = UIQuizManager.quizArray;

        }

        private void getCurrentQuiz()
        {
            currentQuiz = quizArray[currentQuizIndex];
            currentQuizIndex++;
        }

        private void loadQuestions()
        {
            //第一次进load一次，触发next last或submit重新load
            getCurrentQuiz();
            statement.text = currentQuiz.statement;
            if (currentQuiz.type == Constant.Type.QUIZ_MULTIPLE_ANSWERS_CHOICES)
            {
                multipleAnswersChoices();
            }
            else if (currentQuiz.type == Constant.Type.QUIZ_SINGLE_ANSWERS_CHOICES)
            {
                singleAnswersChoices();
            }
            else
            {
                matchingQuestions();
            }
        }

        private void multipleAnswersChoices()
        {
            //是否有作答记录
            if (!currentQuiz.haveSeen)
            {
                instantiateChoicesPrefab();
            }
            else
            {

            }
            
        }
        private void instantiateChoicesPrefab()
        {

            foreach (KeyValuePair<string, string> option in currentQuiz.Options)
            {
                Canvas newChoicePrefab = Instantiate(ChoicePrefab, ChoicesContainer);
                Toggle toggle = newChoicePrefab.GetComponentInChildren<Toggle>();
                toggle.isOn = false;

                TextMeshProUGUI choiceText = newChoicePrefab.GetComponentInChildren<TextMeshProUGUI>();
                choiceText.text = option.Value;

                currentQuiz.addOptionsCanvas(option.Key, newChoicePrefab);
                //增加确认按钮和下一个按钮的处理，保存当前作答状态
                //toggle group
            }

        }


        private void singleAnswersChoices()
        {

        }

        private void trueOrFalse()
        {

        }

        private void matchingQuestions()
        {

        }

        private void OnSubmitButtonClick()
        {

        }
    }
}
