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

        private int? currentNPCUIID;
        private int? artifactInfoUIID;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.changeToProcedureMap = false;
            this.currentNPCUIID = null;
            this.artifactInfoUIID = null;

            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(PlanetInfoEventArgs.EventId, OnPlanetInfo);
            GameEntry.Event.Subscribe(PlanetLandingPointEventArgs.EventId, OnPlanetLandingPointClick);
            GameEntry.Event.Subscribe(NPCUIChangeEventArgs.EventId, OnNPCUIChange);
            GameEntry.Event.Subscribe(ArtifactInfoUIChangeEventArgs.EventId, OnArtifactInfoUIChange);

            GameEntry.Event.Fire(this, PlanetInfoEventArgs.Create(GameEntry.Data.GetData<DataPlanet>().currentPlanetID));

            MapManager.Instance.focusedPlanet.GetComponent<DragRotate>().enabled = true;

            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);
        }

        private void OnArtifactInfoUIChange(object sender, GameEventArgs e)
        {
            ArtifactInfoUIChangeEventArgs ne = (ArtifactInfoUIChangeEventArgs)e;
            if (ne == null)
                return;

            if(ne.Type == Constant.Type.UI_OPEN)
            {
                artifactInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIArtifactInfoForm);
            }

            if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (artifactInfoUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)artifactInfoUIID);
                }
                artifactInfoUIID = null;
            }
        }

        private void OnNPCUIChange(object sender, GameEventArgs e)
        {
            NPCUIChangeEventArgs ne = (NPCUIChangeEventArgs)e;
            if (ne == null)
                return;

            if (currentNPCUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)currentNPCUIID);
            }

            if (ne.Type == Constant.Type.NPC_UI_TALK_OPEN)
            {
                currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
            }
            else if (ne.Type == Constant.Type.NPC_UI_TRADE_OPEN)
            {
                currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCTradeForm);
            }
            else if (ne.Type == Constant.Type.UI_CLOSE)
            {
                if (currentNPCUIID != null)
                {
                    GameEntry.UI.CloseUIForm((int)currentNPCUIID);
                }

                currentNPCUIID = null;
            }
        }

        private void OnPlanetLandingPointClick(object sender, GameEventArgs e)
        {
            PlanetLandingPointEventArgs ne = (PlanetLandingPointEventArgs)e;
            if (ne == null)
                return;

            // 打开 planetScene UI
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
            GameEntry.Event.Unsubscribe(ArtifactInfoUIChangeEventArgs.EventId, OnArtifactInfoUIChange);

            MapManager.Instance.focusedPlanet.GetComponent<PlanetBase>().isFocused = false;
            MapManager.Instance.focusedPlanet.GetComponent<DragRotate>().enabled = false;
            MapManager.Instance.focusedPlanet = null;

            artifactInfoUIID = null;
            currentNPCUIID = null;

            CloseUI();
            GameEntry.Sound.StopMusic();
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void MouseControl() {
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
        }
    }
}
