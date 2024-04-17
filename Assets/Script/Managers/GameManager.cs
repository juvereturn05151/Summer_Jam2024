using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawner2;

    [SerializeField]
    private GameObject spawner3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.score >= 300)
        {
            if (spawner3)
                spawner3.gameObject.SetActive(true);
        }
        else if(ScoreManager.score >= 150)
        {
            if (spawner2)
                spawner2.gameObject.SetActive(true);
        }

    }
}
