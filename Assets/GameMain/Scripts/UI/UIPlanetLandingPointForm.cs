using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace ETLG
{
    public class UIPlanetLandingPointForm : UGuiFormEx
    {

        public RectTransform NPCsContainer;

     //  public Transform NPCsContainer;
        public TextMeshProUGUI Desc;
        public TextMeshProUGUI Title;

        public Button closeButton;
        public VerticalLayoutGroup containerVerticalLayoutGroup;
        public VerticalLayoutGroup scrollVerticalLayoutGroup;

        private DataLearningProgress dataLearningProgress;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            closeButton.onClick.AddListener(OnCloseButtonClick);
            dataLearningProgress= GameEntry.Data.GetData<DataLearningProgress>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }
            ShowNPCSelectionButtonItems();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)containerVerticalLayoutGroup.transform);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (dataLearningProgress.UIPlanetLandingPointsUpdate)
            {
                HideAllItem();
                ShowNPCSelectionButtonItems();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)containerVerticalLayoutGroup.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scrollVerticalLayoutGroup.transform);
                dataLearningProgress.UIPlanetLandingPointsUpdate = false;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)containerVerticalLayoutGroup.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)scrollVerticalLayoutGroup.transform);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void ShowNPCSelectionButtonItems()
        {

            NPCData[] npcDatas = GameEntry.Data.GetData<DataLandingPoint>().GetCurrentLandingPointData().npcs;
            Title.text = GameEntry.Data.GetData<DataLandingPoint>().GetCurrentLandingPointData().Title;
            Desc.text= GameEntry.Data.GetData<DataLandingPoint>().GetCurrentLandingPointData().Description;
            Log.Debug(Desc.text);
            foreach (var npcData in npcDatas)
            {
                ShowItem<ItemNPCSelect>(EnumItem.ItemNPCSelect, (item) =>
                {
                    item.transform.SetParent(NPCsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.GetComponent<ItemNPCSelect>().SetNPCData(npcData);
                });
            }
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            if (GameEntry.UI.HasUIForm(EnumUIForm.UIPlanetLandingPointForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIPlanetLandingPointForm));
            }
        }
    }
}


