using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class BasicBattleWinEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BasicBattleWinEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public int BasicEnemyKilled { get; private set; }
        public int BasicEnemyPassed { get; private set; }

        public static BasicBattleWinEventArgs Create(int basicEnemyKilled, int basicEnemyPassed)
        {
            BasicBattleWinEventArgs basicBattleWinEventArgs = ReferencePool.Acquire<BasicBattleWinEventArgs>();
            basicBattleWinEventArgs.BasicEnemyKilled = basicEnemyKilled;
            basicBattleWinEventArgs.BasicEnemyPassed = basicEnemyPassed;
            return basicBattleWinEventArgs;
        }

        public override void Clear()
        {
            BasicEnemyKilled = 0;
            BasicEnemyPassed = 0;
        }
    }
}
