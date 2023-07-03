using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using ETLG.Data;

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
            boostScale = 5f;
            originalAttack = (int) GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower;
            skillAttack = (int) (originalAttack * boostScale);
            lastingTime = 5f;
            // GameEntry.Data.GetData<DataPlayer>().GetPlayerData().getSkillsByType("combat");
        }

        protected override void OnUpdate(IFsm<SpaceshipAttack> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (timeElapsed < lastingTime)
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = (float) skillAttack;
                timeElapsed += elapseSeconds;
            }
            else 
            {
                // ChangeState<>
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = originalAttack;
                Debug.Log("Skill Finished");
                ChangeState<DefaultSkill>(fsm);
                
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            lastingTime = 5f;
            timeElapsed = 0f;
            GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower = originalAttack;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }
    }
}
