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
    public class UISpaceshipSelectForm : UGuiFormEx
    {

        // 需要挂在后添加的按钮和文本等对象
        public Button startButton;
        public Button leftButton;
        public Button rightButton;
        public Button returnButton;

        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_description = null;

        public TextMeshProUGUI s_skill1 = null;
        public TextMeshProUGUI s_skill2 = null;
        public TextMeshProUGUI s_skill3 = null;


        // 需要通过数据管理器获取数据
        private DataSpaceship dataSpaceship = null;

        // 当前展示的飞船信息
        private SpaceshipData currentSpaceshipData = null;

        // 当前展示飞船的ID
        private int currentIndex = (int)EnumEntity.InterstellarExplorer;

        // 当前显示的实体模型，可以同时存在多个对象
        private Dictionary<int, EntitySpaceshipSelect> dicEntitySpaceshipSelect;

        // 实体加载器
        private EntityLoader entityLoader;


        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            // 绑定按钮点击事件
            startButton.onClick.AddListener(OnStartButtonClick);
            leftButton.onClick.AddListener(OnLeftButtonClick);
            rightButton.onClick.AddListener(OnRightButtonClick);
            returnButton.onClick.AddListener(OnReturnButtonClick);

            // 获取数据管理器
            dataSpaceship = GameEntry.Data.GetData<DataSpaceship>();

            // 初始化存飞船模型实体对象的字典
            dicEntitySpaceshipSelect = new Dictionary<int, EntitySpaceshipSelect>();
            // 初始化实体加载类
            entityLoader = EntityLoader.Create(this);
            // 调用显示飞船方法
            ShowSpaceshipSelect();


        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            currentIndex = (int)EnumEntity.InterstellarExplorer;
            ShowSpaceshipSelect();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            HideAllEnemyEntity();
        }

        private void OnStartButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

        }

        private void OnLeftButtonClick()
        {

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // 改变飞船编号，初始飞船选项只有 1001~1004
            currentIndex--;
            if(currentIndex < (int)EnumEntity.InterstellarExplorer)
            {
                currentIndex = (int)EnumEntity.Guardian;
            }

            // 用新飞船编号展示UI和模型
            ShowSpaceshipSelect();
        }
        private void OnRightButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);


            currentIndex++;
            if (currentIndex > (int)EnumEntity.Guardian)
            {
                currentIndex = (int)EnumEntity.InterstellarExplorer;
            }
            ShowSpaceshipSelect();
        }

        private void OnReturnButtonClick()
        {
            Log.Debug("返回主菜菜单");
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Menu")));

        }


        public void ShowSpaceshipSelect()
        {

            HideAllEnemyEntity();


            // 通过数据管理器的方法初始化当前飞船信息
            currentSpaceshipData = dataSpaceship.GetSpaceshipData(currentIndex);

            if (currentSpaceshipData == null)
            {
                Log.Error("Can not get spaceship data by id '{0}'.", currentIndex);
                return;
            }

            // 修改UI显示值
            s_name.text = currentSpaceshipData.NameId;
            s_description.text = currentSpaceshipData.Description;

            s_skill1.text = currentSpaceshipData.GetSkillData(0).NameId;
            s_skill2.text = currentSpaceshipData.GetSkillData(1).NameId;
            s_skill3.text = currentSpaceshipData.GetSkillData(2).NameId;

            ShowNewSpaceshipSelect();

        }

        public void ShowNewSpaceshipSelect()
        {

            // 显示新模型
            entityLoader.ShowEntity(currentSpaceshipData.EntityId, TypeUtility.GetEntityType(currentSpaceshipData.Type),
                (entity) =>
                {
                    // 回调函数，将EntityDataSpaceshipSelect.Create方法生成的对象加到dicEntitySpaceshipSelect中，如果不需要回调函数，可以不填写
                    dicEntitySpaceshipSelect.Add(entity.Id, (EntitySpaceshipSelect)entity.Logic);
                },
                EntityDataSpaceshipSelect.Create(
                    currentSpaceshipData));

        }

        // 隐藏模型，这个方法会隐藏字典里所有的模型
        private void HideAllEnemyEntity()
        {
            foreach (var item in dicEntitySpaceshipSelect.Values)
            {
                entityLoader.HideEntity(item.Entity);
            }

            dicEntitySpaceshipSelect.Clear();
        }

    }
}


