using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgm;
    public AudioClip victory;

    public RoundManager roundManager;

    void Start()
    {
        if (roundManager == null)
            roundManager = FindObjectOfType<RoundManager>();

        roundManager.OnRoundEnd += PlayVictoryMusic;

        // start gameplay music
        if (audioSource != null && bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void PlayVictoryMusic()
    {
        // stop the bgm
        audioSource.Stop();

        // switch to victory track
        audioSource.clip = victory;
        audioSource.loop = true;
        audioSource.Play();
    }
}
