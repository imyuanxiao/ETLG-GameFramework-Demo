using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillUpgradeInfoUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillUpgradeInfoUIChangeEventArgs).GetHashCode();

        public SkillUpgradeInfoUIChangeEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }


        public int Type { get; set; }
        
        public static SkillUpgradeInfoUIChangeEventArgs Create(int Type)
        {
            SkillUpgradeInfoUIChangeEventArgs e = ReferencePool.Acquire<SkillUpgradeInfoUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

