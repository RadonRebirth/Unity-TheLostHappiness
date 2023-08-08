using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string Name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float Volume = 1f;

    [Range(0.1f, 3f)]
    public float Pitch = 1f;

    public bool Loop = false;

    
    public AudioSource AudioSource;
}
