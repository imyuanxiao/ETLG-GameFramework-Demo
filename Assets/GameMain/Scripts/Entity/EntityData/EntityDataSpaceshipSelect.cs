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
                entityData.Position = new Vector3(4.5f, 0f, 15f);
                entityData.Rotation = Quaternion.Euler(5, 230f, 0f);
            }
            else
            {
                entityData.Position = new Vector3(-15f, 5f, 0f);
                entityData.Rotation = Quaternion.Euler(0f, -310f, 180f);
/*                entityData.Position = new Vector3(0.5f, 0f, 15f);
                entityData.Rotation = Quaternion.Euler(0f, 140f, 0f);*/
            }

        }

        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


