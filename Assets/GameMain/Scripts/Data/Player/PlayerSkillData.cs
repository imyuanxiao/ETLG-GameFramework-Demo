
using System.Collections.Generic;

namespace ETLG.Data
{

  
  public sealed class PlayerSkillData
    {

        public int Id { get; set; }
        public int Level { get; set; }
        public int ActiveState { get; set; }
        public bool IsActiveSkill { get; set; }
        public bool IsCombatSkill { get; set; }

        public PlayerSkillData(SkillData skillData)
        {
            this.Id = skillData.Id; 
            this.IsActiveSkill = skillData.IsActiveSkill;
            this.IsActiveSkill = skillData.IsCombatSkill;
        }

        public PlayerSkillData(int Id, int ActiveState, int Level)
        {
            this.Id = Id;
            this.ActiveState = ActiveState;
            this.Level = Level;
        }

        // 0 locked 1 unlocked 2 level != 0
        public void setActiveState(int state)
        {
            this.ActiveState = state; 
        }

    }

}

