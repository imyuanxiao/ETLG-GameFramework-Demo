
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Networking.Types;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

namespace ETLG.Data
{
    public sealed class PlayerData
    {
        // Initial SpaceshipData(include model asset)
        public SpaceshipData initialSpaceship { get; set; }

        // Spaceship attributes increased by skills, etc. (for display and calculation)
        public PlayerCalculatedSpaceshipData playerCalculatedSpaceshipData { get; set; }

        // Player position
        //public Vector3 position { get; set; }

        // player artifacts - ID, Number
        private Dictionary<int, int> playerArtifacts { get; set; }
        private List<int> playerModules { get; set; }

        public List<int> PlayedTutorialGroup { get; set; }

        private int[] equippedModules { get; set; }

        private DataArtifact dataArtifact { get; set; }

        private DataLandingPoint dataLandingPoint;
        private DataPlanet dataPlanet;

        // Player Skill - Skill ID, Skill Level,
        // unexists = locked, level = 0 = unlocked, >= 1 = upgraded 
        private Dictionary<int, int> playerSkills { get; set; }

        private DataSkill dataSkill { get; set; }

        //Player NPC Data
        private Dictionary<int, PlayerNPCData> playerNPCs { get; set; }

        private DataNPC dataNPC { get; set; }

        //Player Achievement Data
        private Dictionary<int, int> playerAchievement { get; set; }
        public Dictionary<int, int> playerTotalArtifacts { get; set; }
        private DataAchievement dataAchievement { get; set; }

        public int battleVictoryCount;
        public float[] bossDefeatTime;

        //Player dialog and quiz Data
        private Dictionary<int, UINPCDialogManager> playerDialogs = new Dictionary<int, UINPCDialogManager>();
        private Dictionary<int, UIQuizManager> playerQuizes = new Dictionary<int, UIQuizManager>();
        //Chapter
        public Dictionary<int, bool> ChaptersSaveData;
        //0 correct 1 wrong
        public Dictionary<int, int[]> QuizesSaveData;
        //Courses
        public Dictionary<int, float> CoursesSaveData;
        //Domians
        public Dictionary<int, float> DomiansSaveData;
        //Learning Path
        private DataLearningPath dataLearningPath { get; set; }

        public int[] totalQuizResults = { 0, 0 };
        //learn achievement record
        // private Dictionary<int, int> AchiLearnRecord = new Dictionary<int, int>();

        public int passedQuiz = 0;
        public int finishedDialog = 0;

        public PlayerData(SpaceshipData spaceshipData)
        {
            initialSpaceship = spaceshipData;
            playerCalculatedSpaceshipData = new PlayerCalculatedSpaceshipData(spaceshipData);

            // initialize attrs
            dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataNPC = GameEntry.Data.GetData<DataNPC>();
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            dataLearningPath = GameEntry.Data.GetData<DataLearningPath>();

            playerArtifacts = new Dictionary<int, int>(); // id + number
            playerModules = new List<int>(); // 
            PlayedTutorialGroup = new List<int>();

            playerSkills = new Dictionary<int, int>();

            playerNPCs = new Dictionary<int, PlayerNPCData>();
            instantiatePlayerNPCs();

            playerAchievement = new Dictionary<int, int>(); // id + level
            playerTotalArtifacts = new Dictionary<int, int>();

            // player can only equip 6 module, 0-weapon, 1-attack, 2-defense, 3-powerdrive, 4- support, 6-support
            equippedModules = new int[6];

            // 0-cloud computing, 1-CyberSecurity, 2-Data science, 3-AI, 4-blockchain, 5-IoT, 6-final boss
            bossDefeatTime = new float[7] { -1f, -1f, -1f, -1f, -1f, -1f, -1f };

            // Learning progress init
            ChaptersSaveData = new Dictionary<int, bool>();
            CoursesSaveData = new Dictionary<int, float>();
            DomiansSaveData = new Dictionary<int, float>();
            QuizesSaveData = new Dictionary<int, int[]>();
            KnowledgeSavedData();

            // add initial skills
            foreach (var id in spaceshipData.SkillIds)
            {
                AddSkill(id, 1);

            }

            // add money and skill points
            playerArtifacts.Add((int)EnumArtifact.Money, 5000);
            playerArtifacts.Add((int)EnumArtifact.KnowledgePoint, 0);

            playerTotalArtifacts.Add((int)EnumArtifact.Money, 5000);
            playerTotalArtifacts.Add((int)EnumArtifact.KnowledgePoint, 0);
            playerTotalArtifacts.Add(Constant.Type.ACHIV_TOTAL_SPEND_MONEY, 0);

            // add mock artifacts
            AddMockData();
            //initPlayerAchievementData();

            battleVictoryCount = 0;

            UpdateAttrsByAllSkills(Constant.Type.ADD);
        }

        private void KnowledgeSavedData()
        {
            dataPlanet = GameEntry.Data.GetData<DataPlanet>();
            int[] planets = dataPlanet.GetAllPlanetIDs();
            foreach (int planetID in planets)
            {
                DomiansSaveData.Add(planetID, 0f);
            }

            dataLandingPoint = GameEntry.Data.GetData<DataLandingPoint>();
            int[] DataLandingPoints = dataLandingPoint.GetAlLandingPointsIDs();
            foreach (int landingPointID in DataLandingPoints)
            {
                CoursesSaveData.Add(landingPointID, 0f);
            }

            List<int> NPCIDs = dataNPC.getAllNPCsID();
            foreach (int NPCId in NPCIDs)
            {
                ChaptersSaveData.Add(NPCId, false);
                int[] emptyResults = { 0, 0 };
                QuizesSaveData.Add(NPCId, emptyResults);
            }
        }

        private void instantiatePlayerNPCs()
        {
            foreach (int id in dataNPC.getAllNPCsID())
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
                case Constant.Type.ATTR_Firerate:
                    newValue = playerCalculatedSpaceshipData.FireRate;
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
                case Constant.Type.ATTR_Firerate:
                    playerCalculatedSpaceshipData.FireRate = newValue;
                    break;
            }

            //update achievements
            if(playerCalculatedSpaceshipData.Durability == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4002, 500));
            }
            if (playerCalculatedSpaceshipData.Shields == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4003, 500));
            }
            if (playerCalculatedSpaceshipData.Firepower == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4004, 500));
            }
            if (playerCalculatedSpaceshipData.Energy == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4001, 500));
            }
            if (playerCalculatedSpaceshipData.Agility == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4005, 500));
            }
            if (playerCalculatedSpaceshipData.FireRate == Constant.Type.ATTR_MAX_VALUE)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4006, 500));
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
            sum += playerCalculatedSpaceshipData.FireRate;
            return sum;
        }

        public float GetPlayerScore()
        {
            float sum = 0f;
            sum += GetSpaceshipScore();
            sum += GetSkillScore();
            sum += GetModuleScore();
            return sum;
        }

        public float GetSkillScore()
        {
            float sum = 0f;
            sum += 50 * GetUnlockedLevelsNum();
            sum += 50 * GetUnlockedSkillsNum();
            return sum;
        }

        public float GetModuleScore()
        {
            float sum = 0f;
            sum += 200 * playerModules.Count;
            return sum;
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
                case Constant.Type.ATTR_Firerate:
                    return playerCalculatedSpaceshipData.FireRate;
            }
            return 0f;
        }
        public void AddArtifact(int id, int number)
        {
            // module -> playerModules
            if (dataArtifact.GetArtifactData(id) is ArtifactModuleData)
            {
                if (!playerModules.Contains(id))
                {
                    playerModules.Add(id);
                }
                else
                {
                    playerModules.Remove(id);
                }
                return;
            }
            // artifacts -> playerArtifacts
            if (!playerArtifacts.ContainsKey(id))
            {
                playerArtifacts.Add(id, number);
            }
            else
            {
                playerArtifacts[id] += number;
            }

            //total artifacts for achievement
            if (dataAchievement.isReset || (number <= 0 && id != (int)EnumArtifact.Money))
            {
                return;
            }

            AddTotalArtifact(id, number);

        }
        public void AddTotalArtifact(int id, int number)
        {
            if (!playerTotalArtifacts.ContainsKey(id))
            {
                //if money number<0,add playerTotalArtifacts total spend money 
                if (id == (int)EnumArtifact.Money && number < 0)
                {
                    playerTotalArtifacts.Add(Constant.Type.ACHIV_TOTAL_SPEND_MONEY, -number);
                }
                else
                {
                    playerTotalArtifacts.Add(id, number);
                }
            }
            else
            {
                //if money number<0,add playerTotalArtifacts total spend money 
                if (id == (int)EnumArtifact.Money && number < 0)
                {
                    playerTotalArtifacts[Constant.Type.ACHIV_TOTAL_SPEND_MONEY] += -number;
                }
                else
                {
                    playerTotalArtifacts[id] += number;
                }
            }

            UpdateArtifactAchievements();
        }
        //update ALL artifacts after trading
        public void updateArtifact(Dictionary<int, int> newPlayerArtifacts)
        {
            foreach (KeyValuePair<int, int> kvp in newPlayerArtifacts)
            {
                int id = kvp.Key;
                int number = kvp.Value;

                // module
                if (dataArtifact.GetArtifactData(id) is ArtifactModuleData)
                {
                    if (number <= 0 && playerModules.Contains(id))
                    {
                        playerModules.Remove(id);
                        // if equipped module, remove
                        for (int i = 0; i < equippedModules.Length; i++)
                        {
                            if (equippedModules[i] == id) equippedModules[i] = 0;
                        }

                    }
                    else if (!playerModules.Contains(id))
                    {
                        playerModules.Add(id);
                    }
                }
                // artifact
                else
                {
                    if (number <= 0 && playerArtifacts.ContainsKey(id))
                    {
                        playerArtifacts.Remove(id);
                    }
                    else
                    {
                        if (!playerArtifacts.ContainsKey(id))
                        {
                            AddArtifact(id, number);
                        }
                        else
                        {
                            AddArtifact(id, number - playerArtifacts[id]);
                        }
                    }
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

            //handle total spend money
            if (dataAchievement.isReset)
            {
                //total spand money
                //sub totoal spend
                if (id == (int)EnumArtifact.Money && playerTotalArtifacts[Constant.Type.ACHIV_TOTAL_SPEND_MONEY] > 0)
                {
                    playerTotalArtifacts[Constant.Type.ACHIV_TOTAL_SPEND_MONEY] -= number;
                }
            }
            else
            {
                if (id == (int)EnumArtifact.Money)
                {
                    playerTotalArtifacts[Constant.Type.ACHIV_TOTAL_SPEND_MONEY] += number;
                }
            }
            if (dataAchievement.isReset)
            {
                return;
            }
            UpdateArtifactAchievements();
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

        public Dictionary<int, int> GetTradeableArtifacts()
        {
            Dictionary<int, int> targetList = GetArtifactsByType(Constant.Type.ARTIFACT_TRADE);
            foreach (var module in playerModules)
            {
                if (!targetList.ContainsKey(module))
                    targetList.Add(module, 1);
            }
            return targetList;
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

        public int[] GetEquippedModuleIdS()
        {
            return equippedModules;
            /*            List<int> result = new List<int>();
                        foreach(var id in equippedModules)
                        {
                            if(id != 0)
                            {
                                result.Add(id);
                            }
                        }
                        return result;*/
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

            if (moduleData.Classification == Constant.Type.MODULE_TYPE_SUPPORT)
            {
                if (equippedModules[5] != 0)
                {
                    equippedModules[4] = equippedModules[5];
                }
                equippedModules[5] = moduleData.Id;
            }
            else
            {
                equippedModules[moduleData.Classification - 1] = moduleData.Id;
            }
            dataArtifact.lockCurrentModuleID = false;

            // after modules change
            UpdateAttrsByAllModules(Constant.Type.ADD);

            GameEntry.Event.Fire(this, EquippedModuleChangesEventArgs.Create());

            //modules achievement 
            if(playerModules.Count == dataArtifact.AllModuleCount)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4010,1));
            }
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
                targetList.Add(ArtifactIds[i], ArtifactIds[i + 1]);

            }

            return targetList;
        }

        public void setNpcArtifactsByNpcId(int NpcId, Dictionary<int, int> npcArtifacts)
        {
            // remove unnecessary artifacts
            List<int> keysToRemove = new List<int>();
            foreach (KeyValuePair<int, int> kvp in npcArtifacts)
            {
                if (kvp.Value <= 0)
                {
                    keysToRemove.Add(kvp.Key);
                };
            }
            foreach (int key in keysToRemove)
            {
                npcArtifacts.Remove(key);
            }
            // update npc artifacts
            int[] newNPCArtifacts = new int[npcArtifacts.Count * 2];
            int index = 0;
            foreach (KeyValuePair<int, int> kvp in npcArtifacts)
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
                if (playerModules.Contains(id))
                {
                    return 1;
                }
                return 0;
            }
            return playerArtifacts[id];
        }

        public bool SetArtifactNumById(int id, int newValue)
        {
            if (playerArtifacts.ContainsKey(id))
            {
                int preValue = playerArtifacts[id];
                playerArtifacts[id] = newValue;

                //set total got/spent money
                AddTotalArtifact(id, newValue - preValue);
            }
            return false;
        }

        public void AddSkill(int id, int level)
        {
            for (int i = 0; i <= level; i++)
            {
                AddSkill(id);
            }
        }
        public void AddSkill(int id)
        {
            if (!playerSkills.ContainsKey(id))
            {
                playerSkills.Add(id, 0);
            }
            else
            {
                int maxLevel = dataSkill.GetSkillData(id).Levels.Length;
                playerSkills[id]++;
                if (playerSkills[id] > maxLevel)
                {
                    playerSkills[id] = maxLevel;

                }
            }

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

                if (currentLevel <= 0) continue;

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
            
            //update skill achievement
            //if unlock all skills
            if(GetUnlockedSkillsNum() == dataSkill.skillCount)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4009, 1));
            }
            //if all skills are max level
            if (GetUnlockedSkillsNum() == dataSkill.skillCount && GetUnlockedLevelsNum() == dataSkill.levelCount)
            {
                GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(4008, 1));
            }
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
                if (Type == Constant.Type.SUB)
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
            dataAchievement.isReset = true;
            foreach (var playerSkill in playerSkills)
            {
                int skillId = playerSkill.Key;

                if(playerSkill.Value <= 0)
                {
                    continue;
                }

                int[] costs = dataSkill.GetSkillData(skillId).GetAllLevelsCosts(playerSkill.Value);

                for (int i = 0; i < costs.Length; i += 2)
                {
                    AddArtifact(costs[i], costs[i + 1]);
                }

            }

            List<int> UnlockedSkills = new List<int>();
            foreach (var playerSkill in playerSkills)
            {
                UnlockedSkills.Add(playerSkill.Key);
            }

            // clear skills;
            playerSkills.Clear();

            // add all unlocked skill

            foreach (var skillId in UnlockedSkills)
            {
                AddSkill(skillId);
            }

            // add initial spaceship skill
            foreach (var id in initialSpaceship.SkillIds)
            {
                AddSkill(id);
            }

            // consume costs

            //playerSkillDatas = playerSkills.Values.ToArray();

            foreach (var playerSkill in playerSkills)
            {
                int skillId = playerSkill.Key;
                int level = playerSkill.Value;
                if (level <= 0) continue;

                int[] costs = dataSkill.GetSkillData(skillId).GetAllLevelsCosts(level);

                for (int i = 0; i < costs.Length; i += 2)
                {
                    DeleteArtifact(costs[i], costs[i + 1]);
                }

            }
            dataAchievement.isReset = false;
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
            foreach (var skillLevel in playerSkills.Values)
            {
                result += skillLevel;
            }
            return result;
        }

        private void AddMockData()
        {

            // add artifacts
            List<int> artifactsIds = new List<int>
            {
                1001,1002,1003, 1004, 1101, 1102, 1103
            };
            foreach (var id in artifactsIds)
            {
                AddArtifact(id, 30);
            }


            // add skills
            List<int> skillIds = new List<int>
            {
                101, 102, 201, 202, 203, 301, 302, 303
            };
/*            List<int> skillIds = new List<int>
            {
                101, 102, 201, 202, 203, 301, 302, 303,
                401, 402, 403, 501, 502, 503, 601, 602,
                603, 701, 702, 703
            };*/
            foreach (var id in skillIds)
            {
                AddSkill(id);
            }

            // add modules
            List<int> moduleIds = new List<int>
            {
                3001, 3002, 3005, 3006,  3009, 3010,
                3011, 3012, 3013, 3014, 3017, 3018, 3019
            };

            foreach (var id in moduleIds)
            {
                AddArtifact(id, 1);
            }

        }

        public Dictionary<int, int> GetPlayerAchievement()
        {
            return this.playerAchievement;
        }
        public int GetUnlockedAchievementCount()
        {
            int result = 0;
            foreach (KeyValuePair<int, int> pair in playerAchievement)
            {
                if (dataAchievement.isMaxLevel(pair.Key, pair.Value))
                {
                    result++;
                }
            }
            return result;
        }
        public int GetPlayerAchievementPoints()
        {
            int result = 0;
            foreach (KeyValuePair<int, int> pair in playerAchievement)
            {
                AchievementData achievementData = dataAchievement.GetDataById(pair.Key);
                for (int i = 0; i < pair.Value; i++)
                {
                    result += achievementData.Points[i];
                }
            }
            return result;
        }
        public void UpdatePlayerAchievementData(int id, int level)
        {
            if (playerAchievement.ContainsKey(id))
            {
                playerAchievement[id] = level - 1;
            }
            else
            {
                playerAchievement.Add(id, level - 1);
            }
        }
        public int GetCurrentAchievementLevelById(int id)
        {
            return playerAchievement.ContainsKey(id) ? playerAchievement[id] : 0;
        }
        public bool isAchievementShouldAchieved(int id,int count)
        {
            AchievementData achievementData = dataAchievement.GetDataById(id);
            if(!playerAchievement.ContainsKey(id))
            {
                if(achievementData.Count[0]<=count)
                {
                    return true;
                }
                return false;
            }
            return !isAchievementAchieved(id, count);


        }
        public bool isAchievementAchieved(int id, int count)
        {
            if (!playerAchievement.ContainsKey(id))
            {
                return false;
            }
            if (playerAchievement[id] >= dataAchievement.GetNextLevel(id, count) - 1)
            {
                return true;
            }
            return false;
        }

        public int GetNextLevel(int Id)
        {
            if (!playerAchievement.ContainsKey(Id))
            {
                return 0;
            }
            else
            {
                return dataAchievement.isMaxLevel(Id, playerAchievement[Id]) ? playerAchievement[Id] : playerAchievement[Id] + 1;
            }
        }

        public void UpdateArtifactAchievements()
        {
            int number, achievementId = 0;
            int[] counts;
            int[] artifactIds = {
                                   (int)EnumArtifact.Money,
                                   Constant.Type.ACHIV_TOTAL_SPEND_MONEY,
                                   (int)EnumArtifact.KnowledgePoint,
                                   (int)EnumArtifact.RareOre,
                                   (int)EnumArtifact.FuelRefillUnit
                                 };

            foreach (int artifactId in artifactIds)
            {
                if (playerTotalArtifacts.ContainsKey(artifactId))
                {
                    if (artifactId == (int)EnumArtifact.Money)
                    {
                        achievementId = 5001;
                    }
                    else if (artifactId == Constant.Type.ACHIV_TOTAL_SPEND_MONEY)
                    {
                        achievementId = 5002;
                    }
                    else if (artifactId == (int)EnumArtifact.KnowledgePoint)
                    {
                        achievementId = 5004;
                    }
                    else if (artifactId == (int)EnumArtifact.RareOre)
                    {
                        achievementId = 5005;
                    }
                    else if (artifactId == (int)EnumArtifact.FuelRefillUnit)
                    {
                        achievementId = 5006;
                    }

                    number = playerTotalArtifacts[artifactId];
                    counts = dataAchievement.GetDataById(achievementId).Count;

                    foreach (int count in counts)
                    {
                        if (number >= count && !isAchievementAchieved(achievementId, number))
                        {
                            GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                        }
                    }
                }
            }

            // Total fragments
            achievementId = 5003;
            int[] fragmentsId = {
                                    (int)EnumArtifact.KnowledgeFragments_AI,
                                    (int)EnumArtifact.KnowledgeFragments_Blockchain,
                                    (int)EnumArtifact.KnowledgeFragments_CloudComputing,
                                    (int)EnumArtifact.KnowledgeFragments_Cybersecurity,
                                    (int)EnumArtifact.KnowledgeFragments_DataScience,
                                    (int)EnumArtifact.KnowledgeFragments_IoT
                                };
            number = 0;

            foreach (int fragmentId in fragmentsId)
            {
                if (playerTotalArtifacts.ContainsKey(fragmentId))
                {
                    number += playerTotalArtifacts[fragmentId];
                }
            }

            counts = dataAchievement.GetDataById(achievementId).Count;
            foreach (int count in counts)
            {
                if (number >= count && !isAchievementAchieved(achievementId, count))
                {

                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                }
            }
        }

        public UINPCDialogManager getUINPCDialogById(int NPCId)
        {
            if (playerDialogs.ContainsKey(NPCId))
            {
                UINPCDialogManager foundDialogManager = playerDialogs[NPCId];
                return foundDialogManager;
            }
            return null;
        }

        public UIQuizManager getUIQuizManager(int NPCId)
        {
            if (playerQuizes.ContainsKey(NPCId))
            {
                UIQuizManager found1QuizManager = playerQuizes[NPCId];
                return found1QuizManager;
            }
            return null;
        }

        public void setUINPCDialogById(int NPCId, UINPCDialogManager UINPCDialogManager)
        {
            if (playerDialogs.ContainsKey(NPCId))
            {
                playerDialogs[NPCId] = UINPCDialogManager;
            }
            else
            {
                playerDialogs.Add(NPCId, UINPCDialogManager);
            }
        }

        public void setUIQuizManagerById(int NPCId, UIQuizManager UIQuizManager)
        {
            if (playerQuizes.ContainsKey(NPCId))
            {
                playerQuizes[NPCId] = UIQuizManager;
            }
            else
            {
                playerQuizes.Add(NPCId, UIQuizManager);
            }
        }

        public DataLearningPath getLearningPath()
        {
            return dataLearningPath;
        }

        public void setFinishChapter(int NPCID)
        {
            ChaptersSaveData[NPCID] = true;
            calcCourseProgress();
            calcDomianProgress();
        }

        public void calcCourseProgress()
        {
            Dictionary<int, float> tempSaveData = new Dictionary<int, float>();

            foreach (int landingPointId in CoursesSaveData.Keys)
            {
                int[] chapters = getChaptersIntArray(landingPointId);
                int totalPassedChapters = chapters[0];
                int totalChapters = chapters[1];
                float progress = (float)totalPassedChapters / (float)totalChapters;
                tempSaveData[landingPointId] = progress;
            }

            foreach (var kvp in tempSaveData)
            {
                CoursesSaveData[kvp.Key] = kvp.Value;
            }
        }

        private int[] getChaptersIntArray(int landingPointId)
        {
            //0 passed 1 total
            int[] chapters = { 0, 0 };
            LandingpointData lpData = dataLandingPoint.GetLandingPointData(landingPointId);
            foreach (int NPCId in lpData.NPCsID)
            {
                bool result = ChaptersSaveData[NPCId];
                if (result)
                {
                    chapters[0]++;
                }
                chapters[1]++;
            }
            return chapters;
        }

        public void calcDomianProgress()
        {
            Dictionary<int, float> tempSaveData = new Dictionary<int, float>();

            foreach (int planetId in DomiansSaveData.Keys)
            {
                int totalChapters = 0;
                int totalPassedChapters = 0;
                PlanetData planetData = dataPlanet.GetPlanetData(planetId);
                foreach (int lpdata in planetData.LandingPoints)
                {
                    int[] chapters = getChaptersIntArray(lpdata);
                    totalChapters += chapters[1];
                    totalPassedChapters += chapters[0];
                }
                float progress = (float)totalPassedChapters / (float)totalChapters;
                tempSaveData[planetId] = progress;
            }

            foreach (var kvp in tempSaveData)
            {
                DomiansSaveData[kvp.Key] = kvp.Value;
            }
            updateAchievement_DomianFinish();
        }
        private void updateAchievement_DomianFinish()
        {
            foreach(KeyValuePair<int,float> kvp in DomiansSaveData)
            {
                int count = 1;
                if (kvp.Value == 1f && !isAchievementAchieved(getDomainAchievementId(kvp.Key), count))
                {
                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(getDomainAchievementId(kvp.Key), count));
                }
            }
        }

        private int getDomainAchievementId(int planetId)
        {
            int achievementId;
            switch (planetId)
            {
                case Constant.Type.DOMAIN_CLOUD_COMPUTING:
                    achievementId= Constant.Type.ACHV_DOMIAN_CLOUD;
                    break;
                case Constant.Type.DOMAIN_ARTIFICIAL_INTELLIGENCE:
                    achievementId = Constant.Type.ACHV_DOMIAN_AI;
                    break;
                case Constant.Type.DOMAIN_CYBERSECURITY:
                    achievementId = Constant.Type.ACHV_DOMIAN_CYBERS;
                    break;
                case Constant.Type.DOMAIN_BLOCKCHAIN:
                    achievementId = Constant.Type.ACHV_DOMIAN_BC;
                    break;
                case Constant.Type.DOMAIN_DATA_SCIENCE:
                    achievementId = Constant.Type.ACHV_DOMIAN_DS;
                    break;
                default:
                    achievementId = Constant.Type.ACHV_DOMIAN_IOT;
                    break;
            }
            return achievementId;
        }

        public bool getChapterFinish(int NPCID)
        {
            return ChaptersSaveData[NPCID];
        }

        public void addQuizResult(int NPCId, bool correct)
        {
            if (correct)
            {
                QuizesSaveData[NPCId][0]++;
                getCorrectWrongQuiz();
                updateAchievement_CorrectNumber();
            }
            else
            {
                QuizesSaveData[NPCId][1]++;
                getCorrectWrongQuiz();
                updateAchievement_WrongNumber();
            }
        }

        public int getTotalPassChapterQuiz()
        {
            int totalPassChapterQuiz = 0;
            foreach (bool pass in ChaptersSaveData.Values)
            {
                if (pass)
                {
                    totalPassChapterQuiz++;
                }
            }
            return totalPassChapterQuiz;
        }

        public void getCorrectWrongQuiz()
        {
            int[] newEmpty = { 0, 0 };
            totalQuizResults = newEmpty;
            foreach (int[] result in QuizesSaveData.Values)
            {
                totalQuizResults[0] += result[0];
                totalQuizResults[1] += result[1];
            }
        }

        public void updateAchievement_CorrectNumber()
        {
            int achievementId = Constant.Type.ACHV_CORRECTQUIZ;
            //0 correct 1 wrong
            int[] counts = dataAchievement.GetDataById(achievementId).Count;
            foreach (int count in counts)
            {
                if (totalQuizResults[0] >= count && !isAchievementAchieved(achievementId, totalQuizResults[0]))
                {
                    Debug.Log("count"+count+"correct"+ totalQuizResults[0]);
                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                }
            }
        }

        public void updateAchievement_WrongNumber()
        {
            int achievementId = Constant.Type.ACHV_WRONGQUIZ;
            //0 correct 1 wrong
            int[] counts = dataAchievement.GetDataById(achievementId).Count;
            foreach (int count in counts)
            {
                if (totalQuizResults[1] >= count && !isAchievementAchieved(achievementId, totalQuizResults[1]))
                {
                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                }
            }
            
        }

        public void getPassQuizAndFinishDialog()
        {
            finishedDialog = 0;
            passedQuiz = 0;
            foreach (int NPCId in ChaptersSaveData.Keys)
            {
                if (ChaptersSaveData[NPCId] && dataNPC.GetNPCData(NPCId).Type == Constant.Type.NPC_TYPE_TEACHER)
                {
                    finishedDialog++;
                }
                else if (ChaptersSaveData[NPCId] && dataNPC.GetNPCData(NPCId).Type == Constant.Type.NPC_TYPE_EXAMINER)
                {
                    passedQuiz++;
                }
            }
        }

        public void updateAchievement_QuizNumber()
        {
            int achievementId = Constant.Type.ACHV_PASSEDQUIZ;
            int[] counts = dataAchievement.GetDataById(achievementId).Count;
            foreach (int count in counts)
            {
                if (passedQuiz >= count && !isAchievementAchieved(achievementId, passedQuiz))
                {
                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                }
            }
        }

        public void updateAchievement_DialogNumber()
        {
            int achievementId = Constant.Type.ACHV_FINISHEDDIALOG;
            int[] counts = dataAchievement.GetDataById(achievementId).Count;
            foreach (int count in counts)
            {
                if (finishedDialog >= count && !isAchievementAchieved(achievementId, finishedDialog))
                {
                    GameEntry.Event.Fire(this, AchievementPopUpEventArgs.Create(achievementId, count));
                }
            }
        }

        public float getTotalProgress()
        {
            float totalProgress = getTotalPassChapterQuiz() / (float)ChaptersSaveData.Count;
            return totalProgress;
        }

        public bool testPassAllQuiz()
        {
            foreach(int chapter in ChaptersSaveData.Keys)
            {
                if (dataNPC.GetNPCData(chapter).Type==Constant.Type.NPC_TYPE_EXAMINER&& !ChaptersSaveData[chapter])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

