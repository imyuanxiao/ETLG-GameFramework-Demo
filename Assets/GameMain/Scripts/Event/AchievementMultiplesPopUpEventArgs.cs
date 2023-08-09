using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AchievementMultiplesPopUpEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AchievementMultiplesPopUpEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }
        public int EventParam { get; private set; }

        public static AchievementMultiplesPopUpEventArgs Create()
        {
            AchievementMultiplesPopUpEventArgs e = ReferencePool.Acquire<AchievementMultiplesPopUpEventArgs>();
            return e;
        }
        public override void Clear()
        {
            EventParam = 0;
        }
    }
}
