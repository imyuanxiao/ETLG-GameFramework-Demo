using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Collections.Specialized;

namespace ETLG.Data
{
    public sealed class UIQuiz
    {
        public UIQuiz(XmlNode node)
        {
            this.node = node;
            this.type = node.Attributes["type"].Value;
            this.hint = node.SelectSingleNode("hint").InnerText;
            this.analysis = node.SelectSingleNode("analysis").InnerText;
            this.statement = node.SelectSingleNode("statement").InnerText;
            this.questionTime = int.Parse(node.Attributes["timelimit"].Value);
            getRightAnswers();
            getAllOptions();
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
            foreach(XmlNode answer in answersList)
            {
                rightAnswers.Add(answer.InnerText);
            }
        }
        
        private XmlNode node{ get; }
        public int questionTime { get; }
        public string type { get; }
        public bool trueOrFalse { get; set; }
        public bool haveSeen { get; set; }
        public string hint { get; }
        public string analysis { get; }
        public string statement { get; }

        private List<string> rightAnswers = new List<string>();

        public List<string> RightAnswers
        {
            get { return rightAnswers; }
        }

        private SortedDictionary<string,string> options = new SortedDictionary<string, string>();
        public SortedDictionary<string, string> Options
        {
            get { return options; }
        }
        private SortedDictionary<string, Canvas> optionsCanvas = new SortedDictionary<string, Canvas>();
        public SortedDictionary<string, Canvas> OptionsCanvas
        {
            get { return optionsCanvas; }
        }

        public void addOptionsCanvas(string id,Canvas newOption)
        {
            optionsCanvas.Add(id,newOption);
        }
        public List<Toggle> toggleArray { get; set; }
        public Toggle lastSelectedToggle { get; set; }

        public void testOnToggle()
        {
            foreach(KeyValuePair<string, Canvas> kvp in optionsCanvas)
            {

            }
        }


    }
}