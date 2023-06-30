using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlanetLandingPointEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlanetLandingPointEventArgs).GetHashCode();

        public PlanetLandingPointEventArgs()
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

        public static PlanetLandingPointEventArgs Create()
        {
            PlanetLandingPointEventArgs planetLandingPointEventArgs = ReferencePool.Acquire<PlanetLandingPointEventArgs>();
            return planetLandingPointEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

