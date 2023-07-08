using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class TipUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(TipUIChangeEventArgs).GetHashCode();

        public TipUIChangeEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public Vector3 position { get; set; }
        public string tipTitle { get; set; }
        public int Type { get; set; }


        public static TipUIChangeEventArgs Create(int Type)
        {
            TipUIChangeEventArgs e = ReferencePool.Acquire<TipUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public static TipUIChangeEventArgs Create(Vector3 position, string title, int Type)
        {
            TipUIChangeEventArgs e = ReferencePool.Acquire<TipUIChangeEventArgs>();
            e.position = position;
            e.tipTitle = title;
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

