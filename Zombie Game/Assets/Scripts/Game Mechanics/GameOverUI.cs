using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static bool isGameOver = false;

    public TextMeshProUGUI gameOverText;

    public AudioSource gameOverSource;
    public AudioClip deathSound;
    public AudioClip defeatMusic;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        isGameOver = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowGameOver()
    {
        if (isGameOver) return; // prevents re-calling
        isGameOver = true;

        gameOverText.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioSource[] all = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in all)
        {
            if (a != gameOverSource) a.Stop(); // keep game over source active
        }

        if (gameOverSource != null && deathSound != null)
        {
            gameOverSource.PlayOneShot(deathSound);
            StartCoroutine(PlayDefeatMusic(deathSound.length));
        }
    }

    private IEnumerator PlayDefeatMusic(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (gameOverSource != null && defeatMusic != null)
        {
            gameOverSource.clip = defeatMusic;
            gameOverSource.loop = true;
            gameOverSource.Play();
        }
    }

    void Update()
    {
        if (!isGameOver) return;

        // Restart with R
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOverSource != null) gameOverSource.Stop();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Quit to menu with Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameOverSource.Stop();
            SceneManager.LoadScene("StartMenu");
        }
    }
}



