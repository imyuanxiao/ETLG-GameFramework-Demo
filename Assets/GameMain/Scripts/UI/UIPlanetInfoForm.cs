﻿using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIPlanetInfoForm : UGuiFormEx
    {


        public TextMeshProUGUI p_name = null;
        public TextMeshProUGUI p_type = null;
        public TextMeshProUGUI p_description = null;

        public Transform LandingPointsContainer;

        public Button closeButton = null;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            closeButton.onClick.AddListener(OnCloseButtonClick);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            PlanetData currentPlanetData = GameEntry.Data.GetData<DataPlanet>().GetCurrentPlanetData();
            p_name.text = currentPlanetData.Name;
            p_type.text = currentPlanetData.TypeStr;
            p_description.text = currentPlanetData.Description;

            ShowLandingPoints();
        }
        private void ShowLandingPoints()
        {

            int[] LandingPoints = GameEntry.Data.GetData<DataPlanet>().GetCurrentPlanetData().LandingPoints;

            foreach (var LandingPoint in LandingPoints)
            {
                ShowItem<ItemLandingPointSelect>(EnumItem.ItemLandingPointSelect, (item) =>
                {
                    item.transform.SetParent(LandingPointsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemLandingPointSelect>().SetData(LandingPoint);
                });
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }


        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            this.Close();
        }
    }
}


