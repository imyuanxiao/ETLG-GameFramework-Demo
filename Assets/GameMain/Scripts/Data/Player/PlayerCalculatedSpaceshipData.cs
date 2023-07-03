using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using System;

namespace ETLG.Data
{
    public class PlayerCalculatedSpaceshipData
    {

        public float Energy { get; set; }
        public float Durability { get; set; }
        public float Shields { get; set; }
        public float Firepower { get; set; }
        public float FireRate { get; set; }
        public float Agility { get; set; }
        public float Speed { get; set; }
        public float Dogde { get; set; }
        public float Detection { get; set; }
        public int Capacity { get; set; }


        public PlayerCalculatedSpaceshipData(SpaceshipData spaceshipData) {

            Energy = spaceshipData.Energy;
            Durability = spaceshipData.Durability;
            Shields = spaceshipData.Shields;
            Firepower = spaceshipData.Firepower;
            FireRate = spaceshipData.FireRate;
            Agility = spaceshipData.Agility;
            Speed = spaceshipData.Speed;
            Dogde = spaceshipData.Dogde;
            Detection = spaceshipData.Detection;
            Capacity = spaceshipData.Capacity;
        }

    }
}

