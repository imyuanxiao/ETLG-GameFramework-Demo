using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlayerDeadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerDeadEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public static PlayerDeadEventArgs Create() 
        {
            PlayerDeadEventArgs playerDeadEventArgs = ReferencePool.Acquire<PlayerDeadEventArgs>();
            return playerDeadEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}
