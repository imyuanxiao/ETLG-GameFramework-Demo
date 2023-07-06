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
    public class UISkillTreeMap : UGuiFormEx
    {
        public Transform container;
        public Transform layer1;
        public Transform layer2;
        public Transform layer3;
        public Transform layer4;

        private DataSkill dataSkill;
        private DataPlayer dataPlayer;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataSkill = GameEntry.Data.GetData<DataSkill>();
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            ResetTransform();
            ShowSkillIconItems();

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

        }

        private void ShowSkillIconItems()
        {
            ShowSkillIconByLayer(layer1, 1, Constant.Type.SKILL_SKILL_TREE_MAP);
            ShowSkillIconByLayer(layer2, 2, Constant.Type.SKILL_SKILL_TREE_MAP);
            ShowSkillIconByLayer(layer3, 3, Constant.Type.SKILL_SKILL_TREE_MAP);
            ShowSkillIconByLayer(layer4, 4, Constant.Type.SKILL_SKILL_TREE_MAP);

        }

        private void ShowSkillIconByLayer(Transform layer, int num, int position)
        {

            List<SkillData> skillDatas = dataSkill.GetSkillDataLayer(num);

            Vector3 firstposition = new Vector3(400f, 0f, 0f); // 偏移量

            Vector3 offset = new Vector3(200f, 0f, 0f); // 偏移量

            foreach (var skillData in skillDatas)
            {
                PlayerSkillData playerSkillData =  dataPlayer.GetPlayerData().getSkillById(skillData.Id);

                ShowItem<ItemSkillIcon>(EnumItem.SkillIcon, (item) =>
                {
                    item.transform.SetParent(layer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero + firstposition + skillData.Location[1] * offset;
                    item.GetComponent<ItemSkillIcon>().SetSkillData(playerSkillData, position);
                });
            }
        }

        private void ResetTransform()
        {
            container.localPosition = Vector3.zero;
        }



    }
}


