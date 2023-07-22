using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlayerZoneUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerZoneUIChangeEventArgs).GetHashCode();

        public PlayerZoneUIChangeEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Type { get; set; }

        public static PlayerZoneUIChangeEventArgs Create(int Type)
        {
            PlayerZoneUIChangeEventArgs e = ReferencePool.Acquire<PlayerZoneUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

