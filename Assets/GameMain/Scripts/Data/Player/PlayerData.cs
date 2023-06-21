
using System.Collections.Generic;

namespace ETLG.Data
{
  public sealed class PlayerData
    {

        // 玩家选择的初始飞船（这个数据包含模型路径，玩家在游戏里是以飞船的形象显示的）
        public SpaceshipData initialSpaceship { get;  set; }

        // 经过技能等增幅的飞船属性（用于展示和计算）
        public SpaceshipData calculatedSpaceship { get; set; }

        // 玩家显示的飞船模型，应该只需要飞船模型信息以及位置信息
        public EntityDataSpaceship spaceship { get;  set; }

        // 玩家所拥有的技能
        public Dictionary<int, PlayerData> dicSkillDatas;
        
        // 根据玩家所拥有知识点、货币等资源
        public int skillPoints { get;  set; }
        
        //构造函数，新建玩家数据
        public PlayerData (SpaceshipData spaceshipData)
        {
            this.initialSpaceship = spaceshipData;
            this.calculatedSpaceship = spaceshipData;
        }


        // 每次技能、buff、被攻击等，就应该计算需要显示的飞船属性，可以分成好几个方法
        public void CalculateStats()
        {
            // 计算过程

            // 此处应该是等于计算后的飞船数据，需要修改
            calculatedSpaceship = initialSpaceship;
        } 


    }

}

