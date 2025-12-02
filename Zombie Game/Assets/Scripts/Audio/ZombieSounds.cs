using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] growls;

    public float minDelay = 3f; // minimum delay between growls
    public float maxDelay = 7f; // maximum delay between growls

    public float hearingDist = 20f; // maximum distance to player sound can be heard
    
    public Transform player;

    // Start is called before the first frame update
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