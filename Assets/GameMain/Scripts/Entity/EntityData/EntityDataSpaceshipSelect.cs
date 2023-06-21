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
            setPosition(entityData);
            return entityData;
        }


        public static void setPosition(EntityDataSpaceshipSelect entityData)
        {
            entityData.Position = new Vector3(2.5f, 0.5f, 1f);
            entityData.Rotation = Quaternion.Euler(15f, -200f, 340f);
        }

        
        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


