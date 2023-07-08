using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;

namespace ETLG
{
    public class ModuleEquipUIchangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SkillUpgradedEventArgs).GetHashCode();

        public ModuleEquipUIchangeEventArgs()
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

        public static ModuleEquipUIchangeEventArgs Create(int Type)
        {
            ModuleEquipUIchangeEventArgs e = ReferencePool.Acquire<ModuleEquipUIchangeEventArgs>();
            e.Type = Type;
            return e;
        }

        public override void Clear()
        {
        }
    }

}

