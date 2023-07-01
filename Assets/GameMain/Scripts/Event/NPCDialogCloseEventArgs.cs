using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NPCDialogCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NPCDialogCloseEventArgs).GetHashCode();

        public NPCDialogCloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }



        // 0 - talk, 1 - trade
        public static NPCDialogCloseEventArgs Create()
        {
            NPCDialogCloseEventArgs nPCDialogCloseEventArgs = ReferencePool.Acquire<NPCDialogCloseEventArgs>();
            return nPCDialogCloseEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

