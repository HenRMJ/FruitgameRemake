using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private Sound[] sounds;

    private AudioSource audioSource;
    private float sfxVolume = 1f;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("An instance of the sound manager already exists in the scene");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                AudioSource.PlayClipAtPoint(sound.clip, Camera.main.transform.position, sound.volume * sfxVolume);
            }
        }
    }

    public void SetSoundEffectVolume(float volume)
    {
        sfxVolume = volume;
    }

    public AudioSource GetAudioSource() => audioSource;
}
