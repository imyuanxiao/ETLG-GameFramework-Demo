using ETLG.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UISpaceshipCheckForm : UGuiFormEx
    {

        public Button skillTreeButton;

        // 需要挂载后添加的按钮和文本等对象
        public Button returnButton;

        // name and description
        public TextMeshProUGUI s_name = null;

        // type and size
        public TextMeshProUGUI s_type = null;
        public TextMeshProUGUI s_size = null;

        // initial attrs
        public TextMeshProUGUI s_energy = null;
        public TextMeshProUGUI s_durability = null;
        public TextMeshProUGUI s_shields = null;
        public TextMeshProUGUI s_firepower = null;
        public TextMeshProUGUI s_fireRate = null;
        public TextMeshProUGUI s_agility = null;
        public TextMeshProUGUI s_speed = null;
        public TextMeshProUGUI s_dogde = null;
        public TextMeshProUGUI s_detection = null;
        public TextMeshProUGUI s_capacity = null;
        
        private readonly float valueBarMaxWidth = 230;
        private readonly float maxAttrValue = 300;

        public GameObject s_energy_valueBar = null;
        public GameObject s_durability_valueBar = null;
        public GameObject s_shields_valueBar = null;
        public GameObject s_firepower_valueBar = null;
        public GameObject s_agility_valueBar = null;
        public GameObject s_speed_valueBar = null;
        public GameObject s_detection_valueBar = null;
        public GameObject s_capacity_valueBar = null;

        public Transform artifactContainer = null;

        // 需要通过玩家数据管理器获取当前飞船数据
        private DataPlayer dataPlayer = null;

        // 当前展示的飞船信息
        private SpaceshipData currentSpaceshipData = null;

        private EntitySpaceshipSelect showSpaceshipEntity = null;

        // 实体加载器
        private EntityLoader entityLoader;


        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            // 绑定按钮点击事件
            skillTreeButton.onClick.AddListener(OnSkillTreeButtonClick);

            returnButton.onClick.AddListener(OnReturnButtonClick);

            // 获取玩家数据管理器
            dataPlayer = GameEntry.Data.GetData < DataPlayer>();

            // 初始化实体加载类
            entityLoader = EntityLoader.Create(this);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            // 调用显示飞船方法
            ShowSpaceshipSelect();

            // 调用显示道具方法
            ShowArtifactIcons(artifactContainer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            HideSpaceship();
        }

        private void OnSkillTreeButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);
            GameEntry.Event.Fire(this, SkillTreeEventArgs.Create());

        }

        private void OnApplyButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // save new spaceship to player data
            
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("Return to Map");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }



        private void ShowArtifactIcons(Transform container)
        {

            // 此处应该加载玩家拥有的道具信息，目前手动添加一些道具
            DataArtifact dataArtifact = GameEntry.Data.GetData<DataArtifact>();
            List<ArtifactDataBase> playersArtifacts = new List<ArtifactDataBase>();
            playersArtifacts.Add(dataArtifact.GetArtifactData(1001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3003));
            playersArtifacts.Add(dataArtifact.GetArtifactData(1001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3003));
            playersArtifacts.Add(dataArtifact.GetArtifactData(1001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3001));
            playersArtifacts.Add(dataArtifact.GetArtifactData(3003));

            /*
                        List<ArtifactDataBase> artifactDataBases = new List<ArtifactDataBase>();

                        foreach(ArtifactDataBase playersArtifact in playersArtifacts)
                        {
                            artifactDataBases.Add(playersArtifact);
                        }
            */
            // 四个一行，每个向右偏移 150f，每行向下偏移150f


            /*         int line = 0;
                     int index = 0;

                     foreach (var playersArtifact in playersArtifacts)
                     {
                         Vector3 offset = new Vector3((i % 4) * 150f, (i / 4) * (-150f), 0f);

                         ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                         {
                             item.transform.SetParent(artifactContainer, false);
                             item.transform.localScale = Vector3.one;
                             item.transform.eulerAngles = Vector3.zero;
                             item.transform.localPosition = Vector3.zero + ;
                             item.GetComponent<ItemArtifactIcon>().SetArtifactData(playersArtifact, false);
                         });

                     }*/


            for (int i = 0; i < playersArtifacts.Count; i++)
            {
                ArtifactDataBase artifactData = playersArtifacts[i];

                Vector3 offset = new Vector3((i % 4) * 100f, (i / 4) * (-100f), 0f);

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(artifactContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(artifactData, false);
                });

            }
        }


        public void ShowSpaceshipSelect()
        {
            // 获取经过计算的飞船属性，此处应该复制，而不是直接引用
            currentSpaceshipData = dataPlayer.GetPlayerData().calculatedSpaceship;

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by from player data.");
                return;
            }

            // 修改UI显示值
            s_name.text = currentSpaceshipData.NameId;

            s_type.text = currentSpaceshipData.SType;
            s_size.text = currentSpaceshipData.SSize;

            s_energy.text = currentSpaceshipData.Energy.ToString();
            s_durability.text = currentSpaceshipData.Durability.ToString();
            s_shields.text = currentSpaceshipData.Shields.ToString();
            s_firepower.text = currentSpaceshipData.Firepower.ToString();
            s_fireRate.text = currentSpaceshipData.FireRate.ToString();
            s_agility.text = currentSpaceshipData.Agility.ToString();
            s_speed.text = currentSpaceshipData.Speed.ToString();
            s_detection.text = currentSpaceshipData.Detection.ToString();
            s_capacity.text = currentSpaceshipData.Capacity.ToString();
            s_dogde.text = currentSpaceshipData.Dogde.ToString();

            SetWidth(s_energy_valueBar, currentSpaceshipData.Energy);
            SetWidth(s_durability_valueBar, currentSpaceshipData.Durability);
            SetWidth(s_shields_valueBar, currentSpaceshipData.Shields);
            SetWidth(s_firepower_valueBar, currentSpaceshipData.Firepower);
            SetWidth(s_agility_valueBar, currentSpaceshipData.Agility);
            SetWidth(s_speed_valueBar, currentSpaceshipData.Speed);
            SetWidth(s_detection_valueBar, currentSpaceshipData.Detection);
            SetWidth(s_capacity_valueBar, currentSpaceshipData.Capacity);

/*
            s_skill1.text = currentSpaceshipData.GetSkillData(0).NameId;
            s_skill2.text = currentSpaceshipData.GetSkillData(1).NameId;
            s_skill3.text = currentSpaceshipData.GetSkillData(2).NameId;*/

            ShowSpaceship();

        }

        public void SetWidth(GameObject targetObject, float newWidth)
        {
            // 获取目标对象的 RectTransform 组件
            RectTransform rectTransform = targetObject.GetComponent<RectTransform>();

            // 修改 sizeDelta 属性的 x 值来改变宽度
            Vector2 newSizeDelta = rectTransform.sizeDelta;
            newSizeDelta.x = newWidth;
            rectTransform.sizeDelta = newSizeDelta * valueBarMaxWidth / maxAttrValue;
        }


        public void ShowSpaceship()
        {

            // 显示模型
            entityLoader.ShowEntity(currentSpaceshipData.EntityId, TypeUtility.GetEntityType(currentSpaceshipData.Type),
                (entity) =>
                {
                    showSpaceshipEntity = (EntitySpaceshipSelect)entity.Logic;
                },
                EntityDataSpaceshipSelect.Create(currentSpaceshipData, true));

        }

        // 隐藏模型，这个方法会隐藏字典里所有的模型
        private void HideSpaceship()
        {   
            if(showSpaceshipEntity != null)
            {
                entityLoader.HideEntity(showSpaceshipEntity.Entity);
            }
        }

    }
}


