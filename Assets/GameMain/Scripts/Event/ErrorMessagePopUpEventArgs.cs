using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ErrorMessagePopPUpEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ErrorMessagePopPUpEventArgs).GetHashCode();

        public ErrorMessagePopPUpEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ErrorMessagePopPUpEventArgs Create()
        {
            ErrorMessagePopPUpEventArgs e = ReferencePool.Acquire<ErrorMessagePopPUpEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

