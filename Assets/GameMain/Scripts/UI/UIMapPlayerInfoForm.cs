using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using TMPro;
using ETLG.Data;

namespace ETLG
{
    public class UIMapPlayerInfoForm : UGuiFormEx
    {
        public RawImage avatarImage;
        // name and description
        public TextMeshProUGUI s_name = null;

        // type and size
        public TextMeshProUGUI s_type = null;
        public TextMeshProUGUI s_size = null;

        // initial attrs
        public TextMeshProUGUI s_durability = null;
        public TextMeshProUGUI s_shields = null;
        public TextMeshProUGUI s_firepower = null;
        public TextMeshProUGUI s_energy = null;
        public TextMeshProUGUI s_agility = null;
        public TextMeshProUGUI s_speed = null;

        public TextMeshProUGUI s_detection = null;
        public TextMeshProUGUI s_capacity = null;

        public TextMeshProUGUI s_fireRate = null;
        public TextMeshProUGUI s_dogde = null;
        public TextMeshProUGUI playerScore = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            PlayerCalculatedSpaceshipData data = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData;

            s_name.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().initialSpaceship.NameId;
            s_type.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().initialSpaceship.SType;
            s_size.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().initialSpaceship.SSize.ToString();

            s_durability.text = data.Durability.ToString();
            s_shields.text = data.Shields.ToString();
            s_firepower.text = data.Firepower.ToString();
            s_energy.text = data.Energy.ToString();
            s_agility.text = data.Energy.ToString();
            s_speed.text = data.Speed.ToString();
            s_detection.text = data.Detection.ToString();
            s_capacity.text = data.Capacity.ToString();
            s_fireRate.text = data.FireRate.ToString();
            s_dogde.text = data.Dogde.ToString();
            playerScore.text = GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerScore().ToString();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
