using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemLeaderboardInfo : ItemLogicEx
    {
        private DataPlayer dataPlayer;
        public Button playerInfo;
        public TextMeshProUGUI s_rank = null;
        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_score = null;
        public Transform container;

        private LeaderboardData LeaderboardData;
        public bool refresh;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerInfo.onClick.AddListener(OnPlayerInfoClick);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }
        public void SetData(LeaderboardData leaderboardData, Transform container,int Type)
        {
            this.container = container;
            this.LeaderboardData = leaderboardData;
            showContent(Type);
        }
        private void showContent(int Type)
        {
            s_rank.text = this.LeaderboardData.Rank.ToString();
            if (this.LeaderboardData.Name != null)
            {
                s_name.text = this.LeaderboardData.Name.ToString();
            }
            switch (Type)
            {
                case Constant.Type.LB_ACHIVEMENT:
                    s_score.text = this.LeaderboardData.AchievementScore.ToString();
                    break;
                case Constant.Type.LB_SPACESHIP:
                    s_score.text = this.LeaderboardData.SpaceshipScore.ToString();
                    break;
                case Constant.Type.LB_BOSS_AI:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_AI);
                    break;
                case Constant.Type.LB_BOSS_BLOCKCHAIN:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_Blockchain);
                    break;
                case Constant.Type.LB_BOSS_CLOUDCOMPUTING:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_CloudComputing);
                    break;
                case Constant.Type.LB_BOSS_CYBERSECURITY:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_Cybersecurity);
                    break;
                case Constant.Type.LB_BOSS_DATASCIENCE:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_DataScience);
                    break;
                case Constant.Type.LB_BOSS_IOT:
                    s_score.text = ConvertFloatToTimeString(this.LeaderboardData.Boss_IoT);
                    break;
            }
        }
        private string ConvertFloatToTimeString(float seconds)
        {
            int totalSeconds = Mathf.FloorToInt(seconds);
            int hours = totalSeconds / 3600;
            int remainingSeconds = totalSeconds % 3600;
            int minutes = remainingSeconds / 60;
            int remainingMinutes = remainingSeconds % 60;

            string hoursString = hours.ToString().PadLeft(2, '0');
            string minutesString = minutes.ToString().PadLeft(2, '0');
            string secondsString = remainingMinutes.ToString().PadLeft(2, '0');

            return $"{hoursString}:{minutesString}:{secondsString}";
        }

        private void OnPlayerInfoClick()
        {
            
        }

        public void UpdateData(LeaderboardData leaderboardData, int Type)
        {
            this.LeaderboardData = leaderboardData;
            showContent(Type);
        }
    }
}
