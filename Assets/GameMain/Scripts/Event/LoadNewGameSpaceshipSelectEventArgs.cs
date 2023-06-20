using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class LoadNewGameSpaceshipSelectEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadNewGameSpaceshipSelectEventArgs).GetHashCode();

        public LoadNewGameSpaceshipSelectEventArgs()
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

        public static LoadNewGameSpaceshipSelectEventArgs Create(int sceneId, object userData = null)
        {
            LoadNewGameSpaceshipSelectEventArgs loadNewGameSpaceshipSelectEventArgs = ReferencePool.Acquire<LoadNewGameSpaceshipSelectEventArgs>();
            loadNewGameSpaceshipSelectEventArgs.SceneId = sceneId;
            loadNewGameSpaceshipSelectEventArgs.UserData = userData;
            return loadNewGameSpaceshipSelectEventArgs;
        }

        public override void Clear()
        {
            SceneId = 0;
        }
    }

}

