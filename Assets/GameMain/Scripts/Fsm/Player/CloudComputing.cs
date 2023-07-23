using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using ETLG.Data;
using GameFramework.Event;
using System;
using UnityGameFramework.Runtime;

namespace ETLG
{
    // increase the attack power of the spaceship
    public class CloudComputing : FsmState<SpaceshipAttack>
    {
        private int originalAttack;
        private int skillAttack;
        private float boostScale;
        private float lastingTime;
        private float timeElapsed = 0f;
        private bool changeToRespawnState = false;
        private UIBattleInfo uiBattleInfoForm = null;

        public CloudComputing()
        {

        }

        protected override void OnInit(IFsm<SpaceshipAttack> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<SpaceshipAttack> fsm)
        {
            base.OnEnter(fsm);

            GameEntry.Event.Subscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            changeToRespawnState = false;
            
            boostScale = 2f;
            originalAttack = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower;
            skillAttack = (int) (originalAttack * boostScale);
            lastingTime = 5f;
            // GameEntry.Data.GetData<DataPlayer>().GetPlayerData().getSkillsByType("combat");

            this.uiBattleInfoForm = (UIBattleInfo) GameEntry.UI.GetUIForm(EnumUIForm.UIBattleInfo);

            GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceshipSkill>().cloudComputingFX.SetActive(true);
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
            if (changeToRespawnState)
            {
                ChangeState<PlayerRespawn>(fsm);
                return;
            }

            if (timeElapsed < lastingTime)
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = (float) skillAttack;
                timeElapsed += elapseSeconds;
                UpdateSkillUI(timeElapsed, lastingTime);
            }
            else 
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = originalAttack;
                Debug.Log("Skill Finished");
                ChangeState<DefaultSkill>(fsm);
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.Unsubscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            lastingTime = 5f;
            timeElapsed = 0f;
            GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = originalAttack;
            this.uiBattleInfoForm = null;

            GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceshipSkill>().cloudComputingFX.SetActive(false);
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private void UpdateSkillUI(float timeElapsed, float lastingTime)
        {
            SkillUI ui = this.uiBattleInfoForm.GetSkillUIById(EnumSkill.EdgeComputing);
            ui.skillMaskImage.fillAmount = 1 - timeElapsed / lastingTime;
        }
    }
}
