using ETLG;
using ETLG.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tipTitle;

    private void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 itemPosition = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        Vector3 newPosition = itemPosition + new Vector3(0f, 10f, 0f);

        GameEntry.Event.Fire(this, TipOpenEventArgs.Create(newPosition, tipTitle));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEntry.Event.Fire(this, TipCloseEventArgs.Create());
    }


}
