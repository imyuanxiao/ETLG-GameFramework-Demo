using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class UnFocusOnPlanetEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UnFocusOnPlanetEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public PlanetBase PlanetBase { get; private set; }

        public static UnFocusOnPlanetEventArgs Create(PlanetBase planetBase) 
        {
            UnFocusOnPlanetEventArgs e = ReferencePool.Acquire<UnFocusOnPlanetEventArgs>();
            e.PlanetBase = planetBase;
            return e;
        }

        public override void Clear()
        {
            PlanetBase = null;
        }
    }
}
