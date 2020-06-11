using UnityEngine;
using System.Collections.Generic;

public class PlayMusic : MonoBehaviour
{
    private AudioSource audioSource;

    int lastIndex = 0;
    int index = 0;

    public List<AudioClip> tracks;
    public bool playMusic = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playMusic && !audioSource.isPlaying)
        {
            while(index == lastIndex)
            {
                index = Random.Range(0, tracks.Count);
            }

            PlaySound();
        }
    }

    public void PlaySound()
    {
        lastIndex = index;
        audioSource.PlayOneShot(GetMusicTrack());
    }

    private AudioClip GetMusicTrack()
    {
        return tracks[index];
    }
}
