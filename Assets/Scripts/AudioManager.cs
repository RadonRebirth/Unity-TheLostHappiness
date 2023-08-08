using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private TextTypingBetter textTypingBetter;

    public Sound[] Sounds;

    public void Awake()
    {
/*        // Находим объект TextTypingBetter в сцене и сохраняем ссылку на него
        textTypingBetter = FindObjectOfType<TextTypingBetter>();
        if (textTypingBetter == null)
        {
            Debug.LogError("TextTypingBetter не найден в сцене. Убедитесь, что у вас есть объект с компонентом TextTypingBetter.");
        }*/


        foreach (Sound sound in Sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.clip;
            sound.AudioSource.volume = sound.Volume;
            sound.AudioSource.pitch = sound.Pitch;
            sound.AudioSource.loop = sound.Loop;
        }
    }

    public void Play(string SoundName)
    {
        Sound s = FindSound(SoundName);

        if (s == null)
        {
            Debug.LogError(SoundName + " данный клип не найден!");
            return;
        }
        s.AudioSource.Play();
        Debug.Log(SoundName + " играет!");
    }

    public void Stop(string SoundName)
    {
        Sound s = FindSound(SoundName);

        if (s == null)
        {
            Debug.LogError(SoundName + " данный клип не найден!");
            return;
        }

        s.AudioSource.Stop();
        Debug.Log(SoundName + " остановлен!");
    }

    public bool IsPlaying(string SoundName)
    {
        Sound s = FindSound(SoundName);

        if (s == null)
        {
            Debug.LogError(SoundName + " данный клип не найден!");
            return false;
        }

        return s.AudioSource.isPlaying;
    }

    private Sound FindSound(string SoundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.Name == SoundName);
        return s;
    }
}