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
    public class UISpaceshipSelectForm : UGuiFormEx
    {


        // 需要挂在后添加的按钮和文本等对象
        public Button startButton;
        public Button leftButton;
        public Button rightButton;
        public Button returnButton;

        // name and description
        public TextMeshProUGUI s_name = null;
        public TextMeshProUGUI s_description = null;

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

        
        private float valueBarMaxWidth = 150;
        private float maxAttrValue = 300;

        public GameObject s_energy_valueBar = null;
        public GameObject s_durability_valueBar = null;
        public GameObject s_shields_valueBar = null;
        public GameObject s_firepower_valueBar = null;
        public GameObject s_agility_valueBar = null;
        public GameObject s_speed_valueBar = null;
        public GameObject s_detection_valueBar = null;
        public GameObject s_capacity_valueBar = null;



        // initial skills
        public Transform skillContainer = null;




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



        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            currentIndex = (int)EnumEntity.InterstellarExplorer;

            // 调用显示飞船方法
            ShowSpaceshipSelect();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            HideNewSpaceshipSelect();
        }

        private void OnStartButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_forward);

            // Start a new game and use selected spaceship to construct player data
            GameEntry.Data.GetData<DataPlayer>().NewGame(currentSpaceshipData);

            // 通过设置事件，流程里监听该事件从而设置下一个场景和流程
            GameEntry.Event.Fire(this, ChangeSceneEventArgs.Create(GameEntry.Config.GetInt("Scene.Map")));

            HideNewSpaceshipSelect();


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

            HideNewSpaceshipSelect();

        }


        public void ShowSpaceshipSelect()
        {

            HideNewSpaceshipSelect();


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

            ShowNewSpaceshipSelect();

            ShowSkillIconByLayer(skillContainer);

        }

        private void ShowSkillIconByLayer(Transform container)
        {


            List<PlayerSkillData> tmpPlayerSkillsData = new List<PlayerSkillData>();
            foreach (var skillId in currentSpaceshipData.SkillIds)
            {
                tmpPlayerSkillsData.Add(new PlayerSkillData(skillId, Constant.Type.SKILL_UPGRADED, 1));
            }

            //SkillData[] skillDatas = currentSpaceshipData.GetSkillDatas();

            Vector3 offset = new Vector3(150f, 0f, 0f); // 偏移量

            int i = 0;

            foreach (var tmpPlayerSkill in tmpPlayerSkillsData)
            {
                ShowItem<ItemSkillIcon>(EnumItem.SkillIcon, (item) =>
                {
                    item.transform.SetParent(container, false);
                    item.transform.localScale = Vector3.one * 0.5f;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + (i++) * offset;
                    item.GetComponent<ItemSkillIcon>().SetSkillData(tmpPlayerSkill);
                });
            }
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


        public void ShowNewSpaceshipSelect()
        {

            // 显示新模型
            entityLoader.ShowEntity(currentSpaceshipData.EntityId, TypeUtility.GetEntityType(currentSpaceshipData.Type),
                (entity) =>
                {
                    // 回调函数，将EntityDataSpaceshipSelect.Create方法生成的对象加到dicEntitySpaceshipSelect中，如果不需要回调函数，可以不填写
                    dicEntitySpaceshipSelect.Add(entity.Id, (EntitySpaceshipSelect)entity.Logic);
                },
                EntityDataSpaceshipSelect.Create(currentSpaceshipData,false));

        }

        // 隐藏模型，这个方法会隐藏字典里所有的模型
        private void HideNewSpaceshipSelect()
        {
            foreach (var item in dicEntitySpaceshipSelect.Values)
            {
                entityLoader.HideEntity(item.Entity);
            }

            dicEntitySpaceshipSelect.Clear();
        }

    }
}


