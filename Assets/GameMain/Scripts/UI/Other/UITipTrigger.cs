using UnityEngine;
using UnityEngine.EventSystems;
using ETLG;

public class UITipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tipTitle;

    public int position = 0;

    private void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        Vector3 newPosition = itemPosition + new Vector3(0f, 10f, 0f);

        if (position == Constant.Type.TIP_INFO_POSITION_LEFT)
        {
            newPosition = itemPosition + new Vector3(-410f, -100f, 0f);
        }

        GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
    }


}
