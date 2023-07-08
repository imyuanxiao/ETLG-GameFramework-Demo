using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ArtifactInfoUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ArtifactInfoUIChangeEventArgs).GetHashCode();

        public ArtifactInfoUIChangeEventArgs()
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

        public static ArtifactInfoUIChangeEventArgs Create(int Type)
        {
            ArtifactInfoUIChangeEventArgs e = ReferencePool.Acquire<ArtifactInfoUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

