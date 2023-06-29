using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class BattleWinEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BattleWinEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public EnumEntity EnemyType { get; private set; }

        public static BattleWinEventArgs Create(EnumEntity enemyType) 
        {
            BattleWinEventArgs battleWinEventArgs = ReferencePool.Acquire<BattleWinEventArgs>();
            battleWinEventArgs.EnemyType = enemyType;
            return battleWinEventArgs;
        }

        public override void Clear()
        {
            EnemyType = EnumEntity.None;
        }
    }
}
