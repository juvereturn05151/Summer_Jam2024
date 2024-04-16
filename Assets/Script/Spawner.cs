using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner instance;

    // Public property to access the singleton instance
    public static Spawner Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<Spawner>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("Spawner");
                    instance = singletonObject.AddComponent<Spawner>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    // Optional Awake method to ensure the instance is created before any other script's Start method
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If this is the first instance, set it as the singleton instance
        instance = this;

        // Ensure the instance persists between scene changes
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private GameObject []prefabToSpawn; // The prefab you want to spawn
    public float spawnInterval = 2f; // Interval between spawns
    private float nextSpawnTime; // Time of the next spawn

    private Vector3 spawnPoint; // The point where you want to spawn the prefabs

    private void Update()
    {
        if (TimeManager.Instance._TimePhase == TimePhase.Morning)
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
    }

    private void SpawnPrefab()
    {
        int random = Random.Range(0, prefabToSpawn.Length);

        spawnPoint = GetRandomBorderPosition();

        // Check if the spawn point is assigned and the prefab is assigned
        if (spawnPoint != null && prefabToSpawn[random] != null)
        {
            // Instantiate the prefab at the spawn point
            AIAgent aiAgent = Instantiate(prefabToSpawn[random], spawnPoint, Quaternion.identity).GetComponent<AIAgent>();
            aiAgent.SetOnSpawn(PlayerBase.Instance.gameObject);
        }
        else
        {
            // Log an error if either the spawn point or the prefab is not assigned
            Debug.LogError("Spawn point or prefab is not assigned.");
        }
    }

    Vector3 GetRandomBorderPosition()
    {
        // Determine a random border side (top, bottom, left, right)
        int side = Random.Range(0, 4); // 0: top, 1: bottom, 2: left, 3: right

        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Top side
                spawnPosition = new Vector3(Random.Range(-cameraWidth, cameraWidth), cameraHeight, 0f);
                break;
            case 1: // Bottom side
                spawnPosition = new Vector3(Random.Range(-cameraWidth, cameraWidth), -cameraHeight, 0f);
                break;
            case 2: // Left side
                spawnPosition = new Vector3(-cameraWidth, Random.Range(-cameraHeight, cameraHeight), 0f);
                break;
            case 3: // Right side
                spawnPosition = new Vector3(cameraWidth, Random.Range(-cameraHeight, cameraHeight), 0f);
                break;
        }

        return spawnPosition;
    }
}
