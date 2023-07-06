using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIHealthBar : UGuiFormEx
    {
        public Image currentHealthImage;
        public Image currentShieldImage;
        public TextMeshProUGUI healthValue;
        public TextMeshProUGUI shieldValue;
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
            maxShield = 1;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;

            maxShield = health.MaxShield;
            currentShield = health.CurrentShield;

            currentHealthImage.fillAmount = (float) currentHealth / maxHealth;
            currentShieldImage.fillAmount = (float) currentShield / maxShield;

            healthValue.text = health.CurrentHealth + " / " + health.MaxHealth;
            shieldValue.text = health.CurrentShield + " / " + health.MaxShield;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
