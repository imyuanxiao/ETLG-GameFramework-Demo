using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using ETLG.Data;
using System;

namespace ETLG
{
    public class UIBattleInfo : UGuiFormEx
    {
        public TextMeshProUGUI healthValue;
        public TextMeshProUGUI shieldValue;
        public Image currentHealthImage;
        public Image currentShieldImage;
        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI attack;
        public TextMeshProUGUI defense;
        public SkillUI[] skillsUI;
        private Health health;
        private int maxHealth;
        private int currentHealth;
        private int maxShield;
        private int currentShield;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            this.health = (Health) userData;
            if (this.health == null)
            {
                Log.Error("Not valid health data");
            }

            nameLabel.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().initialSpaceship.NameId;
            attack.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower.ToString();
            defense.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Shields.ToString();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            attack.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower.ToString();
            defense.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Shields.ToString();

            healthValue.text = health.CurrentHealth + " / " + health.MaxHealth;
            shieldValue.text = health.CurrentShield + " / " + health.MaxShield;

            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;
            currentHealthImage.fillAmount = (float) currentHealth / maxHealth;

            maxShield = health.MaxShield;
            currentShield = health.CurrentShield;
            currentShieldImage.fillAmount = (float) currentShield / maxShield;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public SkillUI GetSkillUIById(EnumSkill skillId)
        {
            foreach (var skillUI in this.skillsUI)
            {
                if (skillUI.skillId == skillId)
                {
                    return skillUI;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class SkillUI 
    {
        public EnumSkill skillId;
        public TextMeshProUGUI usageCount;
        public RawImage skillImage;
        public RawImage lockImage;
        public Image skillMaskImage;
    }
}
