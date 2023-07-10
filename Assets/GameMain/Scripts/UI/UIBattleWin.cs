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

            if (GameEntry.Procedure.CurrentProcedure is ProcedureBasicBattle)
            {
                Dictionary<string, int> data = (Dictionary<string, int>) userData;
                info1.text = "Enemy Killed: " + data["Killed"] + " | Enemy Passed: " + data["Passed"];
                info2.text = "You got " + data["Killed"] * 100 + " coins!";
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureIntermidateBattle)
            {
                info1.text = "You defeated " + BattleManager.Instance.bossType;
                info2.text = "You got a knowledge fraction!";
            }
            else if (GameEntry.Procedure.CurrentProcedure is ProcedureFinalBattle)
            {
                info1.text = "You defeated final Boss !!!";
                info2.text = "You can now restore your planet !!!";
            }
            else
            {
                info1.text = "Reward";
                info2.text = "Reward";
            }
        }
    }
}
