using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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

            ShowNPCSelectionButtonItems();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void ShowNPCSelectionButtonItems()
        {

            NPCData[] npcDatas = GameEntry.Data.GetData<DataLandingPoint>().GetCurrentLandingPointData().npcs;
            Vector3 offset = new Vector3(0f, -50f, 0f); // 偏移量

            int i = 0;

            foreach (var npcData in npcDatas)
            {
                ShowItem<ItemNPCSelect>(EnumItem.NPCSelect, (item) =>
                {
                    item.transform.SetParent(NPCsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + (i++) * offset; // 根据偏移量计算新的位置
                    item.GetComponent<ItemNPCSelect>().SetNPCData(npcData);
                });
            }
        }

        private void OnCloseButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            this.Close();

        }
    }
}


