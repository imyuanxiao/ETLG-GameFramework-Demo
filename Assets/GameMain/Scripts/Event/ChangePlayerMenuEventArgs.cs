using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ChangePlayerMenuEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangePlayerMenuEventArgs).GetHashCode();



        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int UIFormID
        {
            get;
            private set;
        }

        public static ChangePlayerMenuEventArgs Create(int UIFormID)
        {
            ChangePlayerMenuEventArgs e = ReferencePool.Acquire<ChangePlayerMenuEventArgs>();
            e.UIFormID = UIFormID;
            return e;
        }
        public override void Clear()
        {
        }
    }

}

