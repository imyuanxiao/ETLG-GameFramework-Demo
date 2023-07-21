using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using ETLG.Data;
using Unity.VisualScripting;
using GameFramework.Event;

namespace ETLG
{
    public class UIMapPlayerInfoForm : UGuiFormEx
    {

        public TextMeshProUGUI Description = null;

        public RectTransform PlanetsContainer;

        public Button playerMenuButton;

        private bool refreshPlanetsContainer;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);

        }

        private void OnPlayerMenuButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.PlayerMenu")));

        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Description.text = GameEntry.Localization.GetString("Map_Welcome_Desc");

            GameEntry.Event.Subscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);

            ShowPlanets();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);
            
            if (refreshPlanetsContainer)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(PlanetsContainer);
                refreshPlanetsContainer = false;
            }



        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(PlanetExpandedEventArgs.EventId, OnPlanetExpanded);

        }

        private void ShowPlanets()
        {

            int[] PlanetIDs = GameEntry.Data.GetData<DataPlanet>().GetAllPlanetIDs();

            foreach (var PlanetID in PlanetIDs)
            {
                ShowItem<ItemPlanetSelect>(EnumItem.ItemPlanetSelect, (item) =>
                {
                    item.transform.SetParent(PlanetsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemPlanetSelect>().SetData(PlanetID);
                });
            }
        }

        private void OnPlanetExpanded(object sender, GameEventArgs e)
        {
            PlanetExpandedEventArgs ne = (PlanetExpandedEventArgs)e;
            if (ne == null)
                return;

            refreshPlanetsContainer = true;


        }


    }
}
