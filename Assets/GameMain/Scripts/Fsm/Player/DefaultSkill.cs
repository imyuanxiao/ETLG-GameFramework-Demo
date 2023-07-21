using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace ETLG
{
    public class DefaultSkill : FsmState<SpaceshipAttack>
    {
        private bool changeToRespawnState = false;
        private UIBattleInfo uiBattleInfoForm = null;
        private SpaceshipSkill spaceshipSkill = null;

        public DefaultSkill(SpaceshipSkill spaceshipSkill)
        {
            this.spaceshipSkill = spaceshipSkill;
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
            SetUIBattleInfo();
            if (changeToRespawnState)
            {
                ChangeState<PlayerRespawn>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && spaceshipSkill.IsSkillReady(EnumSkill.EdgeComputing))
            {
                this.spaceshipSkill.ReduceUsageCount(EnumSkill.EdgeComputing);
                ChangeState<CloudComputing>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && spaceshipSkill.IsSkillReady(EnumSkill.ElectronicWarfare))
            {
                this.spaceshipSkill.ReduceUsageCount(EnumSkill.ElectronicWarfare);
                ChangeState<ElectronicWarfare>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && spaceshipSkill.IsSkillReady(EnumSkill.MedicalSupport))
            {
                this.spaceshipSkill.ReduceUsageCount(EnumSkill.MedicalSupport);
                ChangeState<Medicalsupport>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && spaceshipSkill.IsSkillReady(EnumSkill.EnergyBoost))
            {
                this.spaceshipSkill.ReduceUsageCount(EnumSkill.EnergyBoost);
                ChangeState<FireWall>(fsm);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && spaceshipSkill.IsSkillReady(EnumSkill.AdaptiveIntelligentDefense))
            {
                this.spaceshipSkill.ReduceUsageCount(EnumSkill.AdaptiveIntelligentDefense);
                ChangeState<PlayerAIAssist>(fsm);
            }
        }

        protected override void OnLeave(IFsm<SpaceshipAttack> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(PlayerRespawnEventArgs.EventId, OnPlayerRespawn);

            foreach (var skillUI in this.uiBattleInfoForm.skillsUI)
            {
                if (!spaceshipSkill.IsSkillLocked(skillUI.skillId))
                    skillUI.skillMaskImage.fillAmount = 1f;
            }

            this.uiBattleInfoForm = null;
        }

        protected override void OnDestroy(IFsm<SpaceshipAttack> fsm)
        {
            base.OnDestroy(fsm);
        }

        private void SetUIBattleInfo()
        {
            if (this.uiBattleInfoForm != null) { return; }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIBattleInfo))
            {
                this.uiBattleInfoForm = (UIBattleInfo) GameEntry.UI.GetUIForm(EnumUIForm.UIBattleInfo);
                
                foreach (var skillUI in this.uiBattleInfoForm.skillsUI)
                {
                    if (spaceshipSkill.IsSkillReady(skillUI.skillId))
                    {
                        skillUI.skillMaskImage.fillAmount = 0f;
                        skillUI.skillImage.transform.parent.gameObject.SetActive(true);
                        skillUI.lockImage.gameObject.SetActive(false);
                    }
                    else if (spaceshipSkill.IsSkillLocked(skillUI.skillId))
                    {
                        skillUI.skillMaskImage.fillAmount = 0f;
                        skillUI.skillImage.transform.parent.gameObject.SetActive(false);
                        skillUI.lockImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        skillUI.skillMaskImage.fillAmount = 1f;
                    }
                    skillUI.usageCount.text = spaceshipSkill.GetSkillInfoById(skillUI.skillId).usageCount.ToString();
                }
            }            
        }
    }
}
