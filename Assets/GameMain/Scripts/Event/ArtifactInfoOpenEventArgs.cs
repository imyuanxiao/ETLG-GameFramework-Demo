using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ArtifactInfoOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ArtifactInfoOpenEventArgs).GetHashCode();

        public ArtifactInfoOpenEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ArtifactInfoOpenEventArgs Create()
        {
            ArtifactInfoOpenEventArgs artifactInfoOpenEventArgs = ReferencePool.Acquire<ArtifactInfoOpenEventArgs>();
            return artifactInfoOpenEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

