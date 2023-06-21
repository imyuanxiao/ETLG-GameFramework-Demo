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

        // ���ѡ���ʼ�ɴ��󣬸��ݵ�ǰѡ��ķɴ����½������Ϣ
        public void NewGame(SpaceshipData spaceshipData)
        {
            playerData = new PlayerData(spaceshipData);
        }

        // ������Ϸ����ȡ�����ļ�
        public void ContinueGame()
        {
            LoadPlayerData();
        }


        public void SavePlayerData(PlayerData playerData)
        {
/*            // ���л������ϢΪ JSON ��ʽ
            string json = Utility.Json.ToJson(playerInfo);

            // �����л���������Ϣ���浽�浵�ļ�
            File.WriteAllBytes("PlayerInfo.dat", Utility.Converter.GetBytes(json));
*/

        }

        public PlayerData LoadPlayerData()
        {

/*            // �Ӵ浵�ļ���ȡ�����Ϣ
            byte[] data = File.ReadAllBytes("PlayerInfo.dat");

            // ����ȡ�������ݷ����л�Ϊ�����Ϣ����
            string json = Utility.Converter.GetString(data);
            PlayerInfo playerInfo = Utility.Json.FromJson<PlayerInfo>(json);
*/
            return null;
        }

        public PlayerData GetPlayerData()
        {
            return playerData;
        }

    }
}


