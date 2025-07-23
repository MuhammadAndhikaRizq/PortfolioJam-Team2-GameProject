using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public Flowchart flowchart;
    public NPCController npcController;

    public void OnTriggerEnter2D(Collider2D other)
    {
        ItemHolder itemHolder = other.GetComponent<ItemHolder>();
        //int status = flowchart.GetIntegerVariable("StoryState");
        #region NPCTrigger
        if (itemHolder != null)
        {
            ItemData itemData = itemHolder.itemData;

            if (npcController.ReceiveItem(itemData))
            {
                Destroy(other.gameObject);
                //flowchart.ExecuteBlock("Maya1FirstOrder");
                Debug.Log("Item received: " + itemData.itemName);
            }
            else
            {  
                //flowchart.ExecuteBlock("OrderWrong");
                Debug.Log("Item not received: " + itemData.itemName);
            }
        }
        else
        {
            Debug.Log("ItemHolder not found on: " + other.name);
        }
        #endregion

            /*
            if (other.CompareTag("Rokok") && status == 0)
            {
                flowchart.SetBooleanVariable("isItemDelivered", true);
                Destroy(other.gameObject);
                flowchart.ExecuteBlock("Maya1SecondOrder");
                flowchart.SetIntegerVariable("StoryState", 1);
            }

            if (other.CompareTag("Juice") && status == 0)
            {
            flowchart.SetBooleanVariable("isItemDelivered", true);
            Destroy(other.gameObject);
            flowchart.ExecuteBlock("OrderWrong");
            }
            */
    }
}
