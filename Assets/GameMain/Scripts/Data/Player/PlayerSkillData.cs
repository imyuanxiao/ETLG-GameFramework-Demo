
using System.Collections.Generic;

namespace ETLG.Data
{

  
  public sealed class PlayerSkillData
    {

        public int Id { get; set; }
        public int level { get; set; }
        public bool IsActiveSkill { get; set; }
        public bool IsCombatSkill { get; set; }

        public PlayerSkillData(SkillData skillData)
        {
            this.Id = skillData.Id; 
            this.IsActiveSkill = skillData.IsActiveSkill;
            this.IsActiveSkill = skillData.IsCombatSkill;
        }


    }

}

