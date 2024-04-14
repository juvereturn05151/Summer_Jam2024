using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchLightSystem : MonoBehaviour
{
    [SerializeField] private Sunlight sunlight; 
    [SerializeField] private Moonlight moonlight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // switch (TimeManager.Instance._TimePhase)
        // {
        //     case TimePhase.Morning:
        //         moonlight.IsNight = false;
        //         sunlight.IsMorning = true;
        //         break;
        //     case TimePhase.Night:
        //         sunlight.IsMorning = false;
        //         moonlight.IsNight = true;
        //         break;
        // }
    }
}
