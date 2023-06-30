using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using ETLG.Data;

namespace ETLG
{
    public class EntityBossEnemy : EntityLogicEx
    {
        [HideInInspector] public BossEnemyData data;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            this.data = ((EntityDataBossEnemy) userData).BossEnemyData;
        }
    }
}
