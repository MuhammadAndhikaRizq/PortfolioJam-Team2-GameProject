using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class npcController : MonoBehaviour
{
    [Header("Data")]
    public List<ItemData> menuPool;
    public NPCData data;
    [Header("Patience")]
    [ShowInInspector, ReadOnly] float patience;
    float baseDecay = 1;

    [Header("Runtime (debug)")]
    [ShowInInspector, ReadOnly] public List<ItemData> orderList = new();
    [ShowInInspector, ReadOnly] public int expectedTotal;

    void Start()
    {
        patience = data.maxPatienceBarValue;
        MakeOrder();
    }

    void Update()
    {
        TickPatience();
    }

    void TickPatience()
    {
        if (patience > 0)
        {
            patience -= baseDecay * Time.deltaTime;
        }
        else
        {
            Leave();
        }
    }

    void ServeSuccess()
    {
        //Kode kalau berhasil
        Leave();
    }

    void Leave()
    {
        Destroy(gameObject);
    }

    void MakeOrder()
    {
        int count = Random.Range(1, 4);
        for (int i = 0; i < count; i++)
        {
            orderList.Add(menuPool[Random.Range(0, menuPool.Count)]);
            expectedTotal = orderList.Sum(i => i.itemPrice); // tes doang
        }
       
    }
}
