using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIHealthBar : UGuiFormEx
    {
        public Image currentHealthImage;
        private Health health;
        private int maxHealth;
        private int currentHealth;

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
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;
            currentHealthImage.fillAmount = (float) currentHealth / maxHealth;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
