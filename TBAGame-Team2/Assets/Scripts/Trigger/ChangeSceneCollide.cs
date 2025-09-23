using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCollide : MonoBehaviour
{
    public GameObject targetScene;
    private string item = "Item";
    private SwitchUI switchUI;

    void Start()
    {
        switchUI = FindObjectOfType<SwitchUI>(); // or assign in Inspector
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(item)) // tag your draggable item as "Item"
        {
            Debug.Log("Item exited fridge zone");
            switchUI.SwitchTo(targetScene);
        }
    }
}
