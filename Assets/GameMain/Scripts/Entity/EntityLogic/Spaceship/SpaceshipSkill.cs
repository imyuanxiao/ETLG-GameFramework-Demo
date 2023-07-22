using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;

namespace ETLG
{
    // this class is only for handle battle related skills
    public class SpaceshipSkill : MonoBehaviour
    {
        [Header("Skills Info")]
        public List<SkillInfo> skills = new List<SkillInfo>();

        [Header("Skill FX")]
        public GameObject cloudComputingFX;
        public GameObject medicalSupportFX;
        public GameObject fireWallFx;
        public GameObject electronicWarfareFX;

        // Respawn Settings
        [HideInInspector] public bool canRespawn = false;
        [HideInInspector] public int respawnCnt = 0;

        // AIAssist Settings
        public GameObject AISpaceshipPrefab;

        private void OnEnable() 
        {
            DisableAllSkillFX();
            SetPlayerSkill(); 
            respawnCnt = 0;   
        }

        private void DisableAllSkillFX()
        {
            this.cloudComputingFX.SetActive(false);
        }

        private void SetPlayerSkill()
        {
            skills.Add(new SkillInfo(EnumSkill.EdgeComputing, KeyCode.Alpha1));   // CloudComputing
            skills.Add(new SkillInfo(EnumSkill.ElectronicWarfare, KeyCode.Alpha2));  // ElectronicWarfare
            skills.Add(new SkillInfo(EnumSkill.MedicalSupport, KeyCode.Alpha3));  // MedicalSupport
            skills.Add(new SkillInfo(EnumSkill.EnergyBoost, KeyCode.Alpha4));  // FireWall
            skills.Add(new SkillInfo(EnumSkill.AdaptiveIntelligentDefense, KeyCode.Alpha5));  // AIAssist

            // TODO : change its value accroding to skill data
            this.canRespawn = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills().ContainsKey((int) EnumSkill.BlockchainResurgence);
            // PrintSkillsInfo();
        }

        public bool IsSkillReady(EnumSkill id) 
        {
            SkillInfo skillInfo = GetSkillInfoById(id);
            return skillInfo.isUnlocked && skillInfo.usageCount > 0;
        }

        public bool IsSkillLocked(EnumSkill id) 
        {
            return !GetSkillInfoById(id).isUnlocked;
        }

        public SkillInfo GetSkillInfoById(EnumSkill id) 
        {
            foreach (var skillInfo in skills)
            {
                if (skillInfo.skillEnumId == id) 
                {
                    return skillInfo;
                }
            }
            return null;
        }

        public void ReduceUsageCount(EnumSkill id) 
        {
            GetSkillInfoById(id).usageCount--;
        }

        private void PrintSkillsInfo()
        {
            foreach (var skillInfo in this.skills)
            {
                Debug.Log(skillInfo.skillName + " [" + skillInfo.skillId + "]" + " | Is Unlocked ? " + skillInfo.isUnlocked);
            }
        }
        
        private void OnDisable() 
        {
            DisableAllSkillFX();
            skills.Clear();    
        }
    }

    public class SkillInfo
    {
        public string skillName;
        public EnumSkill skillEnumId;
        public int skillId;
        public bool isUnlocked;
        public KeyCode keyCode;
        public int usageCount;

        public SkillInfo(EnumSkill skillEnumId, KeyCode keyCode)
        {
            Dictionary<int, int> skills = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills();

            if (skills == null) { return; }

            this.keyCode = keyCode;
            this.skillName = GameEntry.Data.GetData<DataSkill>().GetSkillData((int) skillEnumId).Name;
            this.skillId = (int) skillEnumId;
            this.skillEnumId = skillEnumId;
            this.usageCount = GameEntry.Data.GetData<DataSkill>().GetSkillData((int) skillEnumId).UsageCount;
            this.isUnlocked = skills.ContainsKey((int) skillEnumId);

            // To be deleted
            // ForTestSkillOnly();
        }

        // To be deleted
        private void ForTestSkillOnly()
        {
            switch (this.skillEnumId)
            {
                case EnumSkill.EdgeComputing:
                    this.isUnlocked = true;
                    break;
                case EnumSkill.ElectronicWarfare:
                    this.isUnlocked = true;
                    break;
                case EnumSkill.MedicalSupport:
                    this.isUnlocked = true;
                    break;
                case EnumSkill.EnergyBoost:
                    this.isUnlocked = true;
                    break;
                case EnumSkill.AdaptiveIntelligentDefense:
                    this.isUnlocked = true;
                    break;
                default:
                    break;
            }
        }
    }
}
