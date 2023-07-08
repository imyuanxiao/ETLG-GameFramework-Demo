using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class EquippedModuleChangesEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EquippedModuleChangesEventArgs).GetHashCode();

        public EquippedModuleChangesEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static EquippedModuleChangesEventArgs Create()
        {
            EquippedModuleChangesEventArgs e = ReferencePool.Acquire<EquippedModuleChangesEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

