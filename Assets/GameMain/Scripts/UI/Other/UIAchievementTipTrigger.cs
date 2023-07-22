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
        private DataPlayer dataPlayer;
        private void Start()
        {
            dataPlayer = GameEntry.Data.GetData<DataPlayer>();
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
                int level = dataPlayer.GetPlayerData().GetNextLevel(id);
                if (icon.acheivementName.text != null)
                {
                    tipTitle = icon.acheivementName.text;
                }
                GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN,level,id));
            }
            else
            {
                Debug.Log("ItemAchievementIcon not found");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Vector3 newPosition = itemPosition + new Vector3(-200f, 50f, 0f);
            GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_CLOSE, 0, 0));
        }
    }
}
