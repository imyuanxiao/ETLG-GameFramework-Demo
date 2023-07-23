using ETLG.Data;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class UIRewardPreviewForm : UGuiFormEx
    {

        private DataNPC dataNPC;

        public RectTransform UIContainer;

        public RectTransform RewardsContainer;

        public bool refresh;

        // 初始化菜单数据
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataNPC = GameEntry.Data.GetData<DataNPC>();

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

            if (refresh)
            {
                showContent();
                LayoutRebuilder.ForceRebuildLayoutImmediate(UIContainer);
                refresh = false;
            }

            base.OnUpdate(elapseSeconds, realElapseSeconds);

     

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            refresh = true;
        }

        public void showContent()
        {
            UIContainer.position = dataNPC.RewardUIPosition;
            ShowRewards(RewardsContainer);
        }



        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
        }

        private void ShowRewards(Transform RewardsContainer)
        {

            HideAllItem();

            NPCData npcData = dataNPC.GetCurrentNPCData();

            if(npcData.RewardArtifacts.Length > 1)
            {
                int[] rewardArtifacts = npcData.RewardArtifacts;
                for(int i = 0; i <  rewardArtifacts.Length; i+=2)
                {
                    int id = rewardArtifacts[i];
                    int num = rewardArtifacts[i+1];
                    ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                    {
                        item.transform.SetParent(RewardsContainer, false);
                        item.transform.localScale = Vector3.one;
                        item.transform.eulerAngles = Vector3.zero;
                        item.transform.localPosition = Vector3.zero;
                        item.GetComponent<ItemRewardPreview>().SetRewardData(id, num, Constant.Type.REWARD_TYPE_ARTIFACT);
                    });
                }
            }

            if (npcData.RewardSkill != 0)
            {
                int id = npcData.RewardSkill;
                ShowItem<ItemRewardPreview>(EnumItem.ItemRewardPreview, (item) =>
                {
                    item.transform.SetParent(RewardsContainer, false);
                    item.transform.localScale = Vector3.one;
                    item.transform.eulerAngles = Vector3.zero;
                    item.transform.localPosition = Vector3.zero;
                    item.GetComponent<ItemRewardPreview>().SetRewardData(id, 1, Constant.Type.REWARD_TYPE_SKILL);
                });

            }

        }
    }
}


