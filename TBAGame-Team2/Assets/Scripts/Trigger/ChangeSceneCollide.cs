using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCollide : MonoBehaviour
{
    [Header("UI Manager")]
    // UI/Scene yang akan dituju setelah item keluar
    public GameObject targetScene; 
    
    // Referensi ke script SwitchUI Anda
    private SwitchUI switchUI;

    [Header("Item Setup")]
    // Pastikan tag ini sama dengan tag pada GameObject item Anda
    public string itemTag = "Item"; 

    void Start()
    {
        // Cari script SwitchUI di scene saat permainan dimulai
        switchUI = FindObjectOfType<SwitchUI>();

        if (switchUI == null)
        {
            Debug.LogError("Script SwitchUI tidak ditemukan di dalam scene!");
        }
    }

    // Fungsi ini akan dieksekusi HANYA KETIKA item KELUAR dari area trigger
    void OnTriggerExit2D(Collider2D other)
    {
        // Cek apakah objek yang keluar memiliki tag yang benar
        if (other.CompareTag(itemTag))
        {
            Debug.Log("Item dengan tag '" + itemTag + "' telah keluar dari area. Mengganti scene...");
            
            // Panggil fungsi untuk beralih ke UI/scene target
            if (targetScene != null && switchUI != null)
            {
                switchUI.SwitchTo(targetScene);
            }
            else
            {
                Debug.LogWarning("Target Scene atau SwitchUI belum di-set di Inspector!");
            }
        }
    }

}
