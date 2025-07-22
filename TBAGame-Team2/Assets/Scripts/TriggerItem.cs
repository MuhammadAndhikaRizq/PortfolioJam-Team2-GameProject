using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public Flowchart flowchart;
    

    public void OnTriggerEnter2D(Collider2D other)
    {
        int status = flowchart.GetIntegerVariable("StoryState");
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
    }
}
