using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SpaceshipChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SpaceshipChangeEventArgs).GetHashCode();

        public SpaceshipChangeEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static SpaceshipChangeEventArgs Create()
        {
            SpaceshipChangeEventArgs ne = ReferencePool.Acquire<SpaceshipChangeEventArgs>();
            return ne;
        }

        public override void Clear()
        {
        }
    }

}

