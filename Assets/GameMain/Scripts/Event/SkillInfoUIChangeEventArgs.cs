using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillInfoUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillInfoUIChangeEventArgs).GetHashCode();

        public SkillInfoUIChangeEventArgs()
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
        
        public static SkillInfoUIChangeEventArgs Create(int Type)
        {
            SkillInfoUIChangeEventArgs e = ReferencePool.Acquire<SkillInfoUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

