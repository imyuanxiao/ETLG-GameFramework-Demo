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

        public Transform NPCsContainer;

        public Button closeButton;
       

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            closeButton.onClick.AddListener(OnCloseButtonClick);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
            }
            ShowNPCSelectionButtonItems();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void ShowNPCSelectionButtonItems()
        {

            NPCData[] npcDatas = GameEntry.Data.GetData<DataLandingPoint>().GetCurrentLandingPointData().npcs;
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


