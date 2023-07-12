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

        // Respawn Settings
        [HideInInspector] public bool canRespawn = false;
        [HideInInspector] public int respawnCnt = 0;

        // AIAssist Settings
        public GameObject AISpaceshipPrefab;

        private void OnEnable() 
        {
            SetPlayerSkill(); 
            respawnCnt = 0;   
        }

        private void SetPlayerSkill()
        {
            skills.Add(new SkillInfo(EnumSkill.EdgeComputing, KeyCode.Alpha1));   // CloudComputing
            skills.Add(new SkillInfo(EnumSkill.ElectronicWarfare, KeyCode.Alpha2));  // ElectronicWarfare
            skills.Add(new SkillInfo(EnumSkill.MedicalSupport, KeyCode.Alpha3));  // MedicalSupport
            skills.Add(new SkillInfo(EnumSkill.EnergyBoost, KeyCode.Alpha4));  // FireWall
            skills.Add(new SkillInfo(EnumSkill.AdaptiveIntelligentDefense, KeyCode.Alpha5));  // AIAssist

            // TODO : change its value accroding to skill data
            this.canRespawn = true;
            PrintSkillsInfo();
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

        public SkillInfo(EnumSkill skillEnumId, KeyCode keyCode)
        {
            Dictionary<int, int> skills = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills();

            if (skills == null) { return; }

            this.keyCode = keyCode;
            this.skillName = GameEntry.Data.GetData<DataSkill>().GetSkillData((int) skillEnumId).Name;
            this.skillId = (int) skillEnumId;
            this.skillEnumId = skillEnumId;

            if (skills.ContainsKey((int)skillEnumId))
            {
                this.isUnlocked = true;
            }
            else 
            {
                this.isUnlocked = false;
            }
        }
    }
}
