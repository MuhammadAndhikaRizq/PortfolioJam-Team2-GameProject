using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPCSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private float minSpawnRate = 0.5f;
    [SerializeField] private float maxSpawnRate = 3f;
    [SerializeField] private bool spawnOnLeft = true;

    void Start()
    {
        StartCoroutine(SpawnBGNPCs());
    }

    IEnumerator SpawnBGNPCs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
            SpawnBackgroundNPC();
        }
    }

    void SpawnBackgroundNPC()
    {
        GameObject npc = Instantiate(npcPrefab, transform.position, Quaternion.identity);
        Vector2 direction = spawnOnLeft ? Vector2.right : Vector2.left;
        npc.GetComponent<BackgroundNPC>().Initialize(direction);
    }
}
