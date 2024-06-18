using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static bool HasInstance => _instance != null;

    public static SoundManager Instance { get { return _instance; } }

    public SoundClass[] Sounds;

    [SerializeField]
    float masterVolume = 1.0f;

    #region Singleton



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        foreach (SoundClass s in Sounds)
        {
            if (s.clip != null)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.outputAudioMixerGroup = s.mixerGroup;
                s.Source.clip = s.clip;
                s.Source.volume = s.Vol;
                s.Source.pitch = s.Pitch;
                s.Source.loop = s.IsLoop;
                s.Source.playOnAwake = s.IsPlayOnAwake;
                s.Source.spatialBlend = s.SpatialBlend;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion


    private void Start()
    {
        AudioListener.volume = masterVolume;
    }

    public void ChangePitch(string name, float pitch)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        pitch = Mathf.Clamp(pitch, 0.1f, 3.0f);
        s.Source.pitch = pitch;
    }

    public void ChangeVolume(string name, float vol)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        vol = Mathf.Clamp(vol, 0f, 1.0f);
        s.Source.volume = vol;
    }

    public void EnableLoop(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        s.Source.loop = true;
    }

    public void StopLoop(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        s.Source.loop = false;
    }

    public void Play(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        if (!s.Source.isPlaying)
        {
            s.Source.Play();
        }
    }

    public void PlayWhileOtherSoundIsNotPlaying(string name)
    {

        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        if (s.Source.isPlaying == false)
        {
            s.Source.Play();
        }
    }

    public void AssignAudioSource(AudioSource source, string audioName)
    {
        foreach (SoundClass s in Sounds)
        {
            if (s.Name == audioName)
            {
                source.clip = s.clip;
                source.volume = s.Vol;
                source.pitch = s.Pitch;
                source.loop = s.IsLoop;
                source.playOnAwake = s.IsPlayOnAwake;
                source.spatialBlend = s.SpatialBlend;
            }
        }

    }

    public void AssignAudioSourceAndPlayLoop(AudioSource source, string audioName)
    {
        foreach (SoundClass s in Sounds)
        {
            if (s.Name == audioName)
            {
                source.clip = s.clip;
                source.volume = s.Vol;
                source.pitch = s.Pitch;
                source.loop = s.IsLoop;
                source.playOnAwake = s.IsPlayOnAwake;
                source.spatialBlend = s.SpatialBlend;
            }
        }

        source.loop = true;

        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    public void LowerVolumeUntilItsStop(AudioSource source, float decreaseRate)
    {
        source.volume -= decreaseRate;

        if (source.volume <= 0)
        {
            source.Stop();
        }
    }

    public void AssignAudioSourceAndPlayOneShot(AudioSource source, string audioName)
    {
        foreach (SoundClass s in Sounds)
        {
            if (s.Name == audioName)
            {
                source.clip = s.clip;
                source.volume = s.Vol;
                source.pitch = s.Pitch;
                source.loop = s.IsLoop;
                source.playOnAwake = s.IsPlayOnAwake;
                source.spatialBlend = s.SpatialBlend;
            }
        }

        source.loop = false;
        source.PlayOneShot(source.clip);

    }

    public void Stop(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        s.Source.Stop();
    }

    public void PlayOneShot(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }

        s.Source.PlayOneShot(s.Source.clip);
    }

    public string RandomSound(string[] audioNameList)
    {
        if (audioNameList.Length <= 0)
        {
            return "";
        }

        int random = UnityEngine.Random.Range(0, audioNameList.Length);

        return audioNameList[random];
    }

    public void FadeStop(string name)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }
        StartCoroutine(DropVolumeToZero(s.Source));
    }

    public void ChageVolume(string name, float volume)
    {
        SoundClass s = Array.Find(Sounds, Sound => Sound.Name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found...");
            return;
        }
        StartCoroutine(DropVolume(s.Source, volume));
    }

    public void InitOnGameBegin()
    {
        Stop("BGM_Title");
        Play("BGM_Gameplay");
    }

    private IEnumerator DropVolumeToZero(AudioSource source)
    {
        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DropVolume(AudioSource source, float volume)
    {
        while (source.volume > volume)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }
    }
}