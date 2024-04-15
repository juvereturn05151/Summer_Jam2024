using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundClass
{

    public string Name;
    public AudioClip clip;
    public bool IsPlayOnAwake;
    public bool IsLoop;
    [Range(0f, 1f)]
    public float Vol;
    [Range(.1f, 3f)]
    public float Pitch;
    [Range(0f, 1f)]
    public float SpatialBlend;

    [HideInInspector]
    public AudioSource Source;
}