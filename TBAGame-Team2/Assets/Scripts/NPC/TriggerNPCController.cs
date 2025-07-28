using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNPCController : MonoBehaviour
{
    public TriggerItem triggerItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            var npcCtrl = other.GetComponent<NPCController>();
            var orderCK = other.GetComponent<OrderCheck>();
            if (npcCtrl != null)
            {
                triggerItem.SetNPCController(npcCtrl, orderCK);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            triggerItem.ClearNPCController();
        }
    }
}
