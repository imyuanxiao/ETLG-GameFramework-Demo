using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NPCDialogOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NPCDialogOpenEventArgs).GetHashCode();

        public NPCDialogOpenEventArgs()
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

        // 0 - talk, 1 - trade
        public static NPCDialogOpenEventArgs Create(int type)
        {
            NPCDialogOpenEventArgs nPCDialogOpenEventArgs = ReferencePool.Acquire<NPCDialogOpenEventArgs>();
            nPCDialogOpenEventArgs.Type = type;
            return nPCDialogOpenEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

