using System.Collections;
using System.Collections.Generic;
using ETLG.Data;
using GameFramework.Event;
using GameFramework.Localization;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETLG
{
    public class ProcedureBattle : ProcedureBase
    {
        private EntityLoader entityLoader;
        private Entity spaceShipEntity;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            entityLoader = EntityLoader.Create(this);

            SpaceshipData spaceshipData = GameEntry.Data.GetData<DataSpaceship>().GetSpaceshipData((int)EnumEntity.InterstellarExplorer);
            
            entityLoader.ShowEntity<EntitySpaceshipSelect>(EnumEntity.InterstellarExplorer, onShowSuccess, EntityDataSpaceshipSelect.Create(spaceshipData));
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            entityLoader.HideEntity(spaceShipEntity);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void onShowSuccess(Entity entity)
        {
            spaceShipEntity = entity;
            Debug.Log("Entity Id: " + entity.Id);
        }
    }
}

