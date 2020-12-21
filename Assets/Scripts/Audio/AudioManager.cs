using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;
    private List<SoundEffect> soundEffects = new List<SoundEffect>();

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }

        soundEffects.AddRange(gameObject.GetComponentsInChildren<SoundEffect>());
    }

    public void Play(SoundEffectType type)
    {
        if (type == SoundEffectType.NONE)
            return;

        for (int i = 0; i < soundEffects.Count; i++)
        {
            if(soundEffects[i].type == type)
            {
                soundEffects[i].PlaySound();
            }
        }
    }

    public void PlayMusic()
    {
        GetComponentInChildren<PlayMusic>().playMusic = true;
    }

    public void StopAllSounds()
    {
        GetComponentInChildren<PlayMusic>().StopAllSounds();
    }

    public AudioClip victoryClip;
    public AudioClip victoryFireworks;

    public void PlayVictorySfx()
    {
        StopAllSounds();

        LeanAudio.play(victoryClip, Settings.VOLUME_SFX);

        LeanTween.value(0, 1, victoryFireworks.length).setOnStart(()=> 
        {
            LeanAudio.play(victoryFireworks, (0.7f * Settings.VOLUME_SFX));

        }).setOnComplete(()=> {
            LeanTween.value(1, 0, victoryFireworks.length)
                .setOnUpdate(DissolveFireworks);
        });
    }

    AudioSource fireworks = null;

    void DissolveFireworks(float value)
    {
        if(fireworks == null)
        {
            fireworks = LeanAudio.play(victoryFireworks, (0.7f * Settings.VOLUME_SFX));
        }
        else
        {
            fireworks.volume *= value;
        }
    }

    public AudioClip defeatClip;
    public AudioClip defeatEffects;

    public void PlayDefeatySfx()
    {
        StopAllSounds();

        LeanAudio.play(defeatClip, Settings.VOLUME_SFX);
        LeanAudio.play(defeatEffects, Settings.VOLUME_SFX).time = 1.3f;
    }
}
