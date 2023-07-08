
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

        public string Name
        {
            get
            {
                return dRSkill.NameId;
            }
        }
        public int[] Location
        {
            get
            {
                return dRSkill.Location;
            }
        }

        public string Domain
        {

            get
            {
                switch (dRSkill.Domain)
                {
                    case Constant.Type.DOMAIN_COMMON:
                        return "Common";
                    case Constant.Type.DOMAIN_CLOUD_COMPUTING:
                        return "Cloud Computing";
                    case Constant.Type.DOMAIN_ARTIFICIAL_INTELLIGENCE:
                        return "AI";
                    case Constant.Type.DOMAIN_CYBERSECURITY:
                        return "Cybersecurity";
                    case Constant.Type.DOMAIN_DATA_SCIENCE:
                        return "Data Science";
                    case Constant.Type.DOMAIN_BLOCKCHAIN:
                        return "Blockchain";
                    case Constant.Type.DOMAIN_IoT:
                        return "IoT";
                    default:
                        return "Unknown";
                }
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

        public string Activeness
        {
            get
            {
                return dRSkill.IsActiveSkill ? "Active" : "Passive";
            }
        }

        public string Functionality
        {
            get
            {
                return dRSkill.IsCombatSkill ? "Combat" : "Explore";
            }
        }

        public int ActiveState
        {
            get
            {
                return dRSkill.ActiveState; 
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
            level--;

            if (dRSkillLevels == null || level > GetMaxLevelIndex())
                return null;

            return dRSkillLevels[level];
        }

        public string GetSkillDescription()
        {
            return GameEntry.Localization.GetString(Constant.Key.SKILL_DESC + Name);
        }

        public int[] GetLevelCosts(int level)
        {
            level--;
            return dRSkillLevels[level].Costs;
        }

        public string GetLevelEffectByLevel(int level)
        {
            if(level == 0)
            {
                return "No Effect.";
            }

            DRSkillLevel currentLevelData = GetSkillLevelData(level);

            if (currentLevelData == null)
            {
                return null;
            }

            int[] attrs = currentLevelData.Attributes;

            // no arrtibutes, sepcial desc in Localization
            if (attrs.Length <= 1)
            {
                return GameEntry.Localization.GetString(Constant.Key.SKILL_LEVEL_DESC + level);
            }

            string result = "Add: ";

            for (int i = 0; i < attrs.Length; i += 2)
            {
                result += GetIndexAttrEffect(i, attrs);
                result += i == attrs.Length - 2 ? "." : ", ";
            }

            return result;

        }

        public string GetIndexAttrEffect(int i, int[] attrs)
        {
            string result = "";
            switch (attrs[i])
            {
                case Constant.Type.ATTR_Durability:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Durability";
                case Constant.Type.ATTR_Shields:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Shileds";
                case Constant.Type.ATTR_Firepower:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Firepower";
                case Constant.Type.ATTR_Energy:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Energy";
                case Constant.Type.ATTR_Agility:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Agility";
                case Constant.Type.ATTR_Speed:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Speed";
                case Constant.Type.ATTR_Detection:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Detection";
                case Constant.Type.ATTR_Capacity:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Capacity";
                case Constant.Type.ATTR_Firerate:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Firerate";
                case Constant.Type.ATTR_Dogde:
                    return result += " <color=yellow> " + attrs[i + 1] + " </color> Dogde";
                default:
                    return "Unknown effect";
            }
        }

        // 获得最大等级
        public int GetMaxLevelIndex()
        {
            return dRSkillLevels == null ? 0 : dRSkillLevels.Length - 1;
        }

    }

}
