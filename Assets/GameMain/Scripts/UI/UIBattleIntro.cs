using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using System;

namespace ETLG
{
    public class UIBattleIntro : UGuiFormEx
    {
        public Button continueButton;
        public Button skipButton;
        public TextMeshProUGUI introInfo;

        private string battleType;
        private string bossType;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            continueButton.onClick.AddListener(OnContinueBtnClicked);
            skipButton.onClick.AddListener(OnSkipButtonClicked);
        }

        private void OnContinueBtnClicked()
        {
            // continue to ProcedureBasicBattle or ProcedureIntermidateBattle or ProcedureFinalBattle
            GameEntry.Event.Fire(this, EnterBattleEventArgs.Create(battleType, bossType));
        }

        private void OnSkipButtonClicked()
        {   
            // return map scene and ProcedureMap
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Dictionary<string, string> battleData = (Dictionary<string, string>) userData;

            this.battleType = battleData["BattleType"];
            this.bossType = battleData["BossType"];

            SetBattleIntroInfo();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void SetBattleIntroInfo()
        {
            if (battleType == "IntermidateBattle")
            {
                introInfo.text = GameEntry.Localization.GetString(battleType + " " + bossType + " Intro");
            }
            else
            {
                introInfo.text = GameEntry.Localization.GetString(battleType + " Intro");
            }
            
        }
    }
}
