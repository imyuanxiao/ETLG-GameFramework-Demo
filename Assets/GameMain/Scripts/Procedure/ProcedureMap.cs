﻿using System;
using System.Collections;
using System.Collections.Generic;
using ETLG.Data;
using GameFramework.Event;
using GameFramework.Localization;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETLG
{
    public class ProcedureMap : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        private bool changeScene = false;
        private bool changeToProcedurePlanet = false;

        private RaycastHit hitInfo;  // store the information of the object that the ray hitted

        private DataPlayer dataPlayer;
        private DataAchievement dataAchievement;

        private Queue<AchievementPopUpEventArgs> popupQueue = new Queue<AchievementPopUpEventArgs>();
        private bool isAchievementShowing;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ResetStates();
            // 订阅事件
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);
            GameEntry.Event.Subscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Subscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();

            MapManager.Instance.focusedPlanet = null;
            MapManager.Instance.isOverUI = false;

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.changeToProcedurePlanet = false;

            GameEntry.UI.OpenUIForm(EnumUIForm.UIMapPlayerInfoForm);
            // 播放BGM
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

           // GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(5001, 9999));

            // Auto-Save : will overwirte the first save slot
            SaveManager.Instance.SaveGame();


/*            if (!dataPlayer.GetPlayerData().PlayedTutorial.Contains(1002))
            {
                dataPlayer.GetPlayerData().PlayedTutorial.Add(1002);
                GameEntry.Data.GetData<DataTutorial>().CurrentTutorialID = 1002;
                GameEntry.UI.OpenUIForm(EnumUIForm.UITutorialForm);
            }*/

        }

        private void ResetStates()
        {
        }

        // called when player clicked the challenge button on UIPlanetOverview
        // or player clicked the explore button on UIPlanetOverview and trigger random battle
        private void OnEnterBattle(object sender, GameEventArgs e)
        {
            EnterBattleEventArgs ne = (EnterBattleEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event [EnterBattleEventArgs]");
            
            procedureOwner.SetData<VarString>("BattleType", ne.BattleType);
            procedureOwner.SetData<VarString>("BossType", ne.BossType);
            procedureOwner.SetData<VarInt32>("Accuracy", ne.Accuracy);
            
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
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
            if (changeToProcedurePlanet)
            {
                ChangeState<ProcedurePlanet>(procedureOwner);
            }
        }

        private void MouseControl() {
            if (MapManager.Instance.isOverUI) { return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // store the returned value into hitInfo
            Physics.Raycast(ray, out hitInfo);

            // if left mouse button is clicked and the object that the ray has hit has collider
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null) 
            {
                // if clicked on a planet
                if (hitInfo.collider.gameObject.CompareTag("Planet")) 
                {
                    GameObject planet = hitInfo.collider.gameObject;

                    if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetOverview))
                    {
                        Debug.Log("Has UI Form " + EnumUIForm.UIPlanetOverview);
                        GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetOverview).Close();
                    }
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetOverview, planet.GetComponent<PlanetBase>());
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);
            GameEntry.Event.Unsubscribe(EnterBattleEventArgs.EventId, OnEnterBattle);
            GameEntry.Event.Unsubscribe(AchievementPopUpEventArgs.EventId, OnAchievementPoPUp);
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIMapPlayerInfoForm))
            {
                GameEntry.UI.GetUIForm(EnumUIForm.UIMapPlayerInfoForm).Close();
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetOverview))
            {
                GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetOverview).Close();
            }

            GameEntry.UI.CloseAllLoadedUIForms();


            // 停止音乐
            GameEntry.Sound.StopMusic();
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }



        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            changeScene = true;
            procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, ne.SceneId);
        }

        private void OnFocusOnPlanet(object sender, GameEventArgs e) 
        {
            FocusOnPlanetEventArgs ne = (FocusOnPlanetEventArgs) e;
            if (ne == null)
                Log.Error("Invalid Event [FocusOnPlanetEventArgs]");

            changeToProcedurePlanet = true;
        }
        public void OnAchievementPoPUp(object sender, GameEventArgs e)
        {
            AchievementPopUpEventArgs ne = (AchievementPopUpEventArgs)e;
            if (ne == null)
                return;
            if (ne.Type == Constant.Type.UI_OPEN)
            {
                if (dataPlayer.GetPlayerData().isAchievementShouldAchieved(ne.achievementId,ne.count))
                {
                    popupQueue.Enqueue(ne);
                    dataPlayer.GetPlayerData().UpdatePlayerAchievementData(ne.achievementId, dataAchievement.GetNextLevel(ne.achievementId, ne.count));
                    if (!isAchievementShowing)
                    {
                        isAchievementShowing = true;
                        ne = popupQueue.Dequeue();
                        dataAchievement.cuurrentPopUpId = ne.achievementId;
                        GameEntry.UI.OpenUIForm(EnumUIForm.UIAchievementPopUp);
                    }
                }
            }
            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (popupQueue.Count == 0 && GameEntry.UI.HasUIForm(EnumUIForm.UIAchievementPopUp))
                {
                    GameEntry.UI.GetUIForm(EnumUIForm.UIAchievementPopUp).Close();
                    isAchievementShowing = false;
                }
                if (popupQueue.Count > 0)
                {
                    AchievementPopUpEventArgs popupArgs = popupQueue.Dequeue();
                    dataAchievement.cuurrentPopUpId = popupArgs.achievementId;
                    // 更新UI
                    GameEntry.Event.Fire(this, AchievementMultiplesPopUpEventArgs.Create());
                }

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
    }
}

