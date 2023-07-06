using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AchievementUICloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AchievementUICloseEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }

        public int EventParam { get; private set; }

        public static AchievementUICloseEventArgs Create()
        {
            AchievementUICloseEventArgs e = ReferencePool.Acquire<AchievementUICloseEventArgs>();
            return e;
        }

        public override void Clear()
        {
            EventParam = 0;
        }
    }
}
