using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillInfoCloseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillInfoCloseEventArgs).GetHashCode();

        public SkillInfoCloseEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static SkillInfoCloseEventArgs Create()
        {
            SkillInfoCloseEventArgs skillInfoCloseEventArgs = ReferencePool.Acquire<SkillInfoCloseEventArgs>();
            return skillInfoCloseEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

