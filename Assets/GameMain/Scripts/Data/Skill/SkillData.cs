
namespace ETLG.Data
{
  public sealed class SkillData
    {
        private DRSkill dRSkill;
        private DRSkillLevel[] dRSkillLevels;

        public int Id
        {
            get
            {
                return dRSkill.Id;
            }
        }
        
        public string NameId
        {
            get
            {
                return dRSkill.NameId;
            }
        }

        public string Icon
        {
            get
            {
                return dRSkill.Icon;
            }
        }

        public string Domain
        {
            get
            {
                return dRSkill.Domain;
            }
        }

        public bool IsActiveSkill
        {
            get
            {
                return dRSkill.IsActiveSkill;
            }
        }

        public bool IsCombatSkill
        {
            get
            {
                return dRSkill.IsCombatSkill;
            }
        }

        public int CurrentLevel
        {
            get
            {
                return dRSkill.Level;
            }
        }

        public int[] Levels
        {
            get
            {
                return dRSkill.Levels;
            }
        }

        public int UnlockPoints
        {
            get
            {
                return dRSkill.UnlockPoints;
            }
        }


        public bool NeedExtraCondition
        {
            get
            {
                return dRSkill.NeedExtraCondition;
            }
        }
        
        // 构造方法
        public SkillData(DRSkill dRSkill, DRSkillLevel[] dRSkillLevels)
        {
            this.dRSkill = dRSkill;
            this.dRSkillLevels = dRSkillLevels;
        }

        // 获取对应等级的技能信息
        public DRSkillLevel GetSkillLevelData(int level)
        {
            if (dRSkillLevels == null || level > GetMaxLevel())
                return null;

            return dRSkillLevels[level];
        }

        // 获得最大等级
        public int GetMaxLevel()
        {
            return dRSkillLevels == null ? 0 : dRSkillLevels.Length - 1;
        }

    }

}

