using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AISpaceshipDestroyedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AISpaceshipDestroyedEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }

        public static AISpaceshipDestroyedEventArgs Create() 
        {
            AISpaceshipDestroyedEventArgs e = ReferencePool.Acquire<AISpaceshipDestroyedEventArgs>();
            return e;
        }

        public override void Clear()
        {
            
        }
    }
}
