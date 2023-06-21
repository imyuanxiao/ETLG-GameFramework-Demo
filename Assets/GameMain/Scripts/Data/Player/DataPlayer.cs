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

        // 玩家选择初始飞船后，根据当前选择的飞船，新建玩家信息
        public void NewGame(SpaceshipData spaceshipData)
        {
            playerData = new PlayerData(spaceshipData);
        }

        // 继续游戏，读取本地文件
        public void ContinueGame()
        {
            LoadPlayerData();
        }


        public void SavePlayerData(PlayerData playerData)
        {
/*            // 序列化玩家信息为 JSON 格式
            string json = Utility.Json.ToJson(playerInfo);

            // 将序列化后的玩家信息保存到存档文件
            File.WriteAllBytes("PlayerInfo.dat", Utility.Converter.GetBytes(json));
*/

        }

        public PlayerData LoadPlayerData()
        {

/*            // 从存档文件读取玩家信息
            byte[] data = File.ReadAllBytes("PlayerInfo.dat");

            // 将读取到的数据反序列化为玩家信息对象
            string json = Utility.Converter.GetString(data);
            PlayerInfo playerInfo = Utility.Json.FromJson<PlayerInfo>(json);
*/
            return null;
        }


    }
}


