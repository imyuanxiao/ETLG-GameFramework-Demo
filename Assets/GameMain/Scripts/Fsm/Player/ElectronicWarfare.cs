using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class ElectronicWarfare : FsmState<SpaceshipAttack>
    {
        private float lastingTime;
        private float timeElapsed = 0f;
        private bool changeToRespawnState;
        private UIBattleInfo uiBattleInfoForm;

        protected override void OnInit(IFsm<SpaceshipAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<SpaceshipAttack> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            changeToRespawnState = false;
            lastingTime = 5f;
            if (BattleManager.Instance.bossEnemyEntity != null)
            {
                BattleManager.Instance.bossEnemyEntity.GetComponent<BossEnemyAttack>().enabled = false;
            }
            this.uiBattleInfoForm = (UIBattleInfo) GameEntry.UI.GetUIForm(EnumUIForm.UIBattleInfo);
        }

        private void OnPlayerRespawn(object sender, GameEventArgs e)
        {
            PlayerRespawnEventArgs ne = (PlayerRespawnEventArgs) e;
            if (ne == null)
                Log.Error("Invalid event [PlayerRespawnEventArgs]");

            changeToRespawnState = true;
        }

        protected override void OnUpdate(IFsm<SpaceshipAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            UpdateSkillUI(timeElapsed, lastingTime);

            if (changeToRespawnState)
            {
                ChangeState<PlayerRespawn>(fsm);
            }

            if (timeElapsed < lastingTime)
            {
                timeElapsed += elapseSeconds;
            }
            else 
            {
                ChangeState<DefaultSkill>(fsm);
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.Unsubscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            if (BattleManager.Instance.bossEnemyEntity != null)
            {
                BattleManager.Instance.bossEnemyEntity.GetComponent<BossEnemyAttack>().enabled = true;
            }
            lastingTime = 5f;
            timeElapsed = 0f;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private void UpdateSkillUI(float timeElapsed, float lastingTime)
        {
            SkillUI ui = this.uiBattleInfoForm.GetSkillUIById(EnumSkill.ElectronicWarfare);
            ui.skillMaskImage.fillAmount = 1 - timeElapsed / lastingTime;
        }
    }
}

