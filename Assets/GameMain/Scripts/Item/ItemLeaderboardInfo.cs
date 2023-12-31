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
       // private DataPlayer dataPlayer;
        public Button playerInfo;
        public TextMeshProUGUI s_rank = null;
        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_score = null;
        private  LeaderboardData leaderboardData;
        public Transform container;
        public bool refresh;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerInfo.onClick.AddListener(OnPlayerInfoClick);
        }
        public void SetData(LeaderboardData leaderboardData, Transform container,int Type)
        {
            this.leaderboardData = leaderboardData;
            this.container = container;
            showContent(Type);
        }
        private void showContent(int Type)
        {
            s_rank.text = this.leaderboardData.Rank.ToString();
            if (this.leaderboardData.Name != null)
            {
                s_name.text = this.leaderboardData.Name.ToString();
            }
            switch (Type)
            {
                case Constant.Type.LB_ACHIVEMENT:
                    s_score.text = this.leaderboardData.data.ToString();
                    break;
                case Constant.Type.LB_SPACESHIP:
                    s_score.text = this.leaderboardData.data.ToString();
                    break;
                case Constant.Type.LB_LEARNING_PROGRESS:
                    s_score.text = UIFloatString.FloatToString( this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_AI:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_BLOCKCHAIN:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_CLOUDCOMPUTING:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_CYBERSECURITY:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_DATASCIENCE:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_IOT:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
                    break;
                case Constant.Type.LB_BOSS_FINAL:
                    s_score.text = ConvertFloatToTimeString(this.leaderboardData.data);
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
            GameEntry.Data.GetData<DataBackend>().selectedRank = leaderboardData;
            BackendDataManager.Instance.GetUserProfileByUserId(leaderboardData.Id);
            

        }

        public void UpdateData(LeaderboardData leaderboardData, int Type)
        {
            this.leaderboardData = leaderboardData;
            showContent(Type);
        }
    }
}
