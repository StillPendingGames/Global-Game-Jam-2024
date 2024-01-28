using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(.1f, 3f)]
    public float pitch = 1;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
    public bool isSong;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null) return;
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }

    public void StopAllSounds()
    {
        foreach (Sound item in sounds)
        {
            if (item != null) item.source.Stop();
        }
    }

    public void StopCurrentSongPlayNew(string name)
    {
        foreach (Sound item in sounds)
        {
            if(item.isSong) item.source.Stop();
        }
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null) return;
        s.source.Play();
    }
}
