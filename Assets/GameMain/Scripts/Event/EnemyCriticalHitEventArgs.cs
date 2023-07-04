using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class EnemyCriticalHitEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyCriticalHitEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }

        public EnumEntity EnemyType { get; private set; }

        public static EnemyCriticalHitEventArgs Create(EnumEntity enemyType) 
        {
            EnemyCriticalHitEventArgs enemyCriticalHitEventArgs = ReferencePool.Acquire<EnemyCriticalHitEventArgs>();
            enemyCriticalHitEventArgs.EnemyType = enemyType;
            return enemyCriticalHitEventArgs;
        }

        public override void Clear()
        {
            EnemyType = EnumEntity.None;
        }
    }
}
