using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class GamePauseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GamePauseEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public EnumUIForm UIPauseId 
        {
            get;
            private set;
        }

        public static GamePauseEventArgs Create(EnumUIForm uiPauseId, object userData = null)
        {
            GamePauseEventArgs gamePauseEventArgs = ReferencePool.Acquire<GamePauseEventArgs>();
            gamePauseEventArgs.UIPauseId = uiPauseId;
            return gamePauseEventArgs;
        }

        public override void Clear()
        {
            UIPauseId = 0;
        }
    }
}
