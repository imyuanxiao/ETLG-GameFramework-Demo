using System;
using UnityEngine;
using GameFramework;
using ETLG.Data;

namespace ETLG
{
    [Serializable]

    public class EntityDataSpaceship : EntityData
    {
        public SpaceshipData SpaceshipData
        {
            get;
            private set;
        }

        // 根据传入的玩家信息，获取玩家当前的飞船，新建飞船模型实体
        public static EntityDataSpaceship Create(PlayerData playerData, object userData = null)
        {
            EntityDataSpaceship entityData = ReferencePool.Acquire<EntityDataSpaceship>();
            setPosition(entityData);
            return entityData;
        }

        // 设置飞船的位置、角度等内容，待修改
        public static void setPosition(EntityDataSpaceship entityData)
        {
            entityData.Position = new Vector3(0f, 0f, 0f);
            entityData.Rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        
        public override void Clear()
        {
            base.Clear();
            SpaceshipData = null;
        }
    }
}


