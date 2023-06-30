using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using Unity.VisualScripting;

namespace ETLG
{
    public class PlanetInfoEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlanetInfoEventArgs).GetHashCode();

        public PlanetInfoEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int PlanetId
        {
            get;
            private set;
        }


        public static PlanetInfoEventArgs Create(int PlanetId)
        {
            PlanetInfoEventArgs planetInfoEventArgs = ReferencePool.Acquire<PlanetInfoEventArgs>();
            planetInfoEventArgs.PlanetId = PlanetId;
            return planetInfoEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

