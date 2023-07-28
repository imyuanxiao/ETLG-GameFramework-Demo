using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class UIAlertTriggerEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UIAlertTriggerEventArgs).GetHashCode();

        public UIAlertTriggerEventArgs()
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

        public static UIAlertTriggerEventArgs Create(int Type)
        {
            UIAlertTriggerEventArgs e = ReferencePool.Acquire<UIAlertTriggerEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

