using ETLG.Data;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace ETLG
{
    public class ItemAchievementIcon : ItemLogicEx, IPointerEnterHandler, IPointerExitHandler
    {
        private DataAchievement dataAchievement;
        private PlayerAchievementData playerAchievementData;
        private int id;
        public Image image;
        public Button achievementButton;
        public TextMeshProUGUI acheivementName = null;
        public TextMeshProUGUI progress = null;
        public Transform container;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            dataAchievement = GameEntry.Data.GetData<DataAchievement>();

            EventTrigger trigger = achievementButton.gameObject.AddComponent<EventTrigger>();

        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 获得挂载对象的位置
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);

            // artifactInfo new position
            Vector3 offset = new Vector3(100f, -150f, 0f);
            Vector3 newPosition = itemPosition + offset;
           // RectTransform containerRectTransform = container.GetComponent<RectTransform>();

            GameEntry.Event.Fire(this, AchievementInfoUIEventArgs.Create());

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, SkillInfoCloseEventArgs.Create());
        }
        public void SetAchievementData(PlayerAchievementData playerAchievement,Transform container)
        {
            this.id = playerAchievement.Id;
            this.playerAchievementData = playerAchievement;
            if (playerAchievement.IsUnlocked == true)
            {
                Sprite sprite = Resources.Load<Sprite>(Constant.Type.UNLOCKED_TREASURE_CHEST);
                this.image.sprite = sprite;
            }
            int[] count = dataAchievement.GetDataById(playerAchievement.Id).Count;
            this.acheivementName.text = dataAchievement.GetDataById(playerAchievement.Id).Name;
            this.progress.text = playerAchievement.Progress.ToString()+ " / "+ count[playerAchievement.activeState];
            this.container=container;
        }

    }
}
