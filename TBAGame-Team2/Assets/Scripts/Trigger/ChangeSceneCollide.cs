using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCollide : MonoBehaviour
{
    [Header("UI Manager")]
    public GameObject targetScene; // UI tujuan (etalase utama)
    private SwitchUI switchUI;

    [Header("Item Setup")]
    public string itemTag = "Item";


    private bool isInside = false; // penanda item ada di dalam kulkas

    void Start()
    {
        switchUI = FindObjectOfType<SwitchUI>();

        // cek manual: apakah item sudah ada di dalam trigger saat start
        // Collider2D col = GetComponent<Collider2D>();
        // Collider2D[] overlaps = new Collider2D[10];
        // ContactFilter2D filter = new ContactFilter2D().NoFilter();

        // int count = col.OverlapCollider(filter, overlaps);
        // for (int i = 0; i < count; i++)
        // {
        //     if (overlaps[i].CompareTag(itemTag))
        //     {
        //         isInside = true;
        //         Debug.Log("Item sudah ada di dalam kulkas sejak awal");
        //     }
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.CompareTag(itemTag) && isInside)
        // {
            Debug.Log("Item keluar kulkas → kembali ke etalase");

            switchUI.SwitchTo(targetScene);



        // }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag(itemTag))
    //     {
    //         // cek kalau posisi item sudah lewat batas kulkas
    //         if (!GetComponent<Collider2D>().bounds.Contains(other.transform.position))
    //         {
    //             Debug.Log("Item benar-benar keluar kulkas");
    //             switchUI.SwitchTo(targetScene);


    //             Destroy(other.gameObject);
    //         }
    //     }
    // }
    
}
