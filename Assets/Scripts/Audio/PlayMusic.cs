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

            Play();
        }
    }

    public void Play()
    {
        lastIndex = index;
        audioSource.PlayOneShot(GetMusicTrack(), Settings.VOLUME_MUSIC);
    }

    public void StopAllSounds()
    {
        index = 0;
        playMusic = false;
        audioSource.Stop();
    }

    private AudioClip GetMusicTrack()
    {
        return tracks[index];
    }
}
