using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Procedure;
using System;
using UnityGameFramework.Runtime;
using ETLG.Data;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ETLG
{
    public class ProcedureLoadingScene : ProcedureBase
    {
        private bool loadSceneCompleted = false;
        //private bool notDestoryMap = false;

        private SceneData sceneData = null;

        private int loadingSceneId = -1;


        protected override void OnInit(ProcedureOwner procedureOwner)
        {

            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug("进入 ProcedureLoadingScene 流程");

            loadSceneCompleted = false;
            loadingSceneId = -1;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
                
            // 添加一个事件，用于防止星系地图场景被销毁，星系地图应该只有在回到主菜单、退出游戏等情况下才会销毁
            // 后面可以改成一个比较通用的事件，NotDestroySceneEventArgs,从而防止任何指定场景被销毁。
            //GameEntry.Event.Subscribe(NotDestroyMapEventArgs.EventId, OnNotDestroyMap);

            // 卸载所有场景，除了指定不要销毁的场景，目前只有大地图
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
              /*  if (notDestoryMap)
                {
                    // 此处判断是否是星系地图，是就保留，判断条件需要再改
                    if (loadedSceneAssetNames[i].Equals("Map"))
                        continue;
                }*/
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 关闭所有UI
            GameEntry.UI.CloseAllLoadedUIForms();

            loadingSceneId = procedureOwner.GetData<VarInt32>(Constant.ProcedureData.NextSceneId).Value;
            sceneData = GameEntry.Data.GetData<DataScene>().GetSceneData(loadingSceneId);

            if (sceneData == null)
            {
                Log.Warning("Can not can scene data id :'{0}'.", loadingSceneId.ToString());
                return;
            }

            GameEntry.Scene.LoadScene(sceneData.AssetPath, Constant.AssetPriority.SceneAsset, this);

        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (loadSceneCompleted)
            {
                Type procedureType = Type.GetType(string.Format("ETLG.{0}", sceneData.Procedure));
                if (null != procedureType)
                {

                    ChangeState(procedureOwner, procedureType);
                }
                else
                    Log.Warning("Can not change state,scene procedure '{0}' error, from scene '{1}.{2}'.", sceneData.Procedure.ToString(), sceneData.Id, sceneData.AssetPath);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            loadingSceneId = -1;

            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            //GameEntry.Event.Unsubscribe(NotDestroyMapEventArgs.EventId, OnNotDestroyMap);

        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            loadSceneCompleted = true;

            // GameEntry.Event.Fire(this, LoadLevelFinishEventArgs.Create(loadingSceneId));
            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }


        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

/*        private void OnNotDestroyMap(object sender, GameEventArgs e)
        {
            NotDestroyMapEventArgs ne = (NotDestroyMapEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            notDestoryMap = true;

            Log.Info("Scene Map won't be destroied.");
        }
*/
    }
}
