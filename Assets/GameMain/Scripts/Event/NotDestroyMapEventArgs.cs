using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class NotDestroyMapEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(NotDestroyMapEventArgs).GetHashCode();

        public NotDestroyMapEventArgs()
        {
            SceneId = 0;
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

        public static NotDestroyMapEventArgs Create(int sceneId, object userData = null)
        {
            NotDestroyMapEventArgs notDestroyMapEventArgs = ReferencePool.Acquire<NotDestroyMapEventArgs>();
            notDestroyMapEventArgs.SceneId = sceneId;
            notDestroyMapEventArgs.UserData = userData;
            return notDestroyMapEventArgs;
        }

        public override void Clear()
        {
            SceneId = 0;
        }
    }

}

