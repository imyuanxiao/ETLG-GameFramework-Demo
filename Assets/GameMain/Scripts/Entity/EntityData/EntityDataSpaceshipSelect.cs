using System;
using UnityEngine;
using GameFramework;
using ETLG.Data;

namespace ETLG
{
    [Serializable]
    public class EntityDataSpaceshipSelect : EntityData
    {
        public SpaceshipData SpaceshipData
        {
            get;
            private set;
        }

        public EntityDataSpaceshipSelect() : base()
        {
            SpaceshipData = null;
        }

        public static EntityDataSpaceshipSelect Create(SpaceshipData spaceshipData, object userData = null)
        {
            EntityDataSpaceshipSelect entityData = ReferencePool.Acquire<EntityDataSpaceshipSelect>();
            entityData.SpaceshipData = spaceshipData;
            return entityData;
        }
        
        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


