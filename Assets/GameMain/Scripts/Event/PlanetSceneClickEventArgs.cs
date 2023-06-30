using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlanetSceneClickEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlanetSceneClickEventArgs).GetHashCode();

        public PlanetSceneClickEventArgs()
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

        public static PlanetSceneClickEventArgs Create()
        {
            PlanetSceneClickEventArgs planetSceneClickEventArgs = ReferencePool.Acquire<PlanetSceneClickEventArgs>();
            return planetSceneClickEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

