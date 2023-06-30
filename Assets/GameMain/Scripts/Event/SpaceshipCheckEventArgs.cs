using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SpaceshipCheckEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SpaceshipCheckEventArgs).GetHashCode();

        public SpaceshipCheckEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SceneId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static SpaceshipCheckEventArgs Create()
        {
            SpaceshipCheckEventArgs spaceshipCheckEventArgs = ReferencePool.Acquire<SpaceshipCheckEventArgs>();
            return spaceshipCheckEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

