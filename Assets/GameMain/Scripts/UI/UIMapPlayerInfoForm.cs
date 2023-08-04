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

        public RectTransform UIContainer;

        public Button playerMenuButton;

        //public InputField searchField;
        public TMP_InputField tmpInputField;
       // public TextMeshProUGUI displayText;

       private bool refreshPlanetsContainer;

        private bool refresh;
       // private bool expandedAll;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerMenuButton.onClick.AddListener(OnPlayerMenuButtonClick);
            tmpInputField.onEndEdit.AddListener(OnInputEndEdit);
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

            this.refresh = true;

            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }

            // tutorial in spaceship menu
            GameEntry.Data.GetData<DataTutorial>().OpenGroupTutorials(2);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if (refresh)
            {
                HideAllItem();
                ShowPlanets(PlanetsContainer);

                LayoutRebuilder.ForceRebuildLayoutImmediate(UIContainer);

                refresh = false;
            }

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

        private void ShowPlanets(RectTransform PlanetsContainer)
        {

            int[] PlanetIDs = GameEntry.Data.GetData<DataPlanet>().GetAllPlanetIDs();

            foreach (var PlanetID in PlanetIDs)
            {
                // 此处判断Planet是否有相关课程
                if (!PlanetHasRelatedCourses(PlanetID))
                {
                    continue;
                }

                ShowItem<ItemPlanetSelect>(EnumItem.ItemPlanetSelect, (item) =>
                {
                    item.transform.SetParent(PlanetsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemPlanetSelect>().SetData(PlanetID);
                });
            }
        }

        private bool PlanetHasRelatedCourses(int PlanetID)
        {
            DataPlanet planet = GameEntry.Data.GetData<DataPlanet>();
            string keyword = planet.keyword;
            if(keyword == null || keyword.Equals(""))
            {
                return true;
            }

            int[] courseId = planet.GetPlanetData(PlanetID).LandingPoints;
            foreach(var courseID in courseId)
            {
               string title = GameEntry.Data.GetData<DataLandingPoint>().GetLandingPointData(courseID).Title;
                if (title.Contains(keyword)) { return true; }
            }

            return false;
        }

        private void OnPlanetExpanded(object sender, GameEventArgs e)
        {
            PlanetExpandedEventArgs ne = (PlanetExpandedEventArgs)e;
            if (ne == null)
                return;
            //refresh = true;
            refreshPlanetsContainer = true;
        }

        public void OnInputEndEdit(string inputText)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // displayText.text = "You entered: " + inputText;
                //Log.Debug(inputText);
                GameEntry.Data.GetData<DataPlanet>().keyword = inputText;
                //GameEntry.Data.GetData<DataPlanet>().expandAll = true;

                refresh = true;
                //refreshPlanetsContainer = true;
                //expandedAll = true;
            }
        }

    }
}
