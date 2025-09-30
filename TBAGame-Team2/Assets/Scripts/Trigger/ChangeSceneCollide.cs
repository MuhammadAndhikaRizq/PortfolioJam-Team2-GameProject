using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCollide : MonoBehaviour
{
    [Header("UI Manager")]
   
    public GameObject targetScene; 
    public float scaleSpeed = 0.5f;
    public Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
    private SwitchUI switchUI;
    

    [Header("Item Setup")]

    public string itemTag = "Item"; 

    void Start()
    {
        switchUI = FindObjectOfType<SwitchUI>();

        if (switchUI == null)
        {
            Debug.LogError("Script SwitchUI tidak ditemukan di dalam scene!");
        }
    }

  
    void OnTriggerExit2D(Collider2D other)
    {
      
        if (other.CompareTag(itemTag))
        {
            Debug.Log("Item dengan tag '" + itemTag + "' telah keluar dari area. Mengganti scene...");

            if (targetScene != null && switchUI != null)
            {
                other.transform.SetParent(targetScene.transform);
                other.transform.localScale = targetScale;
                switchUI.SwitchTo(targetScene);
            }
            else
            {
                Debug.LogWarning("Target Scene atau SwitchUI belum di-set di Inspector!");
            }
        }
    }

}
