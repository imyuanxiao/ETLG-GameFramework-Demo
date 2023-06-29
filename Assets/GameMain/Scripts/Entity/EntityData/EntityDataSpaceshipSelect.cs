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
            entityData.Position = new Vector3(4f, 1f, 15f);
            entityData.Rotation = Quaternion.Euler(20f, 160f, -10f);
        }

        
        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


