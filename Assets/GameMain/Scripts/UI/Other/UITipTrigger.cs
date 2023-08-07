using UnityEngine;
using UnityEngine.EventSystems;
using ETLG;
using ETLG.Data;

public class UITipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tipTitle;
    public string tipContent = "";

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

        if (position == Constant.Type.TIP_INFO_POSITION_DOWN)
        {
            newPosition = itemPosition + new Vector3(0f, -120f, 0f);
        }

        GameEntry.Data.GetData<DataPlayer>().tipUiPosition = newPosition;
        GameEntry.Data.GetData<DataPlayer>().tipTitle = tipTitle;
        GameEntry.Data.GetData<DataPlayer>().tipContent = tipContent.Equals("0") ? 
            GameEntry.Localization.GetString(Constant.Key.PRE_TIP + tipTitle) : tipContent;

        if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
        {
            GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
        }

        GameEntry.UI.OpenUIForm(EnumUIForm.UITipForm);
       // GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(newPosition, tipTitle, Constant.Type.UI_OPEN));

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameEntry.UI.HasUIForm(EnumUIForm.UITipForm))
        {
            GameEntry.UI.CloseUIForm(GameEntry.UI.GetUIForm(EnumUIForm.UITipForm));
        }
        //GameEntry.Event.Fire(this, TipUIChangeEventArgs.Create(Constant.Type.UI_CLOSE));
    }


}
