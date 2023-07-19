using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETLG.Data;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class SaveManager : Singleton<SaveManager>
    {
        public SavedGamesInfo savedGamesInfo;
        private void OnEnable() 
        {
            savedGamesInfo = new SavedGamesInfo();
            Load("SavedGamesInfo", savedGamesInfo);
        }

        // int SaveId represent the idx of one of the five save slots
        public void SaveGame(int SaveId = 0)
        {
            if (SaveId < 0 || SaveId > 4)
            {
                Log.Error("Invalid Save Id, SaveId must be between 0, 1, 2, 3, 4");
                return;
            }

            string saveIdStr = "_" + SaveId.ToString();
            
            Save("InitialSpaceshipIdx" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().initialSpaceship.Id);
            Save("PlayerCalculatedSpaceshipData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
            Save("PlayerSkillData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            Save("PlayerArtifacts" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_ALL));
            Save("PlayerModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL));
            Save("EquippedModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetEquippedModules());
            Save("PlayerNPCs" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData());
            Save("PlayerAchievement" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerAchievement());

            if (savedGamesInfo.savedGamesDic.ContainsKey(SaveId))
            {
                savedGamesInfo.savedGamesDic[SaveId] = DateTime.Now.ToLongTimeString();
            }
            else
            {
                savedGamesInfo.savedGamesDic.Add(SaveId, DateTime.Now.ToLongTimeString());
            }

            Save("SavedGamesInfo", savedGamesInfo);

            // GameEntry.Event.Fire(this, SaveGameEventArgs.Create(SaveId));
        }

        // int SaveId represent the idx of one of the five save slots
        public void LoadGame(int SaveId = 0)
        {
            if (SaveId < 0 || SaveId > 4)
            {
                Log.Error("Invalid Save Id, SaveId must be between 0, 1, 2, 3, 4");
                return;
            }

            string saveIdStr = "_" + SaveId.ToString();

            int initialSpaceshipId = LoadObject<int>("InitialSpaceshipIdx" + saveIdStr);
            
            GameEntry.Data.GetData<DataPlayer>().LoadGame(GameEntry.Data.GetData<DataSpaceship>().GetSpaceshipData(initialSpaceshipId));

            Load("PlayerSpaceshipData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
            Load("PlayerSkillData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            Load("PlayerArtifacts" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_ALL));
            Load("PlayerModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL));
            Load("EquippedModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetEquippedModules());
            Load("PlayerNPCs" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData());
            Load("PlayerAchievement" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerAchievement());

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        public void DeleteSave(int SaveId)
        {
            if (SaveId < 0 || SaveId > 4)
            {
                Log.Error("Invalid Save Id, SaveId must be between 0, 1, 2, 3, 4");
                return;
            }
            string saveIdStr = "_" + SaveId.ToString();

            Delete("PlayerSpaceshipData" + saveIdStr);
            Delete("PlayerSkillData" + saveIdStr);
            Delete("PlayerArtifacts" + saveIdStr);
            Delete("PlayerModules" + saveIdStr);
            Delete("EquippedModules" + saveIdStr);
            Delete("PlayerNPCs" + saveIdStr);
            Delete("PlayerAchievement" + saveIdStr);

            if (savedGamesInfo.savedGamesDic.ContainsKey(SaveId))
            {
                savedGamesInfo.savedGamesDic.Remove(SaveId);
            }

            Save("SavedGamesInfo", savedGamesInfo);
        }

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

        public void Delete(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
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


        // For Testing Purpose Only !
        private void Update() 
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                // Save("PlayerSpaceshipData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
                // Save("PlayerSkillData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                // Load("PlayerSpaceshipData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
                // Load("PlayerSkillData", GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
                // LoadGame();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower += 10;
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int)EnumSkill.ElectronicWarfare, 0);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintSavedData("InitialSpaceshipIdx_0");
                PrintSavedData("PlayerCalculatedSpaceshipData_0");
                PrintSavedData("PlayerSkillData_0");
                PrintSavedData("PlayerArtifacts_0");
                PrintSavedData("PlayerModules_0");
                PrintSavedData("EquippedModules_0");
                PrintSavedData("PlayerNPCs_0");
                PrintSavedData("PlayerAchievement_0");
                PrintSavedData("SavedGamesInfo");
            }
        }
    }

    // the information displayed at Save UI
    public class SavedGamesInfo
    {
        public Dictionary<int, string> savedGamesDic;

        public SavedGamesInfo()
        {
            this.savedGamesDic = new Dictionary<int, string>();
        }
    }
}
