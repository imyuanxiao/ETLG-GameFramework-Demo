using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AchievementInfoUIEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AchievementInfoUIEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }

        public int EventParam { get; private set; }

        public static AchievementInfoUIEventArgs Create()
        {
            AchievementInfoUIEventArgs e = ReferencePool.Acquire<AchievementInfoUIEventArgs>();
            
            return e;
        }

        public override void Clear()
        {
            EventParam = 0;
        }
    }
}
