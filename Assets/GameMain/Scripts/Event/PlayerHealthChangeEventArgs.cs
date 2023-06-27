using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlayerHealthChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerHealthChangeEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public int CurrentHealth
        {
            get;
            private set;
        }

        public static PlayerHealthChangeEventArgs Create(int currentHealth)
        {
            PlayerHealthChangeEventArgs playerHealthChangeEventArgs = ReferencePool.Acquire<PlayerHealthChangeEventArgs>();
            playerHealthChangeEventArgs.CurrentHealth = currentHealth;
            return playerHealthChangeEventArgs;
        }

        public override void Clear()
        {
            CurrentHealth = 0;
        }
    }
}
