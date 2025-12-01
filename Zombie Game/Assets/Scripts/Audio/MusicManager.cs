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

        // RoundEnd sends an int (the round #), so we accept it but ignore it
        roundManager.OnRoundEnd += PlayVictoryMusic;

        // start gameplay music
        if (audioSource != null && bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // MATCHES Action<int> required by OnRoundEnd
    void PlayVictoryMusic(int _)
    {
        if (audioSource == null || victory == null) return;

        // stop the bgm
        audioSource.Stop();

        // switch to victory track
        audioSource.clip = victory;
        audioSource.loop = true;
        audioSource.Play();
    }
}

