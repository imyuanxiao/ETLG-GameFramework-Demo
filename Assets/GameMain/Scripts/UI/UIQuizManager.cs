using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;

namespace ETLG.Data
{
    public sealed class UIQuizManager
    {
        public bool award = false;

        public List<UIQuiz> quizArray = new List<UIQuiz>();
        public bool boss = false;
        public int totalQuestion { get; set; }
        private int totalSubmitQuestions;
        private int correctQuestions;
        public int currentQuizIndex=0;

        public void reset()
        {
            totalSubmitQuestions = 0;
            correctQuestions = 0;
            currentQuizIndex = 0;
            foreach (UIQuiz quiz in quizArray)
            {
                quiz.reset();
            }
        }

        public UIQuizManager(string XMLPath)
        {
            parseXMLFile(XMLPath);
        }

        private void parseXMLFile(string XMLPath)
        {
            TextAsset xmlFile = Resources.Load<TextAsset>(XMLPath);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);

            XmlNode quizNode = xmlDoc.SelectSingleNode("quiz");
            XmlAttribute bossAttribute = quizNode.Attributes["boss"];
            if (bossAttribute != null)
            {
                boss = true;
            }
            else
            {
                boss = false;
            }

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("question");
            foreach (XmlNode node in nodes)
            {
                addQuiz(new UIQuiz(node));
            }
            totalQuestion = quizArray.Count;
        }

        public int TotalSubmitQuestions
        {
            get 
            {
                totalSubmitQuestions = 0;
                foreach (UIQuiz quiz in quizArray)
                {
                    if (quiz.haveSubmitted)
                    {
                        totalSubmitQuestions++;
                    }
                }
                return totalSubmitQuestions; 
            }
        }
        public int CorrectQuestions
        {
            get
            {
                correctQuestions = 0;
                foreach (UIQuiz quiz in quizArray)
                {
                    if (quiz.isCorrect)
                    {
                        correctQuestions++;
                    }
                }
                return correctQuestions;
            }
        }

        public float progressFloat()
        {
            return (float)TotalSubmitQuestions / totalQuestion;
        }

        public void addQuiz(UIQuiz quiz)
        {
            quizArray.Add(quiz);
        }

        public float calculateAccuracy()
        {
            if(TotalSubmitQuestions == 0)
            {
                return 0;
            }
            float accuracy = (float)CorrectQuestions / (float)TotalSubmitQuestions;
            return accuracy;
        }

        public bool testFinishQuiz()
        {
            if (totalSubmitQuestions == totalQuestion)
            {
                return true;
            }
            return false;
        }
    }


}

