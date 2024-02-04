using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Sound
{
    // These are different properties for adding audios dynamically in the inspector

    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 1;
    [Range(0.1f, 3f)]
    public float Pitch = 1;
    public bool Loop;

    [HideInInspector]
    public AudioSource Source;

    // Static function for avoiding code redundancy when initializing sounds
    public static void InitializeSounds(GameObject gameObject, Sound[] sounds)
    {
        foreach (Sound sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }
    }

}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private Sound[] _sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        } 
        Instance = this;
        DontDestroyOnLoad(this);

        Sound.InitializeSounds(gameObject, _sounds);
    }

    // Plays the sound based on the name, if doesn't match won't be played
    public void PlaySound(string soundName)
    {
        var sound = _sounds.FirstOrDefault(x => x.Name == soundName);
        if (sound?.Source?.isPlaying == false)
            sound.Source.Play();
    }

    // Stops the sound based on the name, if doesn't match won't be stopped
    private void StopSound(string soundName)
    {
        var sound = _sounds.FirstOrDefault(x => x.Name == soundName);
        if (sound?.Source?.isPlaying == true)
            sound.Source.Stop();
    }
}
