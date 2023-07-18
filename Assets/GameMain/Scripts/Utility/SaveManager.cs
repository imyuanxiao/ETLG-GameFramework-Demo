using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ETLG
{
    public class SaveManager : Singleton<SaveManager>
    {
        public void Save(string key, object data) 
        {
            string jsonData = JsonConvert.SerializeObject(data);
            
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public void Load(string key, object data)
        {
            if (PlayerPrefs.HasKey(key))
            {
                JsonConvert.PopulateObject(PlayerPrefs.GetString(key), data);
            }
        }

        public T LoadObject<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(key));
            }
            return default(T);
        }

        public JObject LoadJsonObject(string key) 
        {
            if (PlayerPrefs.HasKey(key)) 
            {
                JObject jsonObj = JObject.Parse(PlayerPrefs.GetString(key));
                return jsonObj;
            }
            return null;
        }

        public void PrintSavedData(string key)
        {
            Debug.Log(key + " : " + PlayerPrefs.GetString(key));
        }

        public void LoadPlayerSkillData()
        {
            JObject jsonObj = LoadJsonObject("PlayerSkillData");
            JArray jsonArr  = (JArray) jsonObj[0];
        }

        // For Testing Purpose Only !
        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Save("PlayerSpaceshipData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
                Save("PlayerSkillData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load("PlayerSpaceshipData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
                Load("PlayerSkillData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower += 10;
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int)EnumSkill.ElectronicWarfare, 0);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintSavedData("PlayerSpaceshipData");
                PrintSavedData("PlayerSkillData");
            }
        }
    }
}
