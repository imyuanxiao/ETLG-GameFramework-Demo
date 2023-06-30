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

        public static EntityDataSpaceshipSelect Create(SpaceshipData spaceshipData, bool checkScene)
        {
            EntityDataSpaceshipSelect entityData = ReferencePool.Acquire<EntityDataSpaceshipSelect>();
            entityData.SpaceshipData = spaceshipData;
            setPosition(entityData, checkScene);
            return entityData;
        }

        public static void setPosition(EntityDataSpaceshipSelect entityData, bool checkScene)
        {
            if (!checkScene)
            {
                entityData.Position = new Vector3(4f, 1f, 15f);
                entityData.Rotation = Quaternion.Euler(20f, 160f, -10f);
            }
            else
            {
                entityData.Position = new Vector3(0.5f, 0f, 15f);
                entityData.Rotation = Quaternion.Euler(18f, 140f, 0f);
            }

        }

        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


