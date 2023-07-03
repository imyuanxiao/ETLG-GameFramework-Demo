
using System.Collections.Generic;

namespace ETLG.Data
{

  
  public sealed class PlayerSkillData
    {

        public int Id { get; set; }
        public int level { get; set; }

        public bool unLocked { get; set; }
        public bool IsActiveSkill { get; set; }
        public bool IsCombatSkill { get; set; }

        // 玩家技能信息（技能ID，是否解锁，当前等级）
        public PlayerSkillData(SkillData skillData)
        {
            this.Id = skillData.Id; 
            this.unLocked = true;
            this.IsActiveSkill = skillData.IsActiveSkill;
            this.IsActiveSkill = skillData.IsCombatSkill;
        }


    }

}

