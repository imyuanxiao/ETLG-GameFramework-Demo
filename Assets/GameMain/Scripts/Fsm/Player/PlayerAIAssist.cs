using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ETLG.Data;
using System;

namespace ETLG
{
    public class PlayerAIAssist : FsmState<SpaceshipAttack>
    {
        private bool changeToRespawnState;
        private float lastingTime;
        private float timeElapsed;
        private SpaceshipSkill skill;

        private UIBattleInfo uiBattleInfoForm;

        public PlayerAIAssist(SpaceshipSkill skill)
        {
            this.skill = skill;
        }

        protected override void OnInit(IFsm<SpaceshipAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<SpaceshipAttack> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            this.changeToRespawnState = false;
            this.lastingTime = 10f;
            this.timeElapsed = 0f;
            
            Transform[] spawnPoints = BattleManager.Instance.PlayerAIShipSpawnPoints;

            for (int i=0; i < spawnPoints.Length; i++)
            {
                GameObject obj = ObjectPoolManager.SpawnObject(skill.AISpaceshipPrefab, spawnPoints[i].position,
                    skill.gameObject.transform.rotation, ObjectPoolManager.PoolType.GameObject);
                obj.GetComponent<PlayerAIShip>().lastingTime = lastingTime;
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
                return;
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
            timeElapsed = 0f;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private void UpdateSkillUI(float timeElapsed, float lastingTime)
        {
            SkillUI ui = this.uiBattleInfoForm.GetSkillUIById(EnumSkill.AdaptiveIntelligentDefense);
            ui.skillMaskImage.fillAmount = 1 - timeElapsed / lastingTime;
        }
    }
}
