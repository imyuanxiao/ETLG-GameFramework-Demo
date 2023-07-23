using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemRewardIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {
        private DataNPC dataNPC;
        private int NpcId;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataNPC = GameEntry.Data.GetData<DataNPC>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }

        public void SetData(int npcId)
        {
            NpcId = npcId;
        }


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(-10f, 0, 0);
            Vector3 newPosition = itemPosition + offset;

            dataNPC.currentNPCId = NpcId;
            dataNPC.RewardUIPosition = newPosition;

            GameEntry.UI.OpenUIForm(EnumUIForm.UIRewardPreviewForm);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (GameEntry.UI.HasUIForm(EnumUIForm.UIRewardPreviewForm))
            {
                GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UIRewardPreviewForm));
            }
        }


    }
}


