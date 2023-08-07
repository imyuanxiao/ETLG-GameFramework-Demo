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
    public class UIMapPlayerInfoRewardIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {
        private DataNPC dataNPC;
        private DataLearningPath dataLearningPath;

        public void OnPointerEnter(PointerEventData eventData)
        {
            dataLearningPath = GameEntry.Data.GetData<DataLearningPath>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();

            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 offset = new Vector3(400f, -30f, 0);
            Vector3 newPosition = itemPosition + offset;

            dataNPC.currentNPCId = dataLearningPath.getCurrentPath().NPCId;
            
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
