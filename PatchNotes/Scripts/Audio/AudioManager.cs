using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Dictionary<AudioClip, AudioSource> sourcesDic = new();
    public AudioMixerGroup uiGroup;
    public AudioMixer mixer;

    public void Init()
    {
        GlobalVolume = GlobalVolume;        
    }

    public void Play(AudioClip clip)
    {
        AudioSource s;
        if (!sourcesDic.TryGetValue(clip, out s))
        {
            s = gameObject.AddComponent<AudioSource>();
            s.clip = clip;
            s.outputAudioMixerGroup = uiGroup;
            sourcesDic[clip] = s;
        }
        s.Stop();
        s.Play();
    }
    public void StopAll()
    {
        foreach (var (k, v) in sourcesDic)
        {
            v.Stop();
        }
    }

    public float GlobalVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("GlobalVolume", 20.0f);
        }
        set
        {
            mixer.SetFloat("GlobalVolume", value - 80);
            PlayerPrefs.SetFloat("GlobalVolume", value);
        }
    }
}