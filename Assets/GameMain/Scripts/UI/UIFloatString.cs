using UnityEngine;
public class UIFloatString : MonoBehaviour
{
    public static string FloatToString(float progress)
    {
            string progressString = (progress * 100).ToString("F0")+"%";
            return progressString;
    }
}