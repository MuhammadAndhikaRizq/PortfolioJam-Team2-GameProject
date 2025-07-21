using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class TriggerItem : MonoBehaviour
{
    public Flowchart flowchart;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rokok"))
        {
            flowchart.SetBooleanVariable("isItemDelivered", true);
            Destroy(other.gameObject);
            flowchart.ExecuteBlock("Maya1SecondOrder");
        }

        if (other.CompareTag("Juice"))
        {
            flowchart.SetBooleanVariable("isItemDelivered", true);
            Destroy(other.gameObject);
            flowchart.ExecuteBlock("OrderWrong");
        }
    }
}
