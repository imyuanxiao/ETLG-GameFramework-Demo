
using Unity.VisualScripting;

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
                return GameEntry.Localization.GetString(Constant.Key.PRE_SKILL + this.NameID);
            }
        }

        public string Description
        {
            get
            {
                return GameEntry.Localization.GetString(Constant.Key.PRE_SKILL + this.NameID + Constant.Key.POST_DESC);
            }
        }


        public string NameID
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
                return GameEntry.Localization.GetString(Constant.Key.PRE_DOMAIN + dRSkill.Domain);
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
                return dRSkill.IsActiveSkill ? Constant.Type.SKILL_TYPE_ACTIVE_STR : Constant.Type.SKILL_TYPE_PASSIVE_STR;
            }
        }

        public string Functionality
        {
            get
            {
                return dRSkill.IsCombatSkill ? Constant.Type.SKILL_TYPE_COMBAT_STR : Constant.Type.SKILL_TYPE_EXPLORE_STR;
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

        public int UsageCount
        {
            get
            {
                return InitUsageCount(Id);
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
            int index = level - 1;
            if (dRSkillLevels == null || index < 0 || index > GetMaxLevelIndex())
                return null;
            return dRSkillLevels[index];
        }

 
        public int[] GetLevelCosts(int level)
        {
            int index = level - 1;
            return dRSkillLevels[index].Costs;
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
                return GameEntry.Localization.GetString(Constant.Key.PRE_SKILL + this.NameID + Constant.Key.POST_SKILL_LEVEL + level);
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


        public int[] GetAllLevelsCosts(int level)
        {
            int[] result = null;
            for(int i = level; i >= 1; i--)
            {
                int[] costs = GetSkillLevelData(i).Costs;
                if(result == null)
                {
                    result = (int[])costs.Clone();
                }
                else
                {
                    for(int j = 1; j < costs.Length; j += 2)
                    {
                        result[j] += costs[j];
                    }
                }
            }
            return result;

        }

        private int InitUsageCount(int id) 
        {
            switch (id)
            {
                case (int) EnumSkill.EdgeComputing:
                    return 2;
                case (int) EnumSkill.ElectronicWarfare:
                    return 1;
                case (int) EnumSkill.MedicalSupport:
                    return 3;
                case (int) EnumSkill.EnergyBoost:
                    return 3;
                case (int) EnumSkill.AdaptiveIntelligentDefense:
                    return 1;
                default:
                    return 0;
            }
        }

    }

}
