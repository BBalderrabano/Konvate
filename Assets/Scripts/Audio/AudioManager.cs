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
}
