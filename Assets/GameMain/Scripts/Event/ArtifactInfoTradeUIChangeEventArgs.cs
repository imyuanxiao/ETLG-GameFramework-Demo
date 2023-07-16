using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ArtifactInfoTradeUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ArtifactInfoTradeUIChangeEventArgs).GetHashCode();

        public ArtifactInfoTradeUIChangeEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Type { get; set; }

        public static ArtifactInfoTradeUIChangeEventArgs Create(int Type)
        {
            ArtifactInfoTradeUIChangeEventArgs e = ReferencePool.Acquire<ArtifactInfoTradeUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

