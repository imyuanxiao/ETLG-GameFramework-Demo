using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using ETLG.Data;

namespace ETLG
{
    [Serializable]

    public class EntityDataBossEnemy : EntityData
    {
        public BossEnemyData BossEnemyData
        {
            get;
            private set;
        }

        public static EntityDataBossEnemy Create(BossEnemyData bossEnemyData, object userData = null) 
        {
            EntityDataBossEnemy entityData = ReferencePool.Acquire<EntityDataBossEnemy>();
            entityData.BossEnemyData = bossEnemyData;
            setPosition(entityData);
            return entityData;
        }

        private static void setPosition(EntityDataBossEnemy entityData)
        {
            entityData.Position = new Vector3(0f, 0f, 60f);
            entityData.Rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public override void Clear()
        {
            base.Clear();
            BossEnemyData = null;
        }
    }
}
