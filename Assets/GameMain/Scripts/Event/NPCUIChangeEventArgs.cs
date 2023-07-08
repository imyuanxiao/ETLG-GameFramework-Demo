using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NPCUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NPCUIChangeEventArgs).GetHashCode();

        public NPCUIChangeEventArgs()
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

        public static NPCUIChangeEventArgs Create(int type)
        {
            NPCUIChangeEventArgs e = ReferencePool.Acquire<NPCUIChangeEventArgs>();
            e.Type = type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

