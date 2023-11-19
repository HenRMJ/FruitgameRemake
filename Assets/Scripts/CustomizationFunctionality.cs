using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationFunctionality : MonoBehaviour
{
    public void ResetButtons()
    {
        foreach (UICustomizationElement element in FindObjectsOfType<UICustomizationElement>())
        {
            ES3.DeleteKey(element.GetElementKey());
        }

        GameSceneManager.Instance.ReloadScene();
    }
}
