using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public bool isMenu;

    public Sound[] sounds;
    public bool muteSound;
    public bool muteMusic;
    public Sound[] shutDown;
    public Sound[] menuSounds;

    private int soundtrackType = 0; // 0: instrumental, 1: normal, 2: drunk
    private float soundtrackTime = 0;
    private float soundtrackCounter = 0;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.ignoreListenerVolume = true;
            s.source.loop = s.loop;
        }
    }

    public void Mute()
    {
        muteSound = true;
    }

    public void Unmute()
    {
        muteSound = false;
    }

    public void PlayMusic(string name)
    {
        if (muteMusic)
            return;
        if (name == "")
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name " + name + " was not found");
            return;
        }
        s.source.Play();
    }

    public void Play(string name) // plays a sound with the given name
    {
        if (muteSound)
            return;
        if (name == "")
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name " + name + " was not found");
            return;
        }
        //s.source.Play();
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name) // plays a sound with the given name
    {
        if (muteSound)
            return;
        if (name == "")
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with name " + name + " was not found");
            return;
        }
        s.source.Stop();
    }

    public void StartSoundtrack()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "music");
        soundtrackTime = 31.0f;
        PlayMusic("music_instrumental");
        soundtrackType = 0;
        soundtrackCounter = 0;
    }

    public void StopSoundtrack()
    {
        Stop("music_instrumental");
        Stop("music");
        Stop("music_drunk");
    }

    public void ChangeSoundtrack(int type) // 0: instrumental, 1: normal, 2: drunk
    {
        string music_string = "";
        switch (type)
        {
            case 0:
                {
                    music_string = "music_instrumental";
                    break;
                }
            case 1:
                {
                    music_string = "music";

                    break;
                }
            case 2:
                {
                    music_string = "music_drunk";
                    break;
                }
        }
        Stop("music_drunk");
        Stop("music");
        Stop("music_instrumental");
        Sound s = Array.Find(sounds, sound => sound.name == music_string);
        soundtrackType = type;
        s.source.Play();
        s.source.time = soundtrackCounter;
    }

    void Update()
    {
        if (isMenu)
            return;
        if (soundtrackTime == 0)
            return;
        soundtrackCounter += Time.deltaTime;
        if (soundtrackCounter >= soundtrackTime)
        {
            if (soundtrackType == 0)
            {
                soundtrackType = 1;
                Sound s = Array.Find(sounds, sound => sound.name == "music");
                soundtrackTime = s.clip.length;
                Stop("music_instrumental");
            }
            soundtrackCounter = 0;
            ChangeSoundtrack(soundtrackType);
        }
    }
}
