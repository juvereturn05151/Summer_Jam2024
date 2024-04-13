using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject []prefabToSpawn; // The prefab you want to spawn
    [SerializeField]
    private float spawnInterval = 2f; // Interval between spawns
    [SerializeField]
    private Transform spawnPoint; // The point where you want to spawn the prefabs
    [SerializeField]
    private MoveDirection MoveDirection;
    private float nextSpawnTime; // Time of the next spawn

    private void Update()
    {
        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            // Spawn the prefab
            SpawnPrefab();

            // Calculate the time for the next spawn
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnPrefab()
    {
        int random = Random.Range(0, prefabToSpawn.Length);

        // Check if the spawn point is assigned and the prefab is assigned
        if (spawnPoint != null && prefabToSpawn[random] != null)
        {
            // Instantiate the prefab at the spawn point
            AIAgent aIAgent = Instantiate(prefabToSpawn[random], spawnPoint.position, spawnPoint.rotation).GetComponent<AIAgent>();
            aIAgent.SetOnSpawn(MoveDirection);
        }
        else
        {
            // Log an error if either the spawn point or the prefab is not assigned
            Debug.LogError("Spawn point or prefab is not assigned.");
        }
    }
}
