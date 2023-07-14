using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class AchievementPopUpEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AchievementPopUpEventArgs).GetHashCode();

        public override int Id { get { return EventId; } }
        public int achievementId { get; private set; }
        public int count { get; private set; }
        public int Type { get; private set; }
        public int EventParam { get; private set; }

        public static AchievementPopUpEventArgs Create(int Id,int count)
        {
            AchievementPopUpEventArgs e = ReferencePool.Acquire<AchievementPopUpEventArgs>();
            e.achievementId = Id;
            e.Type = Constant.Type.UI_OPEN;
            e.count = count;
            return e;
        }
        public static AchievementPopUpEventArgs Create(int Type)
        {
            AchievementPopUpEventArgs e = ReferencePool.Acquire<AchievementPopUpEventArgs>();
            e.Type = Type;
            return e;
        }
        public override void Clear()
        {
            EventParam = 0;
        }
    }
}
