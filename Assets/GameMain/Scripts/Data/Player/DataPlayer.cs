using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Data;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace ETLG.Data
{
    public sealed class DataPlayer : DataBase
    {
        private PlayerData playerData;

        protected override void OnInit()
        {

        }

        protected override void OnPreload()
        {
        }

        protected override void OnLoad()
        {
            
        }

        protected override void OnUnload()
        {
        }

        protected override void OnShutdown()
        {

        }

        // new player
        public void NewGame(SpaceshipData spaceshipData)
        {
            playerData = new PlayerData(spaceshipData);
        }

        public void ContinueGame()
        {
            LoadPlayerData();
        }

        // save player to local
        public void SavePlayerData(PlayerData playerData)
        {
/*            string json = Utility.Json.ToJson(playerInfo);

            File.WriteAllBytes("PlayerInfo.dat", Utility.Converter.GetBytes(json));*/

        }

        public PlayerData LoadPlayerData()
        {

/*            byte[] data = File.ReadAllBytes("PlayerInfo.dat");

            string json = Utility.Converter.GetString(data);
            PlayerInfo playerInfo = Utility.Json.FromJson<PlayerInfo>(json);*/

            return null;
        }

        public PlayerData GetPlayerData()
        {
            return playerData;
        }

    }
}


