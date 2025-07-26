using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Sirenix.OdinInspector;
using Fungus;


public class TriggerDialog : MonoBehaviour
{
    [LabelText("Dialog to Trigger")]
    public Flowchart flowchart;
    


    public void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("NPC"))
        {
            if (flowchart != null && !string.IsNullOrEmpty(blockName))
            {
                flowchart.ExecuteBlock(blockName);
                this.gameObject.SetActive(false); // Disable the trigger after execution
            }
            else
            {
                Debug.LogWarning("Flowchart or block name is not set.");
            }
        }*/

        NPCController npcController = other.GetComponent<NPCController>();
        if (npcController != null)
        {
            NPCOrderData order = npcController.currentOrder;
            flowchart = npcController.currentFlowchart;
            flowchart.ExecuteBlock(order.blockName);
        }

    }
}
