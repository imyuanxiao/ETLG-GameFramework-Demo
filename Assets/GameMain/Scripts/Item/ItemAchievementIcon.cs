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
        private DataPlayer dataPlayer;
        private AchievementData achievementData;
        public Image image;
        public Button achievementButton;
        public TextMeshProUGUI acheivementName = null;
        public TextMeshProUGUI progress = null;
        public TextMeshProUGUI next_level = null;
        public Transform container;
        public string tipTitle;
        public int position = 0;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();

            EventTrigger trigger = achievementButton.gameObject.AddComponent<EventTrigger>();

        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(0f, -130f, 0f);
            if (acheivementName.text != null)
            {
                tipTitle = acheivementName.text;
            }
            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }
        public void SetAchievementData(AchievementData achievementData, Transform container)
        {
            int id = achievementData.Id;
            int type = achievementData.ConditionId;
            this.achievementData = achievementData;
            if (dataPlayer.GetPlayerData().GetPlayerAchievement().ContainsKey(id))
            {
                Sprite sprite = Resources.Load<Sprite>(Constant.Type.UNLOCKED_TREASURE_CHEST);
                this.image.sprite = sprite;
            }
            this.acheivementName.text = achievementData.Name;
           /* switch(type)
            {

            }
           */
            this.progress.text = "0";
            this.next_level.text= GetNextLevel();
            this.container=container;
        }
        private string GetNextLevel()
        {
            PlayerData playerData = dataPlayer.GetPlayerData();
            Dictionary<int,int> playerAchievement = playerData.GetPlayerAchievement();

            if (!playerAchievement.ContainsKey(achievementData.Id))
            {
                return achievementData.Count[0].ToString();
            }

            int level = playerAchievement[achievementData.Id];
            int countIndex = (level < achievementData.Count.Length) ? level : level - 1;

            return achievementData.Count[countIndex].ToString();
        }
    }
}
