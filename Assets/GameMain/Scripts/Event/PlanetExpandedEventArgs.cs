using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class PlanetExpandedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlanetExpandedEventArgs).GetHashCode();

        public PlanetExpandedEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static PlanetExpandedEventArgs Create()
        {
            PlanetExpandedEventArgs e = ReferencePool.Acquire<PlanetExpandedEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

