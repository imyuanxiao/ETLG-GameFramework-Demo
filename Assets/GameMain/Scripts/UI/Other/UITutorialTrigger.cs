using ETLG;
using ETLG.Data;
using UnityEngine;
using UnityEngine.UI;


public class UITutorialTrigger : MonoBehaviour
{

    public Button button;
    public int TutorialID;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameEntry.Data.GetData<DataTutorial>().CurrentTutorialID = TutorialID;

        UnityGameFramework.Runtime.UIForm[] uiForms = GameEntry.UI.GetAllLoadedUIForms();

        foreach(var uiForm in uiForms)
        {
            uiForm.OnPause();
        }

  /*      if (GameEntry.Data.GetData<DataUI>().UINavigationFormUIID != null)
        {
            GameEntry.UI.CloseUIForm((int)GameEntry.Data.GetData<DataUI>().UINavigationFormUIID);
            GameEntry.Data.GetData<DataUI>().UINavigationFormUIID = null;
        }*/

        GameEntry.UI.OpenUIForm(EnumUIForm.UITutorialForm);
    }

}
