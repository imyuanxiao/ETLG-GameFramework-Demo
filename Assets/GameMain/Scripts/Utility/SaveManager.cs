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
            // Save("PlayerCalculatedSpaceshipData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
            Save("PlayerSkillData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            Save("PlayerArtifacts" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_ALL));
            Save("PlayerModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL));
            Save("EquippedModules" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetEquippedModules());
            Save("PlayerNPCs" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData());
            Save("PlayerAchievement" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerAchievement());
            Save("BattleVictoryCount" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().battleVictoryCount);
            Save("BossDefeatTime" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().bossDefeatTime);

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

            // Load("PlayerSpaceshipData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData);
            Load("PlayerSkillData" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetAllSkills());
            Load("PlayerArtifacts" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetArtifactsByType(Constant.Type.ARTIFACT_ALL));
            LoadPlayerModules("PlayerModules" + saveIdStr);
            LoadEquippedModules("EquippedModules" + saveIdStr);
            LoadPlayerNPCs("PlayerNPCs" + saveIdStr);
            Load("PlayerAchievement" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerAchievement());
            Load("BossDefeatTime" + saveIdStr, GameEntry.Data.GetData<DataPlayer>().GetPlayerData().bossDefeatTime);
            GameEntry.Data.GetData<DataPlayer>().GetPlayerData().battleVictoryCount = LoadObject<int>("BattleVictoryCount" + saveIdStr);

            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));
        }

        private void LoadPlayerNPCs(string key)
        {
            JObject jsonData = LoadJsonObject(key);
            foreach (int npcId in GameEntry.Data.GetData<DataNPC>().getAllNPCsID())
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData()[npcId].NPCId = (int) jsonData[npcId.ToString()]["NPCId"];
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData()[npcId].Money = (int) jsonData[npcId.ToString()]["Money"];
                JArray artifacts = (JArray) jsonData[npcId.ToString()]["Artifacts"];

                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData()[npcId].Artifacts = new int[artifacts.Count];
                for (int i=0; i < artifacts.Count; i++)
                {
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetPlayerNPCsData()[npcId].Artifacts[i] = (int) artifacts[i];
                }
            }
        }

        private void LoadEquippedModules(string key)
        {
            JArray jsonData = LoadJsonArray(key);
            for (int i=0; i < GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetEquippedModules().Length; i++)
            {
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetEquippedModules()[i] = (int) jsonData[i];
            }
        }

        private void LoadPlayerModules(string key)
        {
            JArray jsonData = LoadJsonArray(key);
            for (int i=0; i < jsonData.Count; i++)
            {
                if (i >= GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL).Count)
                {
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL).Add((int) jsonData[i]);
                }
                else
                {
                    GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL)[i] = (int) jsonData[i];
                }
            }
            // for (int i=0; i < GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL).Count; i++)
            // {
            //     GameEntry.Data.GetData<DataPlayer>().GetPlayerData().GetModulesByType(Constant.Type.MODULE_TYPE_ALL)[i] = (int) jsonData[i];
            // }
        }

        public Dictionary<string, string> UploadSave(int SaveId)
        {
            this.savedGamesInfo.cloudSaveId = SaveId;
            Save("SavedGamesInfo", savedGamesInfo);

            Dictionary<string, string> result = new Dictionary<string, string>();

            string saveIdStr = "_" + SaveId.ToString();
            result.Add("InitialSpaceshipIdx" + saveIdStr, PlayerPrefs.GetString("InitialSpaceshipIdx" + saveIdStr));
            result.Add("PlayerSkillData" + saveIdStr, PlayerPrefs.GetString("PlayerSkillData" + saveIdStr));
            result.Add("PlayerArtifacts" + saveIdStr, PlayerPrefs.GetString("PlayerArtifacts" + saveIdStr));
            result.Add("PlayerModules" + saveIdStr, PlayerPrefs.GetString("PlayerModules" + saveIdStr));
            result.Add("EquippedModules" + saveIdStr, PlayerPrefs.GetString("EquippedModules" + saveIdStr));
            result.Add("PlayerNPCs" + saveIdStr, PlayerPrefs.GetString("PlayerNPCs" + saveIdStr));
            result.Add("PlayerAchievement" + saveIdStr, PlayerPrefs.GetString("PlayerAchievement" + saveIdStr));
            result.Add("BattleVictoryCount" + saveIdStr, PlayerPrefs.GetString("BattleVictoryCount" + saveIdStr));
            result.Add("BossDefeatTime" + saveIdStr, PlayerPrefs.GetString("BossDefeatTime" + saveIdStr));

            return result;
        }

        public void DeleteSave(int SaveId)
        {
            if (SaveId < 0 || SaveId > 4)
            {
                Log.Error("Invalid Save Id, SaveId must be between 0, 1, 2, 3, 4");
                return;
            }
            string saveIdStr = "_" + SaveId.ToString();

            // Delete("PlayerSpaceshipData" + saveIdStr);
            Delete("PlayerSkillData" + saveIdStr);
            Delete("PlayerArtifacts" + saveIdStr);
            Delete("PlayerModules" + saveIdStr);
            Delete("EquippedModules" + saveIdStr);
            Delete("PlayerNPCs" + saveIdStr);
            Delete("PlayerAchievement" + saveIdStr);
            Delete("BattleVictoryCount" + saveIdStr);
            Delete("BossDefeatTime" + saveIdStr);

            if (savedGamesInfo.savedGamesDic.ContainsKey(SaveId))
            {
                savedGamesInfo.savedGamesDic.Remove(SaveId);
            }
            if (savedGamesInfo.cloudSaveId == SaveId)
            {
                savedGamesInfo.cloudSaveId = -1;
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

        public void SaveResolution(int width, int height)
        {
            PlayerPrefs.SetInt("Resolution_Width", width);
            PlayerPrefs.SetInt("Resolution_Height", height);
        }

        public void SaveSoundVolume(float soundVolume)
        {
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        }

        public int[] LoadResolution()
        {
            int[] resolution = {1920, 1080};
            if (PlayerPrefs.HasKey("Resolution_Width") && PlayerPrefs.HasKey("Resolution_Height"))
            {
                resolution[0] = PlayerPrefs.GetInt("Resolution_Width");
                resolution[1] = PlayerPrefs.GetInt("Resolution_Height");
            }
            return resolution;
        }

        public void LoadSoundVolume()
        {
            if (PlayerPrefs.HasKey("SoundVolume"))
            {
                AudioListener.volume = PlayerPrefs.GetFloat("SoundVolume");
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

        public JArray LoadJsonArray(string key)
        {
            if (PlayerPrefs.HasKey(key)) 
            {
                JArray jsonArray = JArray.Parse(PlayerPrefs.GetString(key));
                return jsonArray;
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
                // GameEntry.Data.GetData<DataPlayer>().GetPlayerData().playerCalculatedSpaceshipData.Firepower += 10;
                // GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddSkill((int)EnumSkill.ElectronicWarfare, 0);
                GameEntry.Data.GetData<DataPlayer>().GetPlayerData().AddArtifact(3010, 1);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintSavedData("InitialSpaceshipIdx_0");
                // PrintSavedData("PlayerCalculatedSpaceshipData_0");
                PrintSavedData("PlayerSkillData_0");
                PrintSavedData("PlayerArtifacts_0");
                PrintSavedData("PlayerModules_0");
                PrintSavedData("EquippedModules_0");
                PrintSavedData("PlayerAchievement_0");
                PrintSavedData("SavedGamesInfo");
                PrintSavedData("PlayerNPCs_0");
                PrintSavedData("BattleVictoryCount_0");
                PrintSavedData("BossDefeatTime_0");
            }
        }
    }

    // the information displayed at Save UI
    public class SavedGamesInfo
    {
        public Dictionary<int, string> savedGamesDic;
        public int cloudSaveId;

        public SavedGamesInfo()
        {
            this.savedGamesDic = new Dictionary<int, string>();
            this.cloudSaveId = -1;
        }
    }
}
