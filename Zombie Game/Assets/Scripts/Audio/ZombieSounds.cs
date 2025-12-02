using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] growls;

    public float minDelay = 3f;
    public float maxDelay = 7f;

    public float hearingDist = 20f;

    public Transform player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(PlayRandomSounds());
    }

    private IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (player == null) continue;

            if (Vector3.Distance(transform.position, player.position) <= hearingDist)
            {
                PlayRandomGrowl();
            }
        }
    }

    void PlayRandomGrowl()
    {
        if (GameOverUI.isGameOver) return;
        if (growls.Length == 0) return;

        AudioClip clip = growls[Random.Range(0, growls.Length)];
        audioSource.PlayOneShot(clip);
    }
}

