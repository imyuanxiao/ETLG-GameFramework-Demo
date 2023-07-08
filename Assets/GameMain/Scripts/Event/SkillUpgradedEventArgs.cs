using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillUpgradedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillUpgradedEventArgs).GetHashCode();

        public SkillUpgradedEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static SkillUpgradedEventArgs Create()
        {
            SkillUpgradedEventArgs e = ReferencePool.Acquire<SkillUpgradedEventArgs>();
            return e;
        }

        public override void Clear()
        {
        }
    }

}

