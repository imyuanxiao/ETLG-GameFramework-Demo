using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ETLG.Data
{
    public sealed class UIQuizManager
    {
        public List<UIQuiz> quizArray = new List<UIQuiz>();

        private int totalSeenQuestions = 0;
        private int correctQuestions = 0;

        public int TotalSeenQuestions
        {
            get { return totalSeenQuestions; }
        }
        public int CorrectQuestions
        {
            get { return correctQuestions; }
        }

        public void addQuiz(UIQuiz quiz)
        {
            quizArray.Add(quiz);
        }

        public int calculateAccuracy()
        {
            foreach (UIQuiz quiz in quizArray)
            {
                if (quiz.haveSeen)
                {
                    if (quiz.trueOrFalse)
                    {
                        correctQuestions++;
                    }
                    totalSeenQuestions++;
                }
            }
            int accuracy = correctQuestions / totalSeenQuestions;
            return accuracy;
        }
    }


}

