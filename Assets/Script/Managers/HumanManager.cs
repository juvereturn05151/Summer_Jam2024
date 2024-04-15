using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    private static HumanManager instance;

    // Public property to access the singleton instance
    public static HumanManager Instance
    {
        get
        {
            // If the instance doesn't exist, try to find it in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<HumanManager>();

                // If it still doesn't exist, create a new GameObject with the SingletonExample component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("HumanManager");
                    instance = singletonObject.AddComponent<HumanManager>();
                }

                // Ensure the instance persists between scene changes
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    [SerializeField]
    private Human _human;
    public Human Human => _human;

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

    public void MoveHere(GameObject target) 
    {
        HumanAIAgent humanAIAgent = Human.HumanAIAgent;
        humanAIAgent.ChangeState(new StateSeek(humanAIAgent, target));
    }

    public void MoveHere(Vector3 pos)
    {
        HumanAIAgent humanAIAgent = Human.HumanAIAgent;
        humanAIAgent.ChangeState(new StateSeek(humanAIAgent, pos));
    }
}
