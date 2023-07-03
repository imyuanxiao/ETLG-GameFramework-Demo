using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class TipOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TipOpenEventArgs).GetHashCode();

        public TipOpenEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector3 position { get; set; }
        public string tipTitle { get; set; }


        public static TipOpenEventArgs Create(Vector3 position, string title)
        {
            TipOpenEventArgs tipOpenEventArgs = ReferencePool.Acquire<TipOpenEventArgs>();
            tipOpenEventArgs.position = position;
            tipOpenEventArgs.tipTitle = title;
            return tipOpenEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

