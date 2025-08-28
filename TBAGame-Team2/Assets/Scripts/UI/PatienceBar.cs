using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private Slider patienceSlider;
    // [SerializeField] private Customer customer;

    void Start()
    {
        if (patienceSlider == null)
        {
            patienceSlider = GetComponent<Slider>();
        }

        if (npcData != null)
        {
            patienceSlider.maxValue = npcData.maxPatienceBarValue;
            patienceSlider.value = npcData.maxPatienceBarValue;
        }
        else
        {
            Debug.LogWarning("NPC Data not assigned to PatienceBar");
        }
    }

    void Update()
    {
        // if (customer != null)
        // {
        //     // Update the slider value based on customer's current patience
        //     patienceSlider.value = customer.currentWaitTime;

        //     // Optional: Change color based on patience level
        //     UpdateBarColor();
        // }
    }
    
    void UpdateBarColor()
    {
        float patiencePercent = patienceSlider.value / patienceSlider.maxValue;
        Image fillImage = patienceSlider.fillRect.GetComponent<Image>();
        
        if (patiencePercent > 0.6f)
            fillImage.color = Color.green;
        else if (patiencePercent > 0.3f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }
}
