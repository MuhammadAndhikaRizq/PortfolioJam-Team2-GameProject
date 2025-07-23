using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPCBarrier : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collision detected with: {other.name}");
        if (other.CompareTag("BackgroundNPC"))
        {
            Debug.Log("Destroying NPC");
            Destroy(other.gameObject);
        }
    }
}
