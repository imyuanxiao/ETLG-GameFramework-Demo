using ETLG.Data;
using UnityGameFramework.Runtime;
using System;
using GameFramework;

namespace ETLG
{

    // 选择界面的飞船预制体上挂载的脚本
    public class EntitySpaceshipSelect : EntityLogicEx
    {

        // protected IFsm<EntitySpaceshipSelect> fsm;

       // private bool hide = false;

        // protected List<FsmState<EntitySpaceshipSelect>> stateList;
        
        public EntityDataSpaceshipSelect EntityDataSpaceshipSelect
        {
            get;
            private set;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            //stateList = new List<FsmState<EntitySpaceshipSelect>>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityDataSpaceshipSelect = userData as EntityDataSpaceshipSelect;

            if (EntityDataSpaceshipSelect == null)
            {
                Log.Error("Entity spaceship '{0}' entity data invaild.", Id);
                return;
            }

            //hide = false;

            // CreateFsm();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

           // hide = true;

            // DestroyFsm();

        }

        // protected virtual void AddFsmState()
        // {
        //     stateList.Add(SpaceshipMoveState.Create());
        //     stateList.Add(SpaceshipAttackHomeBaseState.Create());
        //     stateList.Add(SpaceshipAttackTowerState.Create());
        // }

        // protected virtual void StartFsm()
        // {
        //     fsm.Start<SpaceshipMoveState>();
        // }

        // private void CreateFsm()
        // {
        //     AddFsmState();
        //     fsm = GameEntry.Fsm.CreateFsm<EntitySpaceshipSelect>(gameObject.name, this, stateList);
        //     StartFsm();
        // }

        // private void DestroyFsm()
        // {
        //     GameEntry.Fsm.DestroyFsm(fsm);
        //     foreach (var item in stateList)
        //     {
        //         ReferencePool.Release((IReference)item);
        //     }

        //     stateList.Clear();
        //     fsm = null;
        // }
/*
        protected override void Dead()
        {
            base.Dead();

            if (!hide)
            {
                hide = true;
                GameEntry.Event.Fire(this, HideSpaceshipEventArgs.Create(Id));
            }
        }

        public void Pause()
        {
            IsPause = true;
        }*/

    }
}