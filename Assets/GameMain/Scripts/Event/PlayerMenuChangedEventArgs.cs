using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlayerMenuChangedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerMenuChangedEventArgs).GetHashCode();

        public PlayerMenuChangedEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static PlayerMenuChangedEventArgs Create()
        {
            PlayerMenuChangedEventArgs e = ReferencePool.Acquire<PlayerMenuChangedEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

