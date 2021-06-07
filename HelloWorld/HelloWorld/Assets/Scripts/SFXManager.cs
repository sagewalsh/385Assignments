using UnityEngine.Audio;
using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public SFX[] soundEffects;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        foreach(SFX sfx in soundEffects)
        {
            sfx.audioSource = gameObject.AddComponent<AudioSource>();
            sfx.audioSource.clip = sfx.clip;
            sfx.audioSource.volume = sfx.volume;
            sfx.audioSource.pitch = sfx.pitch;
            sfx.audioSource.loop = sfx.loop;
        }

        PlaySound("music");
    }

    public void PlaySound(string sfxSearch)
    {
        if(!FindSound(sfxSearch, out SFX sfx))
        {
            return;
        }

        sfx.audioSource.Play();
    }

    public void Mute(string sfxSearch)
    {
        if (!FindSound(sfxSearch, out SFX sfx))
        {
            return;
        }

        sfx.audioSource.mute = true;
    }

    public void Unmute(string sfxSearch)
    {
        if (!FindSound(sfxSearch, out SFX sfx))
        {
            return;
        }

        sfx.audioSource.mute = false;
    }

    private bool FindSound(string sfxSearch, out SFX sfx)
    {
        sfx = Array.Find(soundEffects, sfx => sfx.name == sfxSearch);

        if (sfx != null)
        {
            return true;
        }
        Debug.Log("No SFX file with the name of '" + sfxSearch + "' found");
        return false;
    }
}
