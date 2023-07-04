using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NPCUICloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NPCUICloseEventArgs).GetHashCode();

        public NPCUICloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static NPCUICloseEventArgs Create()
        {
            NPCUICloseEventArgs e = ReferencePool.Acquire<NPCUICloseEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

