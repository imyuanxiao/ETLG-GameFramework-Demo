using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ModuleInfoUIChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ModuleInfoUIChangeEventArgs).GetHashCode();

        public ModuleInfoUIChangeEventArgs()
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

        public static ModuleInfoUIChangeEventArgs Create(int Type)
        {
            ModuleInfoUIChangeEventArgs e = ReferencePool.Acquire<ModuleInfoUIChangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

