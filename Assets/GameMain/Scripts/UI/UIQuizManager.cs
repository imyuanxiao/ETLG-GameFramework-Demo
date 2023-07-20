using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETLG.Data
{
    public sealed class UIQuizManager
    {
        public bool award = false;

        public List<UIQuiz> quizArray = new List<UIQuiz>();

        public int totalQuestion { get; set; }
        private int totalSubmitQuestions;
        private int correctQuestions;

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
            Debug.Log("corrent" + CorrectQuestions);
            Debug.Log("submit" + TotalSubmitQuestions);
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
