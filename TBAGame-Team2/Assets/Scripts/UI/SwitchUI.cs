using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;

    public void SwitchTo(GameObject uiEnabled)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiEnabled.SetActive(true);
    }
    
}
