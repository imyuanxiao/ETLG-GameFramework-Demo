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

        private int? currentNPCUIID;
        private int? artifactInfoUIID;

        private RaycastHit hitInfo;  // store the information of the object that the ray hitted
        private PlanetBase currentlyFocusedPlanet = null;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            Log.Debug("进入 ProcedureMap 流程");
            base.OnEnter(procedureOwner);

            // 订阅事件
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Subscribe(PlanetLandingPointEventArgs.EventId, OnPlanetLandingPointClick);

            GameEntry.Event.Subscribe(NPCUIOpenEventArgs.EventId, OnNPCUIOpen);
            GameEntry.Event.Subscribe(NPCUICloseEventArgs.EventId, OnNPCUIClose);

            GameEntry.Event.Subscribe(PlanetInfoEventArgs.EventId, OnPlanetInfo);

            GameEntry.Event.Subscribe(ArtifactInfoOpenEventArgs.EventId, OnArtifactInfoOpen);
            GameEntry.Event.Subscribe(ArtifactInfoCloseEventArgs.EventId, OnArtifactInfoClose);


            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.currentlyFocusedPlanet = null;    

            currentNPCUIID = null;
            artifactInfoUIID = null;



            // 播放BGM
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);

            // 打开UI
            Log.Debug("此处应打开 Map 场景界面");
            // GameEntry.UI.OpenUIForm(EnumUIForm.UIMapInfoForm);

        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            MouseControl();
            
            // Open UI Map Info (for testing purpose only)
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

            if (changeScene)
            {
                ChangeState<ProcedureLoadingScene>(procedureOwner);
            }

            // Switch to battle scene and battle procedure (for test purpose only)
            //   1. switch to basic battle
            if (Input.GetKeyDown(KeyCode.C))
            {
                procedureOwner.SetData<VarString>("BattleType", "BasicBattle");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }
            //  2. switch to intermidate battle (mini boss battle)
            else if (Input.GetKeyDown(KeyCode.V))
            {
                procedureOwner.SetData<VarString>("BattleType", "IntermidateBattle");
                // procedureOwner.SetData<VarString>("BossType", "CloudComputing");
                // procedureOwner.SetData<VarString>("BossType", "Cybersecurity");
                // procedureOwner.SetData<VarString>("BossType", "AI");
                // procedureOwner.SetData<VarString>("BossType", "DataScience");
                // procedureOwner.SetData<VarString>("BossType", "Blockchain");
                procedureOwner.SetData<VarString>("BossType", "IoT");
                // procedureOwner.SetData<VarString>("BossType", "Final");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }
            //  3. switch to final boss battle
            else if (Input.GetKeyDown(KeyCode.B))
            {
                procedureOwner.SetData<VarString>("BattleType", "FinalBattle");
                GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Battle")));
            }
        }

        private void MouseControl() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // store the returned value into hitInfo
            Physics.Raycast(ray, out hitInfo);

            // if left mouse button is clicked and the object that the ray has hit has collider
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null) 
            {
                // if clicked on a planet
                if (hitInfo.collider.gameObject.CompareTag("Planet") && currentlyFocusedPlanet == null) 
                {
                    currentlyFocusedPlanet = hitInfo.collider.gameObject.GetComponent<PlanetBase>();
                    GameEntry.Data.GetData<DataPlanet>().currentPlanetID = currentlyFocusedPlanet.PlanetId;

                    GameEntry.Event.Fire(this, FocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
                    GameEntry.Event.Fire(this, PlanetInfoEventArgs.Create(GameEntry.Data.GetData<DataPlanet>().currentPlanetID));
                }
                if (hitInfo.collider.gameObject.CompareTag("LandingPoint") && currentlyFocusedPlanet != null)
                {
                    LandingPoint currentlyClickedLandingPoint = hitInfo.collider.gameObject.GetComponent<LandingPoint>();
                    GameEntry.Data.GetData<DataLandingPoint>().currentLandingPointID = currentlyClickedLandingPoint.landingPointId;
                    GameEntry.Event.Fire(this, PlanetLandingPointEventArgs.Create());
                } 
            }

            // if right clicked
            if (Input.GetMouseButtonDown(1)) 
            {
                if (currentlyFocusedPlanet != null)
                {
                    GameEntry.Event.Fire(this, UnFocusOnPlanetEventArgs.Create(currentlyFocusedPlanet));
                    currentlyFocusedPlanet = null;
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(PlanetLandingPointEventArgs.EventId, OnPlanetLandingPointClick);
            GameEntry.Event.Unsubscribe(NPCUIOpenEventArgs.EventId, OnNPCUIOpen);

            GameEntry.Event.Unsubscribe(NPCUICloseEventArgs.EventId, OnNPCUIClose);


            GameEntry.Event.Unsubscribe(PlanetInfoEventArgs.EventId, OnPlanetInfo);

            GameEntry.Event.Unsubscribe(ArtifactInfoOpenEventArgs.EventId, OnArtifactInfoOpen);
            GameEntry.Event.Unsubscribe(ArtifactInfoCloseEventArgs.EventId, OnArtifactInfoClose);

            artifactInfoUIID = null;
            currentNPCUIID = null;
            currentlyFocusedPlanet = null;    

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

        private void OnPlanetLandingPointClick(object sender, GameEventArgs e)
        {

            PlanetLandingPointEventArgs ne = (PlanetLandingPointEventArgs)e;
            if (ne == null)
                return;

            
            // 打开 planetScene UI
            GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetLandingPointForm);

        }
        private void OnNPCUIOpen(object sender, GameEventArgs e)
        {

            NPCUIOpenEventArgs ne = (NPCUIOpenEventArgs)e;
            if (ne == null)
                return;

            if (currentNPCUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)currentNPCUIID);
            }

            if (ne.Type == Constant.Type.NPC_UI_TALK)
            {
                currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCDialogForm);
            }

            else if (ne.Type == Constant.Type.NPC_UI_TRADE)
            {
                currentNPCUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UINPCTradeForm);
            }

        }

        private void OnNPCUIClose(object sender, GameEventArgs e)
        {

            NPCUICloseEventArgs ne = (NPCUICloseEventArgs)e;
            if (ne == null)
                return;

            if (currentNPCUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)currentNPCUIID);
            }

            currentNPCUIID = null;

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

        private void OnArtifactInfoOpen(object sender, GameEventArgs e)
        {
            ArtifactInfoOpenEventArgs ne = (ArtifactInfoOpenEventArgs)e;
            if (ne == null)
                return;
            artifactInfoUIID = GameEntry.UI.OpenUIForm(EnumUIForm.UIArtifactInfoForm);

        }

        private void OnArtifactInfoClose(object sender, GameEventArgs e)
        {
            ArtifactInfoCloseEventArgs ne = (ArtifactInfoCloseEventArgs)e;
            if (ne == null)
                return;
            if (artifactInfoUIID != null)
            {
                GameEntry.UI.CloseUIForm((int)artifactInfoUIID);
            }
            artifactInfoUIID = null;
        }



    }
}

