using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NPCUIOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NPCUIOpenEventArgs).GetHashCode();

        public NPCUIOpenEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Type
        {
            get;
            private set;
        }

        public static NPCUIOpenEventArgs Create(int type)
        {
            NPCUIOpenEventArgs e = ReferencePool.Acquire<NPCUIOpenEventArgs>();
            e.Type = type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

