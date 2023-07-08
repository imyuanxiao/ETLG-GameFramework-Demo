using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlayerRespawnEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerRespawnEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public PlayerHealth PlayerHealth { get; private set; }

        public static PlayerRespawnEventArgs Create(PlayerHealth health) 
        {
            PlayerRespawnEventArgs e = ReferencePool.Acquire<PlayerRespawnEventArgs>();
            e.PlayerHealth = health;
            return e;
        }

        public override void Clear()
        {
            PlayerHealth = null;
        }
    }
}
