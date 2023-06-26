using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ActiveBattleComponentEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ActiveBattleComponentEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public ActiveBattleComponentEventArgs()
        {

        }

        public static ActiveBattleComponentEventArgs Create()
        {
            ActiveBattleComponentEventArgs e = ReferencePool.Acquire<ActiveBattleComponentEventArgs>();
            return e;
        }

        public override void Clear()
        {
           
        }
    }
}
