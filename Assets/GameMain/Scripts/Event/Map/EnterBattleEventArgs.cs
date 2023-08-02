using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class EnterBattleEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnterBattleEventArgs).GetHashCode();
        public override int Id { get { return EventId; } }

        public string BattleType { get; private set; }

        public string BossType { get; private set; }

        public int Accuracy { get; private set; }

        public static EnterBattleEventArgs Create(string BattleType, string BossType = null, int Accuracy = 0)
        {
            EnterBattleEventArgs e = ReferencePool.Acquire<EnterBattleEventArgs>();
            e.BattleType = BattleType;
            e.BossType = BossType;
            e.Accuracy = Accuracy;
            return e;
        }

        public override void Clear()
        {
            BattleType = null;;
            BossType = null;
        }
    }
}
