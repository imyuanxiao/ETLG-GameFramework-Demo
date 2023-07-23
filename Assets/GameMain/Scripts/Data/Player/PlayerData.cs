﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Networking.Types;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

namespace ETLG.Data
{
  public sealed class PlayerData
    {

        // Initial SpaceshipData(include model asset)
        public SpaceshipData initialSpaceship { get;  set; }

        // Spaceship attributes increased by skills, etc. (for display and calculation)
        public PlayerCalculatedSpaceshipData playerCalculatedSpaceshipData { get; set; }

        // Player position
        public Vector3 position { get; set; }
        
        // player artifacts - ID, Number
        private Dictionary<int, int> playerArtifacts { get; set; }
        private List<int> playerModules { get; set; }

        private int[] equippedModules { get; set; }

        private DataArtifact dataArtifact { get; set; }

        // Player Skill - Skill ID, Skill Level,
        // unexists = locked, level = 0 = unlocked, >= 1 = upgraded 
        private Dictionary<int, int> playerSkills { get; set; }

        private DataSkill dataSkill { get; set; }

        //Player NPC Data
        private Dictionary<int, PlayerNPCData> playerNPCs { get; set; }

        private DataNPC dataNPC { get; set; }

        //Player Achievement Data
        private Dictionary<int,int> playerAchievement { get; set; }
        private DataAchievement dataAchievement { get; set; }

        public UITradeData UI_tradeData = null;
        public int battleVictoryCount;

        public PlayerData (SpaceshipData spaceshipData)
        {
            initialSpaceship = spaceshipData;
            playerCalculatedSpaceshipData = new PlayerCalculatedSpaceshipData(spaceshipData);

            // initialize attrs
            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            dataAchievement= GameEntry.Data.GetData<DataAchievement>();

            playerArtifacts = new Dictionary<int, int>(); // id + number
            playerModules = new List<int>(); // 

            playerSkills = new Dictionary<int, int>();

            playerNPCs = new Dictionary<int, PlayerNPCData>();
            instantiatePlayerNPCs();

            playerAchievement = new Dictionary<int, int>(); // id + level

            // player can only equip 6 module, 0-weapon, 1-attack, 2-defense, 3-powerdrive, 4- support, 6-support
            equippedModules = new int[6];

            // add initial skills
            foreach (var id in spaceshipData.SkillIds)
            {
                AddSkill(id, 1);

            }

            UpdateAttrsByAllSkills(Constant.Type.ADD);

            // add money and skill points
            playerArtifacts.Add((int)EnumArtifact.Money, 0);
            playerArtifacts.Add((int)EnumArtifact.KnowledgePoint, 0);

            // add mock artifacts
            AddMockData();
            //initPlayerAchievementData();

            battleVictoryCount = 0;
        }

        private void instantiatePlayerNPCs()
        {
            foreach(int id in dataNPC.getAllNPCsID())
            {
                playerNPCs.Add(id, new PlayerNPCData(dataNPC.GetNPCData(id)));
            }
        }

        // Call this method everytime skills change or initialSpaceship changes
        public void UpdateAttributeByType(int AttrType, float Change)
        {

            float newValue = 0;

            // get old value
            switch (AttrType)
            {
                case Constant.Type.ATTR_Durability:
                    newValue = playerCalculatedSpaceshipData.Durability;
                    break;
                case Constant.Type.ATTR_Shields:
                    newValue = playerCalculatedSpaceshipData.Shields;
                    break;
                case Constant.Type.ATTR_Firepower:
                    newValue = playerCalculatedSpaceshipData.Firepower;
                    break;
                case Constant.Type.ATTR_Energy:
                    newValue = playerCalculatedSpaceshipData.Energy;
                    break;
                case Constant.Type.ATTR_Agility:
                    newValue = playerCalculatedSpaceshipData.Agility;
                    break;
                case Constant.Type.ATTR_Speed:
                    newValue = playerCalculatedSpaceshipData.Speed;
                    break;
                case Constant.Type.ATTR_Detection:
                    newValue = playerCalculatedSpaceshipData.Detection;
                    break;
                case Constant.Type.ATTR_Capacity:
                    newValue = playerCalculatedSpaceshipData.Capacity;
                    break;
                case Constant.Type.ATTR_Firerate:
                    newValue = playerCalculatedSpaceshipData.FireRate;
                    break;
                case Constant.Type.ATTR_Dogde:
                    newValue = playerCalculatedSpaceshipData.Dogde;
                    break;
            }

            newValue += Change;

            if (newValue >= Constant.Type.ATTR_MAX_VALUE)
            {
                newValue = Constant.Type.ATTR_MAX_VALUE;
            }
            if (newValue <= 0)
            {
                newValue = 0;
            }

            // set new value
            switch (AttrType)
            {
                case Constant.Type.ATTR_Durability:
                    playerCalculatedSpaceshipData.Durability = newValue;
                    break;
                case Constant.Type.ATTR_Shields:
                    playerCalculatedSpaceshipData.Shields = newValue;
                    break;
                case Constant.Type.ATTR_Firepower:
                    playerCalculatedSpaceshipData.Firepower = newValue;
                    break;
                case Constant.Type.ATTR_Energy:
                    playerCalculatedSpaceshipData.Energy = newValue;
                    break;
                case Constant.Type.ATTR_Agility:
                    playerCalculatedSpaceshipData.Agility = newValue;
                    break;
                case Constant.Type.ATTR_Speed:
                    playerCalculatedSpaceshipData.Speed = newValue;
                    break;
                case Constant.Type.ATTR_Detection:
                    playerCalculatedSpaceshipData.Detection = newValue;
                    break;
                case Constant.Type.ATTR_Capacity:
                    playerCalculatedSpaceshipData.Capacity = newValue;
                    break;
                case Constant.Type.ATTR_Firerate:
                    playerCalculatedSpaceshipData.FireRate = newValue;
                    break;
                case Constant.Type.ATTR_Dogde:
                    playerCalculatedSpaceshipData.Dogde += newValue;
                    break;
            }

        }

        public float GetSpaceshipScore()
        {
            float sum = 0;
            sum += playerCalculatedSpaceshipData.Durability;
            sum += playerCalculatedSpaceshipData.Shields;
            sum += playerCalculatedSpaceshipData.Energy;
            sum += playerCalculatedSpaceshipData.Firepower;
            sum += playerCalculatedSpaceshipData.Energy;
            sum += playerCalculatedSpaceshipData.Agility;
            sum += playerCalculatedSpaceshipData.Speed;
            sum += playerCalculatedSpaceshipData.Detection;
            sum += playerCalculatedSpaceshipData.Capacity;
            sum += playerCalculatedSpaceshipData.FireRate * 100;
            sum += playerCalculatedSpaceshipData.Dogde * 100;
            return sum;
        }

        public float GetPlayerScore()
        {

            return GetSpaceshipScore();
        }


        public float GetSpaceshipAttribute(int AttrType)
        {
            switch (AttrType)
            {
                case Constant.Type.ATTR_Durability:
                    return playerCalculatedSpaceshipData.Durability;
                case Constant.Type.ATTR_Shields:
                    return playerCalculatedSpaceshipData.Shields;
                case Constant.Type.ATTR_Firepower:
                    return playerCalculatedSpaceshipData.Firepower;
                case Constant.Type.ATTR_Energy:
                    return playerCalculatedSpaceshipData.Energy;
                case Constant.Type.ATTR_Agility:
                    return playerCalculatedSpaceshipData.Agility;
                case Constant.Type.ATTR_Speed:
                    return playerCalculatedSpaceshipData.Speed;
                case Constant.Type.ATTR_Detection:
                    return playerCalculatedSpaceshipData.Detection;
                case Constant.Type.ATTR_Capacity:
                    return playerCalculatedSpaceshipData.Capacity;
                case Constant.Type.ATTR_Firerate:
                    return playerCalculatedSpaceshipData.FireRate;
                case Constant.Type.ATTR_Dogde:
                    return playerCalculatedSpaceshipData.Dogde;
            }
            return 0f;
        }
        public void AddArtifact(int id, int number)
        {
            // module -> playerModules
            if(dataArtifact.GetArtifactData(id) is ArtifactModuleData)
            {
                if(!playerModules.Contains(id))
                {
                    playerModules.Add(id);
                }
                return;
            }

            // artifacts -> playerArtifacts
            if(!playerArtifacts.ContainsKey(id))
            {
                playerArtifacts.Add(id, number);
            }
            else
            {
                playerArtifacts[id] += number;
            }

        }

        //update ALL artifacts after trading
        public void updateArtifact(Dictionary<int,int> newPlayerArtifacts)
        {
            foreach (KeyValuePair<int, int> kvp in newPlayerArtifacts)
            {
                int id = kvp.Key;
                int number = kvp.Value;

                if (playerArtifacts.ContainsKey(id))
                {
                    playerArtifacts[id] = number;
                }
                else
                {
                    AddArtifact(id, number);
                }
            }
        }

        public void DeleteArtifact(int id, int number)
        {
            // module
            if (dataArtifact.GetArtifactData(id) is ArtifactModuleData)
            {
                if (!playerModules.Contains(id))
                {
                    return;
                }
                playerModules.Remove(id);
            }

            // artifacts
            if (!playerArtifacts.ContainsKey(id))
            {
                return;
            }

            playerArtifacts[id] -= number;
            if (playerArtifacts[id] <= 0)
            {
                playerArtifacts.Remove(id);
            }
        }

        public void SellArtifact(int id, int number)
        {
            if (!playerArtifacts.ContainsKey(id))
            {
                return;
            }
            // get value from artifactDataBase
            int value = dataArtifact.GetArtifactData(id).Value;

            // if number <= 0, remove from playerArtifacts
            playerArtifacts[(int)EnumArtifact.Money] += number * value;

            DeleteArtifact(id, number);
        }

        public Dictionary<int, int> GetArtifactsByType(int Type)
        {
            if (Type.Equals(Constant.Type.ARTIFACT_ALL))
            {
                return playerArtifacts;
            }

            Dictionary<int, int> targetList = new Dictionary<int, int>();

            foreach (var playerArtifact in playerArtifacts)
            {
                if (dataArtifact.GetArtifactData(playerArtifact.Key).Type == Type)
                {
                    targetList.Add(playerArtifact.Key, playerArtifact.Value);
                }
            }
            return targetList;
        }

        public List<int> GetModulesByType(int Type)
        {
            if (Type.Equals(Constant.Type.MODULE_TYPE_ALL))
            {
                return playerModules;
            }

            List<int> targetList = new List<int>();

            foreach (var playerModule in playerModules)
            {

                if (dataArtifact.GetModuleData(playerModule).Classification == Type)
                {
                    targetList.Add(playerModule);
                }
            }
            return targetList;
        }

        public List<int> GetEquippedModuleIdS()
        {
            List<int> result = new List<int>();
            foreach(var id in equippedModules)
            {
                if(id != 0)
                {
                    result.Add(id);
                }
            }

            return result;
        }

        public int[] GetEquippedModules()
        {
            return this.equippedModules;
        }

        public void EquipCurrentModule()
        {

            // before modules change
            UpdateAttrsByAllModules(Constant.Type.SUB);

            // change modules
            ArtifactModuleData moduleData = dataArtifact.GetCurrentShowModuleData();

            if(moduleData.Classification == Constant.Type.MODULE_TYPE_SUPPORT)
            {
                if (equippedModules[5] != 0)
                {
                    equippedModules[4] = equippedModules[5];
                }
                equippedModules[5] = moduleData.Id;
                return;
            }

            equippedModules[moduleData.Classification - 1] = moduleData.Id;

            dataArtifact.lockCurrentModuleID = false;

            // after modules change
            UpdateAttrsByAllModules(Constant.Type.ADD);

            GameEntry.Event.Fire(this, EquippedModuleChangesEventArgs.Create());

        }

        public void UpdateAttrsByAllModules(int Type)
        {

            foreach (var moduleId in equippedModules)
            {
                if (moduleId != 0 && dataArtifact.GetModuleData(moduleId) != null)
                {
                    int[] attrs = dataArtifact.GetModuleData(moduleId).Attributes;
                    UpdateAttributes(attrs, Type);
                }

            }

        }


        public PlayerNPCData GetNpcDataById(int NpcId)
        {
            if (!playerNPCs.ContainsKey(NpcId))
            {
                playerNPCs.Add(NpcId, new PlayerNPCData(dataNPC.GetNPCData(NpcId)));
            }

            return playerNPCs[NpcId];

        }

        public Dictionary<int, PlayerNPCData> GetPlayerNPCsData()
        {
            return this.playerNPCs;
        }

        public Dictionary<int, int> GetNpcArtifactsByNpcId(int NpcId)
        {

            if (!playerNPCs.ContainsKey(NpcId))
            {
                playerNPCs.Add(NpcId, new PlayerNPCData(dataNPC.GetNPCData(NpcId)));
            }

            PlayerNPCData npcData = playerNPCs[NpcId];

            int[] ArtifactIds = npcData.Artifacts;

            // artifactId, Num
            Dictionary<int, int> targetList = new Dictionary<int, int>();

            for (int i = 0; i < ArtifactIds.Length; i += 2)
            {
                targetList.Add(ArtifactIds[i], ArtifactIds[i+1]);

            }

            return targetList;
        }

        public void setNpcArtifactsByNpcId(int NpcId, Dictionary<int, int> npcArtifacts)
        {
            int[] newNPCArtifacts = new int[npcArtifacts.Count*2];
            int index = 0;

            foreach (KeyValuePair<int,int> kvp in npcArtifacts)
            {
                newNPCArtifacts[index] = kvp.Key;
                index++;
                newNPCArtifacts[index] = kvp.Value;
                index++;
            }
            playerNPCs[NpcId].Artifacts = newNPCArtifacts;
        }

        public int GetArtifactNumById(int id)
        {

            if (!playerArtifacts.ContainsKey(id))
            {
                return 0;
            }
            return playerArtifacts[id];
        }

        public bool SetArtifactNumById(int id,int newValue)
        {
            if (playerArtifacts.ContainsKey(id))
            {
                playerArtifacts[id] = newValue;
            }
            return false;
        }

        public void AddSkill(int id, int level)
        {
            if (!playerSkills.ContainsKey(id))
            {
                playerSkills.Add(id, 0);
            }
            playerSkills[id] = level;
        }

        public int GetSkillLevelById(int Id)
        {
            if (!playerSkills.ContainsKey(Id))
            {
                return Constant.Type.SKILL_LOCKED;
            }
            return playerSkills[Id];
        }

        public List<int> GetSkillsByFunctionality(string Type)
        {
     /*       if (Type.Equals(Constant.Type.SKILL_TYPE_ALL))
            {
                return playerSkills.Keys.ToList();
            }
*/
            List<int> targetIdList = new List<int>();
            foreach (var skillId in playerSkills.Keys)
            {
                SkillData skillData = dataSkill.GetSkillData(skillId);
                if (Type.Equals(skillData.Functionality))
                {
                    targetIdList.Add(skillId);
                }
            }
            return targetIdList;
        }
        
        public Dictionary<int, int> GetAllSkills()
        {   
            return this.playerSkills;
        }

        public void UpdateAttrsByAllSkills(int Type)
        {
           // PlayerSkillData[] playerSkillDatas  = playerSkills.Values.ToArray();

            foreach (var playerSkill in playerSkills)
            {
                int skillId = playerSkill.Key;
                int currentLevel = playerSkill.Value;

                SkillData skillData = dataSkill.GetSkillData(skillId);
                int[] attrs = skillData.GetSkillLevelData(currentLevel).Attributes;
                UpdateAttributes(attrs, Type);
            }

        }

        public void UpgradeCurrentSkill()
        {
            int currentLevel = playerSkills[dataSkill.currentSkillId];

            SkillData skillData = dataSkill.GetCurrentSkillData();

            int[] costIds = skillData.GetLevelCosts(currentLevel + 1);

            for (int i = 0; i < costIds.Length; i += 2)
            {
                playerArtifacts[costIds[i]] -= costIds[i + 1];
            }

            // before level up
            UpdateAttrsByAllSkills(Constant.Type.SUB);

            // change skills
            playerSkills[dataSkill.currentSkillId]++;

            // after level up
            UpdateAttrsByAllSkills(Constant.Type.ADD);

            //dataSkill.lockCurrentSkillID = false;
            GameEntry.Event.Fire(this, SkillUpgradedEventArgs.Create());

        }

        private void UpdateAttributes(int[] AttrIDs, int Type)
        {
            if (AttrIDs.Length <= 1)
            {
                return;
            }

                for (int i = 0; i < AttrIDs.Length; i += 2)
            {
                int AttrID = AttrIDs[i];
                int Change = AttrIDs[i + 1];
                if(Type == Constant.Type.SUB)
                {
                    Change = -Change;
                }
                UpdateAttributeByType(AttrID, Change);
            }
        }



        public void ResetSkills()
        {

            // reset calculated spaceship
            this.playerCalculatedSpaceshipData = new PlayerCalculatedSpaceshipData(initialSpaceship);

            // reset all costs consumed
            //PlayerSkillData[] playerSkillDatas =  playerSkills.Values.ToArray();

            foreach (var playerSkill in playerSkills)
            {
                int skillId = playerSkill.Key;

                int[] costs = dataSkill.GetSkillData(skillId).GetAllLevelsCosts(playerSkill.Value);

                for(int i = 0; i < costs.Length; i += 2)
                {
                    AddArtifact(costs[i], costs[i + 1]);
                }

            }

            // clear skills;
            playerSkills.Clear();

            foreach (var id in initialSpaceship.SkillIds)
            {
                AddSkill(id, 1);
            }

            // consume costs

            //playerSkillDatas = playerSkills.Values.ToArray();

            foreach (var playerSkill in playerSkills)
            {
                int skillId = playerSkill.Key;

                int[] costs = dataSkill.GetSkillData(skillId).GetAllLevelsCosts(playerSkill.Value);

                for (int i = 0; i < costs.Length; i += 2)
                {
                    DeleteArtifact(costs[i], costs[i + 1]);
                }

            }

            UpdateAttrsByAllSkills(Constant.Type.ADD);

            UpdateAttrsByAllModules(Constant.Type.ADD);

            GameEntry.Event.Fire(this, SkillUpgradedEventArgs.Create());

        }

        public int GetUnlockedSkillsNum()
        {
            return playerSkills.Count;
        }

        public int GetUnlockedLevelsNum()
        {
            int result = 0;
            foreach(var skillLevel in playerSkills.Values)
            {
                result += skillLevel;
            }
            return result;
        }

        private void AddMockData()
        {


            playerArtifacts[(int)EnumArtifact.Money] += 99999;
            playerArtifacts[(int)EnumArtifact.KnowledgePoint] += 45;


            AddArtifact((int)EnumArtifact.LowLevelUpgradeUnit, 50);
            AddArtifact((int)EnumArtifact.IntermediateUpgradeUnit, 60);
            AddArtifact((int)EnumArtifact.AdvancedUpgradeUnit, 70);

            AddArtifact((int)EnumArtifact.FirepowerModule, 1);
            AddArtifact((int)EnumArtifact.DamageAmplifier, 1);
            AddArtifact((int)EnumArtifact.MissileLauncher, 1);
            AddArtifact((int)EnumArtifact.BeamEmitter, 1);

            AddArtifact((int)EnumArtifact.RareOre, 30);
            AddArtifact((int)EnumArtifact.FuelRefillUnit, 40);

            AddArtifact((int)EnumArtifact.KnowledgeFragments_CloudComputing, 1);
            AddArtifact((int)EnumArtifact.KnowledgeFragments_AI, 1);
            AddArtifact((int)EnumArtifact.KnowledgeFragments_Blockchain, 1);
           
        }

        public Dictionary<int,int> GetPlayerAchievement()
        {
            return this.playerAchievement;
        }
        public int GetUnlockedAchievementCount()
        {
            int result = 0;
            foreach(KeyValuePair<int,int> pair in playerAchievement)
            {
                if(dataAchievement.isMaxLevel(pair.Key,pair.Value))
                {
                    result++;
                }
            }
            return result;
        }
        public int GetPlayerAchievementPoints()
        {
            int result = 0;
            foreach(KeyValuePair<int,int> pair in playerAchievement)
            {
                AchievementData achievementData = dataAchievement.GetDataById(pair.Key);
                for(int i=0;i<pair.Value;i++)
                {
                    result += achievementData.Points[i];
                }
            }
            return result;
        }
        public void UpdatePlayerAchievementData(int id,int level)
        {
            if(playerAchievement.ContainsKey(id))
            {
                playerAchievement[id] = level;
            }
            else
            {
                playerAchievement.Add(id, level);
            }
        }
        public int GetCurrentAchievementLevelById(int id)
        {
            return playerAchievement.ContainsKey(id) ? playerAchievement[id] : 0;
        }
        public bool isAchievementAchieved(int count)
        {
            return playerAchievement.ContainsKey(dataAchievement.cuurrentPopUpId) &&
       dataAchievement.GetNextLevel(dataAchievement.cuurrentPopUpId, count) == playerAchievement[dataAchievement.cuurrentPopUpId];
        }
        public int GetNextLevel(int Id)
        {
            AchievementData achievementData = dataAchievement.GetDataById(Id);
            if (achievementData == null)
            {
                return 0;
            }

            if (!playerAchievement.ContainsKey(Id))
            {
                return 1;
            }
            else
            {
                return dataAchievement.isMaxLevel(Id, playerAchievement[Id]) ? playerAchievement[Id] : playerAchievement[Id] + 1;
            }
        }
    }

}

