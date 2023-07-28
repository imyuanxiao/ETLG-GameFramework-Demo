using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;


namespace ETLG
{
    public class ToProcedureMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ToProcedureMapEventArgs).GetHashCode();

        public override int Id 
        {
            get
            {
                return EventId;
            }
        }

        public static ToProcedureMapEventArgs Create()
        {
            ToProcedureMapEventArgs e = ReferencePool.Acquire<ToProcedureMapEventArgs>();
            return e;
        }

        public override void Clear()
        {
            
        }
    }
}