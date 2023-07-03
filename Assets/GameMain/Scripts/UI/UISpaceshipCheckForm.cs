﻿using ETLG.Data;
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

        // buttons
        public Button skillTreeButton;
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

        // data manager
        private DataPlayer dataPlayer;
        private DataArtifact dataArtifact;

        // 
        private PlayerCalculatedSpaceshipData currentSpaceshipData = null;

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
            dataArtifact = GameEntry.Data.GetData<DataArtifact>();

            entityLoader = EntityLoader.Create(this);

            currentSpaceshipData = dataPlayer.GetPlayerData().playerCalculatedSpaceshipData;


        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            ShowSpaceshipSelect();

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


            List<PlayerArtifactData> playerArtifacts = dataPlayer.GetPlayerData().getArtifactsByType("all");

            for (int i = 0; i < playerArtifacts.Count; i++)
            {
                //ArtifactDataBase artifactData = dataArtifact.GetArtifactData(playersArtifacts[i].Id);

                Vector3 offset = new Vector3((i % 4) * 100f, (i / 4) * (-110f), 0f);

                // int artifactNumber = playersArtifacts[i].Number;

                PlayerArtifactData playerArtifact = playerArtifacts[i];

                ShowItem<ItemArtifactIcon>(EnumItem.ArtifactIcon, (item) =>
                {
                    item.transform.SetParent(artifactContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + offset;
                    item.GetComponent<ItemArtifactIcon>().SetArtifactData(playerArtifact);
                });

            }
        }


        public void ShowSpaceshipSelect()
        {
            // 获取经过计算的飞船属性，此处应该复制，而不是直接引用

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by from player data.");
                return;
            }

            s_name.text = dataPlayer.GetPlayerData().initialSpaceship.NameId;
            s_type.text = dataPlayer.GetPlayerData().initialSpaceship.SType;
            s_size.text = dataPlayer.GetPlayerData().initialSpaceship.SSize;

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
            entityLoader.ShowEntity(dataPlayer.GetPlayerData().initialSpaceship.EntityId, TypeUtility.GetEntityType(dataPlayer.GetPlayerData().initialSpaceship.Type),
                (entity) =>
                {
                    showSpaceshipEntity = (EntitySpaceshipSelect)entity.Logic;
                },
                EntityDataSpaceshipSelect.Create(dataPlayer.GetPlayerData().initialSpaceship, true));

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


