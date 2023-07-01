using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AllUIFormCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AllUIFormCloseEventArgs).GetHashCode();

        public AllUIFormCloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }



        public static AllUIFormCloseEventArgs Create()
        {
            AllUIFormCloseEventArgs uIFormCloseEventArgs = ReferencePool.Acquire<AllUIFormCloseEventArgs>();
            return uIFormCloseEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

