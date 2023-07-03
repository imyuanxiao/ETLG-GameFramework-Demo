using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class TipCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TipCloseEventArgs).GetHashCode();

        public TipCloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static TipCloseEventArgs Create()
        {
            TipCloseEventArgs tipCloseEventArgs = ReferencePool.Acquire<TipCloseEventArgs>();
            return tipCloseEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

