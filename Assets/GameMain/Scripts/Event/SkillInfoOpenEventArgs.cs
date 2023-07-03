using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillInfoOpenEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillInfoOpenEventArgs).GetHashCode();

        public SkillInfoOpenEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static SkillInfoOpenEventArgs Create()
        {
            SkillInfoOpenEventArgs skillInfoOpenEventArgs = ReferencePool.Acquire<SkillInfoOpenEventArgs>();
            return skillInfoOpenEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

