using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SaveGameEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SaveGameEventArgs).GetHashCode();
        public override int Id 
        {
            get 
            {
                return EventId;
            }
        }

        public int SaveId  { get; private set; }

        public static SaveGameEventArgs Create(int saveId) 
        {
            SaveGameEventArgs e = ReferencePool.Acquire<SaveGameEventArgs>();
            e.SaveId = saveId;
            return e;
        }

        public override void Clear()
        {
            SaveId = 0;
        }
    }
}