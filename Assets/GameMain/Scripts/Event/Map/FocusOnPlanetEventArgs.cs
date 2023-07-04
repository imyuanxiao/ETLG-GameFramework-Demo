using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class FocusOnPlanetEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(FocusOnPlanetEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public PlanetBase PlanetBase { get; private set; }

        public static FocusOnPlanetEventArgs Create(PlanetBase planetBase) 
        {
            FocusOnPlanetEventArgs e = ReferencePool.Acquire<FocusOnPlanetEventArgs>();
            e.PlanetBase = planetBase;
            return e;
        }

        public override void Clear()
        {
            PlanetBase = null;
        }
    }
}
