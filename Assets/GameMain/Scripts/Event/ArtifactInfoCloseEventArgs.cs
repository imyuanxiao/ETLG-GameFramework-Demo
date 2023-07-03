using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ArtifactInfoCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ArtifactInfoCloseEventArgs).GetHashCode();

        public ArtifactInfoCloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static ArtifactInfoCloseEventArgs Create()
        {
            ArtifactInfoCloseEventArgs artifactInfoCloseEvent = ReferencePool.Acquire<ArtifactInfoCloseEventArgs>();
            return artifactInfoCloseEvent;
        }

        public override void Clear()
        {
        }
    }

}

