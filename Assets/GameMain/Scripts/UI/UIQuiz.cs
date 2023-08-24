using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;
using System.Linq;

namespace ETLG.Data
{
    public sealed class UIQuiz
    {
        private List<string> wrongAnswers = new List<string>();
        private XmlNode node { get; }
        public int questionTime { get; }
        public string type { get; }
        public bool isCorrect { get; set; }
        public bool haveSubmitted { get; set; }
        public bool haveShown { get; set; }
        public string analysis { get; }
        public bool analysisShown = false;
        public string statement { get; }
        private SortedDictionary<string, string> options = new SortedDictionary<string, string>();
        private SortedDictionary<string, Canvas> optionsCanvas = new SortedDictionary<string, Canvas>();
        public Dictionary<string, Toggle> toggleDic;
        private List<string> selectAnswers = new List<string>();

        private List<string> rightAnswers = new List<string>();
        public UIQuiz(XmlNode node)
        {
            this.node = node;
            this.type = node.Attributes["type"].Value;
            //this.hint = node.SelectSingleNode("hint").InnerText;
            this.analysis = node.SelectSingleNode("analysis").InnerText;
            this.statement = node.SelectSingleNode("statement").InnerText;
            //this.questionTime = int.Parse(node.Attributes["timelimit"].Value);
            getRightAnswers();
            getAllOptions();
        }

        public void reset()
        {
            wrongAnswers = new List<string>();
            isCorrect = false;
            haveSubmitted = false;
            haveShown = false;
            optionsCanvas = new SortedDictionary<string, Canvas>();
            selectAnswers = new List<string>();
            analysisShown = false;
        }

        private void getAllOptions()
        {
            XmlNodeList optionsList = node.SelectNodes("options/option");
            foreach (XmlNode option in optionsList)
            {
                options.Add(option.Attributes["id"].Value, option.InnerText);
            }
        }
        private void getRightAnswers()
        {
            XmlNodeList answersList = node.SelectNodes("answers/answer");
            foreach (XmlNode answer in answersList)
            {
                rightAnswers.Add(answer.InnerText);
            }
        }

        public List<string> RightAnswers
        {
            get { return rightAnswers; }
        }

        public SortedDictionary<string, string> Options
        {
            get { return options; }
        }

        public SortedDictionary<string, Canvas> OptionsCanvas
        {
            get { return optionsCanvas; }
        }

        public void addOptionsCanvas(string id, Canvas newOption)
        {
            optionsCanvas.Add(id, newOption);
        }

        public void getToggleArray()
        {
            toggleDic = new Dictionary<string, Toggle>();
            foreach (KeyValuePair<string, Canvas> kvp in optionsCanvas)
            {
                toggleDic.Add(kvp.Key, kvp.Value.GetComponentInChildren<Toggle>());
            }
        }
        private void collectSelectAnswers()
        {
            foreach (KeyValuePair<string, Toggle> kvp in toggleDic)
            {
                if (kvp.Value.isOn)
                {
                    selectAnswers.Add(kvp.Key);
                }
            }
        }
        public void testOnToggleMCM()
        {
            collectSelectAnswers();
            // find the wrong selected options
            wrongAnswers = selectAnswers.Except(rightAnswers).ToList();
            testCorrect();
            colorAnswers();
        }

        private void testCorrect()
        {
            DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            NPCData npcData = GameEntry.Data.GetData<DataNPC>().GetCurrentNPCData();
            if (wrongAnswers.Count == 0 && selectAnswers.Count == rightAnswers.Count)
            {
                isCorrect = true;
                GameEntry.Sound.PlaySound(EnumSound.ui_quiz_correct);  
            }
            else
            {
                isCorrect = false;
                GameEntry.Sound.PlaySound(EnumSound.ui_quiz_wrong);
            }
            dataPlayer.GetPlayerData().addQuizResult(npcData.Id, isCorrect);
        }

        public void colorAnswers()
        {
            foreach (string wrongAnswer in wrongAnswers)
            {
                changeOptionColor(wrongAnswer, false);
            }
            foreach (string rightAnswer in rightAnswers)
            {
                changeOptionColor(rightAnswer, true);
            }

        }

        private void changeOptionColor(string optionKey, bool result)
        {
            if (result)
            {
                optionsCanvas[optionKey].GetComponentInChildren<RawImage>().color = UIHexColor.HexToColor("386A38");
            }
            else
            {
                optionsCanvas[optionKey].GetComponentInChildren<RawImage>().color = UIHexColor.HexToColor("713838");
            }
        }

        public bool testAllSwitchOff()
        {
            if (toggleDic.Count == 0)
            {
                return true;
            }
            foreach (KeyValuePair<string, Toggle> kvp in toggleDic)
            {
                if (kvp.Value.isOn)
                {
                    return false;
                }
            }
            return true;
        }

        private void switchAlloff()
        {
            foreach (KeyValuePair<string, Toggle> kvp in toggleDic)
            {
                kvp.Value.GetComponentInChildren<Toggle>().isOn = false;
            }
        }
    }

}