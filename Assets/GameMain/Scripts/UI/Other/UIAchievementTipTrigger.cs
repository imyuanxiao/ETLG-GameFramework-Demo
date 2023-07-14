using UnityEngine;
using UnityEngine.EventSystems;
using ETLG;
using ETLG.Data;
using UnityEngine.UI;

namespace ETLG
{
    public class UIAchievementTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string tipTitle;
        private DataAchievement dataAchievement;
        private void Start()
        {
            dataAchievement = GameEntry.Data.GetData<DataAchievement>();
            
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(-200f, 50f, 0f);
            Button button = eventData.pointerEnter.GetComponent<Button>();
            if (button!=null)
            {
                Debug.Log("ItemAchievementIcon found");
                ItemAchievementIcon icon = button.GetComponent<ItemAchievementIcon>();
                int id = icon.GetCurrentAchievementID();
                if (icon.acheivementName.text != null)
                {
                    tipTitle = icon.acheivementName.text;
                }
                GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN));
            }
            else
            {
                Debug.Log("ItemAchievementIcon not found");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
        }
    }
}
