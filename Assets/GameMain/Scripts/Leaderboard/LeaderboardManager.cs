using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETLG
{
    public class LeaderboardManager : Singleton<LeaderboardManager>
    {
        [HideInInspector] public LeaderboardData leaderboardData;
        [HideInInspector] public int planetNum;
        [HideInInspector] public int iconId;

        // 在Awake方法中初始化单例实例
        protected override void Awake()
        {
            base.Awake();
        }
        private void OnEnable()
        {
            this.leaderboardData = null;
        }
        private void OnDisable()
        {
            this.leaderboardData = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
