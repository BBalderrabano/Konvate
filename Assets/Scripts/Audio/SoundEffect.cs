using UnityEngine;
using System.Collections.Generic;

public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    public SoundEffectType type;

    public List<AudioClip> sound_versions;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(GetAudioClip());
    }

    private AudioClip GetAudioClip()
    {
        if(sound_versions.Count > 1)
        {
            return sound_versions[Random.Range(0, sound_versions.Count)];
        }
        else
        {
            return sound_versions[0];
        }
    }
}

public enum SoundEffectType
{
    SHUFFLE_DECK,
    DRAW_CARD,
    PLACE_CHIP,
    DISCARD_CARD,
    NONE,
    PLACE_CARD,
    PICK_CARD_UP,
    BUTTON_CLICK,
    MOVE_CHIP,

    CARD_ORCO_FUERZA_EN_NUMEROS,

    CARD_MAGO_NO_DECK
}
