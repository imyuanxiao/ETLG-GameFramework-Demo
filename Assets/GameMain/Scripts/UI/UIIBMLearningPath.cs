using UnityEngine;
using UnityEngine.UI;

public class UIIBMLearningPath : MonoBehaviour
{
    public string urlToOpen = "https://bit.ly/PathWaytoLearning"; 
    private void Start()
    {
        Button button = GetComponent<Button>(); 
        if (button != null)
        {
            button.onClick.AddListener(OpenLink); 
        }
    }

    private void OpenLink()
    {
        Application.OpenURL(urlToOpen); 
    }
}
