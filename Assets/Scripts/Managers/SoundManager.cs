using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool musicEnabled;
    public bool fxEnabled;

    [Range(0, 1)]
    public float musicVolume = 1.0f;
    [Range(0, 1)]
    public float fxVolume = 1.0f;

    public SoundDescriptor[] sounds;

    public AudioClip[] backgroudMusicClips;

    public AudioSource musicSource;


    public SoundToggledEvent fxToggled = new SoundToggledEvent();
    public SoundToggledEvent musicToggled = new SoundToggledEvent();

    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic(GetRandomClip(backgroudMusicClips));

        fxToggled.Invoke(fxEnabled);
        musicToggled.Invoke(musicEnabled);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!musicEnabled || !musicClip || !musicSource)
        {
            return;
        }

        musicSource.Stop();

        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;

        musicSource.Play();
    }



    public void PlaySound(string soundName)
    {
        SoundDescriptor soundDescriptor = GetSound(soundName);
        if (soundDescriptor == null)
        {
            return;
        }

        if (fxEnabled && soundDescriptor.sound)
        {
            AudioSource.PlayClipAtPoint(soundDescriptor.sound, Camera.main.transform.position, Mathf.Clamp(fxVolume * soundDescriptor.soundMultiplier, 0.05f, 1f));
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateMusic();

        musicToggled.Invoke(musicEnabled);
    }

    public void ToggleFX()
    {
        fxEnabled = !fxEnabled;
        fxToggled.Invoke(fxEnabled);
    }


    private void UpdateMusic()
    {
        if (musicSource.isPlaying != musicEnabled)
        {
            if (musicEnabled)
            {
                PlayBackgroundMusic(GetRandomClip(backgroudMusicClips));
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    private SoundDescriptor GetSound(string soundName)
    {
        foreach (SoundDescriptor sound in sounds)
        {
            if (sound.name == soundName)
            {
                return sound;
            }
        }

        return null;
    }

}
