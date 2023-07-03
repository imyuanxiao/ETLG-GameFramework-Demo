
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine;

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

        // StarCoins
        public int starCoins;

        // KnowlegdePoints
        public int knowledgePoints;

        // Player Artifact Data, get artifacts by type

        private Dictionary<int, PlayerArtifactData> playerArtifacts = new Dictionary<int, PlayerArtifactData>();

        private DataArtifact dataArtifact = GameEntry.Data.GetData<DataArtifact>();

        // Player Skill Data
        private Dictionary<int, PlayerSkillData> playerSkills = new Dictionary<int, PlayerSkillData>();
        private DataSkill dataSkill = GameEntry.Data.GetData<DataSkill>();


        //构造函数，新建玩家数据
        public PlayerData (SpaceshipData spaceshipData)
        {
            this.initialSpaceship = spaceshipData;
            this.playerCalculatedSpaceshipData = new PlayerCalculatedSpaceshipData(spaceshipData);


            addMockArtifactData();

        }

        // Call this method everytime skills change or initialSpaceship changes
        public void UpdatePlayerCalculatedSpaceshipData()
        {
            

        }

        public void addArtifact(int id, int number)
        {
            if(!playerArtifacts.ContainsKey(id))
            {
                playerArtifacts.Add(id, new PlayerArtifactData(dataArtifact.GetArtifactData(id)));
            }
            playerArtifacts[id].Number += number;

        }

        public void sellArtifact(int id, int number)
        {
            // get value from artifactDataBase
            int value = dataArtifact.GetArtifactData(id).Value;

            // if number <= 0, remove from playerArtifacts
            playerArtifacts[id].Number -= number;

            this.starCoins += value;

            if (playerArtifacts[id].Number <= 0)
            {
                playerArtifacts.Remove(id);
            }

        }

        public List<PlayerArtifactData> getArtifactsByType(string type)
        {
            if (type.Equals("all"))
            {

                return playerArtifacts.Values.ToList();
            }

            List< PlayerArtifactData> targetList = new List<PlayerArtifactData>(); 
            foreach (var playerArtifact in playerArtifacts.Values)
            {
                if (playerArtifact.Type.Equals(type))
                {
                    targetList.Add(playerArtifact); 
                }
            }
            return targetList;
        }


        public PlayerArtifactData getArtifactDataById(int id)
        {
            return playerArtifacts[id];
        }

        public void setSkill(int id, int level)
        {
            if (!playerSkills.ContainsKey(id))
            {
                playerSkills.Add(id, new PlayerSkillData(dataSkill.GetSkillData(id)));
            }
            playerSkills[id].level = level;
        }

        public List<PlayerSkillData> getSkillsByType(string type)
        {

            List<PlayerSkillData> targetList = new List<PlayerSkillData>();
            foreach (var playerSkill in playerSkills.Values)
            {
                if (playerSkill.IsActiveSkill)
                {
                    if (type.Equals("combat") && playerSkill.IsCombatSkill)
                    {
                        targetList.Add(playerSkill);
                    }
                    if (type.Equals("explore") && !playerSkill.IsCombatSkill)
                    {
                        targetList.Add(playerSkill);
                    }
                }
            }
            return targetList;
        }

        private void addMockArtifactData()
        {
            DataArtifact dataArtifact = GameEntry.Data.GetData<DataArtifact>();

            addArtifact(3001, 1);
            addArtifact(3002, 1);
            addArtifact(3003, 1);
            addArtifact(1005, 100);
            addArtifact(1006, 200);
            addArtifact(2001, 1);
            addArtifact(2002, 1);
            addArtifact(2003, 1);
            addArtifact(2004, 1);
            addArtifact(2005, 1);
            addArtifact(2006, 1);


        }

    }




}

