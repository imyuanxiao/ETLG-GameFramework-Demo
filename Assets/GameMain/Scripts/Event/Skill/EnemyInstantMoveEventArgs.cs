using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class EnemyInstantMoveEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyInstantMoveEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public static EnemyInstantMoveEventArgs Create() 
        {
            EnemyInstantMoveEventArgs e = ReferencePool.Acquire<EnemyInstantMoveEventArgs>();
            return e;
        }

        public override void Clear()
        {
            
        }
    }
}
