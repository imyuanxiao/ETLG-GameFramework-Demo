using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using GameFramework.Event;
using GameFramework.Localization;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System;

namespace ETLG
{
    public class ProcedurePlanet : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;
        private bool changeToProcedureMap = false;
        private RaycastHit hitInfo;
        private NPCUIChangeEventArgs ne=null;

        /*        private int? currentNPCUIID;
                private int? artifactTradeInfoUIID;
        */
        private int? artifactTradeInfoUIID;

        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            this.procedureOwner = procedureOwner;

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.changeToProcedureMap = false;

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(PlanetInfoEventArgs.EventId, OnPlanetInfo);
            GameEntry.Event.Subscribe(PlanetLandingPointEventArgs.EventId, OnPlanetLandingPointClick);
            GameEntry.Event.Subscribe(NPCUIChangeEventArgs.EventId, OnNPCUIChange);
            GameEntry.Event.Subscribe(ArtifactInfoTradeUIChangeEventArgs.EventId, OnArtifactInfoTradeUIChange);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            GameEntry.Event.Subscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Subscribe(ToProcedureMapEventArgs.EventId, OnToProcedureMap);
            GameEntry.Event.Fire(this, PlanetInfoEventArgs.Create(GameEntry.Data.GetData<DataPlanet>().currentPlanetID));

            MapManager.Instance.focusedPlanet.GetComponent<DragRotate>().enabled = true;

            // if player is entering from the explore button at UIMapPlayerInfoForm
            if (MapManager.Instance.currentLandingPointID != -1)
            {
                GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
            }

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
        }

        private void OnToProcedureMap(object sender, GameEventArgs e)
        {
            PlanetBase currentlyFocusedPlanet = MapManager.Instance.focusedPlanet.GetComponent<PlanetBase>();
            GameEntry.Event.Fire(this, UnFocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
            this.changeToProcedureMap = true;
        }

        private void OnEnterBattle(object sender, GameEventArgs e) 
        {
            EnterBattleEventArgs ne = (EnterBattleEventArgs) e;
            if (ne == null)
                return;
            
            procedureOwner.SetData<VarString>("BattleType", ne.BattleType);
            procedureOwner.SetData<VarString>("BossType", ne.BossType);
            procedureOwner.SetData<VarInt32>("Accuracy", ne.Accuracy);
            
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
        }

        private void OnAlertUITrigger(object sender, GameEventArgs e)
        {
            UIAlertTriggerEventArgs ne = (UIAlertTriggerEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm));
            }

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCQuizRewardForm);
            }
        }

        private void OnArtifactInfoTradeUIChange(object sender, GameEventArgs e)
        {
            ArtifactInfoTradeUIChangeEventArgs ne = (ArtifactInfoTradeUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIArtifactInfoTradeForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIArtifactInfoTradeForm));
            }

            if (ne.Type == Constant.Type.UI_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UIArtifactInfoTradeForm);
            }
        }

        private void OnNPCUIChange(object sender, GameEventArgs e)
        {
            this.ne = (NPCUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogForm));
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCTradeForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCTradeForm));
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizForm));
            }

            // Open UI
            if (ne.Type == Constant.Type.NPC_UI_TALK_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
            }
            else if (ne.Type == Constant.Type.NPC_UI_TRADE_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCTradeForm);
            }
            else if (ne.Type == Constant.Type.NPC_UI_QUIZ_OPEN)
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.UINPCQuizForm);
            }
        }

        /*
                    if (currentNPCUIID != null)
                    {
                        GameEntry.UI.CloseUIForm((int)currentNPCUIID);
                        currentNPCUIID = null;
                    }

                    if (ne.Type == Constant.Type.NPC_UI_TALK_OPEN)
                    {
                        currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
                    }
                    else if (ne.Type == Constant.Type.NPC_UI_TRADE_OPEN)
                    {
                        currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCTradeForm);
                    }
                    else if(ne.Type == Constant.Type.NPC_UI_QUIZ_OPEN)
                    {
                        currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCQuizForm);
                    }
                    else if (ne.Type == Constant.Type.UI_CLOSE)
                    {
                        if (currentNPCUIID != null)
                        {
                            GameEntry.UI.CloseUIForm((int)currentNPCUIID);
                        }

                        currentNPCUIID = null;
                    }*/

        private void OnPlanetLandingPointClick(object sender, GameEventArgs e)
        {
            PlanetLandingPointEventArgs ne = (PlanetLandingPointEventArgs)e;
            if (ne == null)
                return;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetLandingPointForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetLandingPointForm));
            }

            GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetLandingPointForm);
        }

        private void OnPlanetInfo(object sender, GameEventArgs e)
        {
            PlanetInfoEventArgs ne = (PlanetInfoEventArgs)e;
            if (ne == null)
                return;

            // 获取事件里的planetID，将 DataPlanet 里的 currentPlanet 设置为该ID的星球
            GameEntry.Data.GetData<DataPlanet>().currentPlanetID = ne.PlanetId;

            GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetInfoForm);
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            MouseControl();
            KeyboardControl();

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }
            if (changeToProcedureMap)
            {
                ChangeState<ProcedureMap>(procedureOwner);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(PlanetInfoEventArgs.EventId, OnPlanetInfo);
            GameEntry.Event.Unsubscribe(PlanetLandingPointEventArgs.EventId, OnPlanetLandingPointClick);
            GameEntry.Event.Unsubscribe(NPCUIChangeEventArgs.EventId, OnNPCUIChange);
            GameEntry.Event.Unsubscribe(ArtifactInfoTradeUIChangeEventArgs.EventId, OnArtifactInfoTradeUIChange);
            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            GameEntry.Event.Unsubscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Unsubscribe(ToProcedureMapEventArgs.EventId, OnToProcedureMap);

            MapManager.Instance.focusedPlanet.GetComponent<PlanetBase>().isFocused = false;
            MapManager.Instance.focusedPlanet.GetComponent<DragRotate>().enabled = false;
            MapManager.Instance.focusedPlanet = null;
            MapManager.Instance.currentLandingPointID = -1;
            MapManager.Instance.isOverUI = false;

            GameEntry.UI.CloseAllLoadedUIForms();

            CloseUI();
            GameEntry.Sound.StopMusic();
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void MouseControl()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // store the returned value into hitInfo
            Physics.Raycast(ray, out hitInfo);

            // if left mouse button is clicked and the object that the ray has hit has collider
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
            {
                // if clicked on a landing point
                if (hitInfo.collider.gameObject.CompareTag("LandingPoint"))
                {
                    LandingPoint currentlyClickedLandingPoint = hitInfo.collider.gameObject.GetComponent<LandingPoint>();
                    GameEntry.Data.GetData<DataLandingPoint>().currentLandingPointID = currentlyClickedLandingPoint.landingPointId;
                    MapManager.Instance.currentLandingPointID = currentlyClickedLandingPoint.landingPointId;
                    GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
                }
            }

            // if right clicked
            if (Input.GetMouseButtonDown(1))
            {
                PlanetBase currentlyFocusedPlanet = MapManager.Instance.focusedPlanet.GetComponent<PlanetBase>();
                GameEntry.Event.Fire(this, UnFocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
                this.changeToProcedureMap = true;
            }
        }

        private void KeyboardControl()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIMapInfoForm))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIMapInfoForm).Close();
                }
                else
                {
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIMapInfoForm);
                }
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveManager.Instance.SaveGame();
            }
        }

        private void CloseUI()
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetInfoForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetInfoForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetLandingPointForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetLandingPointForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIArtifactInfoForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UIArtifactInfoForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCTradeForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UINPCTradeForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCQuizRewardForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UINPCQuizRewardForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UINPCDialogRewardForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UINPCDialogRewardForm).Close();
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIVideoFullScreenForm))
                GameEntry.UI.GetUIForm(EnumUIForm.UIVideoFullScreenForm).Close();
        }
        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                dataAchievement.cuurrentPopUpId = ne.achievementId;
                if (!dataPlayer.GetPlayerData().isAchievementAchieved(ne.count))
                {
                    dataPlayer.GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, dataAchievement.GetNextLevel(ne.achievementId, ne.count));
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIAchievementPopUp);

                }
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (GameEntry.UI.HasUIForm(EnumUIForm.UIAchievementPopUp))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIAchievementPopUp).Close();
                }
            }
        }
    }
}
