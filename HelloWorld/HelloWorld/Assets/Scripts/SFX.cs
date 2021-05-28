using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SFX 
{
    public string name;
    
    public AudioClip clip;
    
    public float volume;
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;
}
