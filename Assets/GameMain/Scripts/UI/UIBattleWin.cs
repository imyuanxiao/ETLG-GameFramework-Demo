using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using ETLG.Data;

namespace ETLG
{
    public class UIBattleWin : UGuiFormEx
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI info1 = null;
        [SerializeField] private TextMeshProUGUI info2 = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            continueButton.onClick.AddListener(OnContinueBtnClick);
        }

        private void OnContinueBtnClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            info1.text = "Reward";
            info2.text = "Reward";
            // PlayerData data = (PlayerData) userData;
            
            // Debug.Log("UIBattleWin - OnOpen : " + data.calculatedSpaceship.Description);
        }
    }
}
