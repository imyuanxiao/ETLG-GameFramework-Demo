using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class SkillTreeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillTreeEventArgs).GetHashCode();

        public SkillTreeEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int SceneId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public static SkillTreeEventArgs Create()
        {
            SkillTreeEventArgs skillTreeEventArgs = ReferencePool.Acquire<SkillTreeEventArgs>();
            return skillTreeEventArgs;
        }

        public override void Clear()
        {
        }
    }

}

