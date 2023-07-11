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
            GameEntry.Event.Subscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);

            MapManager.Instance.focusedPlanet = null;

            this.procedureOwner = procedureOwner;
            this.changeScene = false;
            this.changeToProcedurePlanet = false;

            GameEntry.UI.OpenUIForm(EnumUIForm.UIMapPlayerInfoForm);
            // 播放BGM
            GameEntry.Sound.PlayMusic(EnumSound.GameBGM);
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
            if (changeToProcedurePlanet)
            {
                ChangeState<ProcedurePlanet>(procedureOwner);
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
                procedureOwner.SetData<VarString>("BossType", "CloudComputing");
                // procedureOwner.SetData<VarString>("BossType", "Cybersecurity");
                // procedureOwner.SetData<VarString>("BossType", "AI");
                // procedureOwner.SetData<VarString>("BossType", "DataScience");
                // procedureOwner.SetData<VarString>("BossType", "Blockchain");
                // procedureOwner.SetData<VarString>("BossType", "IoT");
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
                if (hitInfo.collider.gameObject.CompareTag("Planet")) 
                {
                    GameObject planet = hitInfo.collider.gameObject;

                    if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetOverview))
                    {
                        Debug.Log("Has UI Form " + EnumUIForm.UIPlanetOverview);
                        GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetOverview).Close();
                    }
                    GameEntry.UI.OpenUIForm(EnumUIForm.UIPlanetOverview, planet.GetComponent<PlanetBase>());
                    

                    // MapManager.Instance.focusedPlanet = planet;
                    // planet.GetComponent<PlanetBase>().isFocused = true;
                    // GameEntry.Data.GetData<DataPlanet>().currentPlanetID = planet.GetComponent<PlanetBase>().PlanetId;

                    // GameEntry.Event.Fire(this, FocusOnPlanetEventArgs.Create(planet.GetComponent<PlanetBase>()));

                    // changeToProcedurePlanet = true;
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            // 取消订阅事件
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, OnChangeScene);
            GameEntry.Event.Unsubscribe(FocusOnPlanetEventArgs.EventId, OnFocusOnPlanet);

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIMapPlayerInfoForm))
            {
                GameEntry.UI.GetUIForm(EnumUIForm.UIMapPlayerInfoForm).Close();
            }
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetOverview))
            {
                GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetOverview);
            }
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
    }
}

