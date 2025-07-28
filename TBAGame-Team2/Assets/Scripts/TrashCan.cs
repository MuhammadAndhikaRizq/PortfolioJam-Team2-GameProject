using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public TriggerItem triggerItem;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlasticBag")) return;
        triggerItem.ResetPlastic();
    }
}
