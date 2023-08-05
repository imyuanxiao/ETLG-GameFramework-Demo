using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class BackendFetchedEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BackendFetchedEventArgs).GetHashCode();

        public BackendFetchedEventArgs()
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

        public static BackendFetchedEventArgs Create(int Type)
        {
            BackendFetchedEventArgs e = ReferencePool.Acquire<BackendFetchedEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}
