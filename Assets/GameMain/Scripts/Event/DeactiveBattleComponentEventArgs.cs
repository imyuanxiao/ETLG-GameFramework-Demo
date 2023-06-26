using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class DeactiveBattleComponentEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(DeactiveBattleComponentEventArgs).GetHashCode();

        public override int Id 
        {
            get
            {
                return EventId;
            }
        }

        public DeactiveBattleComponentEventArgs()
        {

        }

        public static DeactiveBattleComponentEventArgs Create()
        {
            DeactiveBattleComponentEventArgs e = ReferencePool.Acquire<DeactiveBattleComponentEventArgs>();
            return e;
        }

        public override void Clear()
        {
            
        }
    }
}
