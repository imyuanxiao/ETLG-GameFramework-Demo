using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillUpgradedLackCostEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillUpgradedLackCostEventArgs).GetHashCode();

        public SkillUpgradedLackCostEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static SkillUpgradedLackCostEventArgs Create()
        {
            SkillUpgradedLackCostEventArgs e = ReferencePool.Acquire<SkillUpgradedLackCostEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

