using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AchievementUIOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AchievementUIOpenEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }

        public int EventParam { get; private set; }

        public static AchievementUIOpenEventArgs Create(int eventParam)
        {
            AchievementUIOpenEventArgs e = ReferencePool.Acquire<AchievementUIOpenEventArgs>();
            e.EventParam = eventParam;
            return e;
        }

        public override void Clear()
        {
            EventParam = 0;
        }
    }
}
