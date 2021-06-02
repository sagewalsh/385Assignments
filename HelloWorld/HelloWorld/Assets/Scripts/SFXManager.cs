using UnityEngine.Audio;
using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public SFX[] soundEffects;

    private void Awake()
    {
        foreach(SFX sfx in soundEffects)
        {
            sfx.audioSource = gameObject.AddComponent<AudioSource>();
            sfx.audioSource.clip = sfx.clip;
            sfx.audioSource.volume = sfx.volume;
            sfx.audioSource.pitch = sfx.pitch;
        }
    }
    public void PlaySound(string sfxSearch)
    {
        SFX sfx = Array.Find(soundEffects, sfx => sfx.name == sfxSearch);        

        if(sfx != null) sfx.audioSource.Play();
        else Debug.Log("No SFX file found");
    }
}