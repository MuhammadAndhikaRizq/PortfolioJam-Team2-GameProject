using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        flowchart.ExecuteBlock("Maya1FirstOrder");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
