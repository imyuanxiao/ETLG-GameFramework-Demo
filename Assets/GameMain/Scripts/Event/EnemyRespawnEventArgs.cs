using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class EnemyRespawnEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyRespawnEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public BossEnemyHealth bossEnemyHealth  { get; private set; }

        public static EnemyRespawnEventArgs Create(BossEnemyHealth bossEnemyHealth)
        {
            EnemyRespawnEventArgs e = ReferencePool.Acquire<EnemyRespawnEventArgs>();
            e.bossEnemyHealth = bossEnemyHealth;
            return e;
        }

        public override void Clear()
        {
            bossEnemyHealth = null;
        }
    }
}
