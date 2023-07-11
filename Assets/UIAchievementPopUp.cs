using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIAchievementPopUp : UGuiFormEx
    {
        private DataAchievement dataAchievement;
        public TextMeshProUGUI acheivementName = null;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            acheivementName.text = dataAchievement.GetDataById(dataAchievement.cuurrentPopUpId).Name.ToString();
        }
    }
}
