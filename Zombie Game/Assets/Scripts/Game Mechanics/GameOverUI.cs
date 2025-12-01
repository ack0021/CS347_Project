using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static bool isGameOver = false;

    public TextMeshProUGUI gameOverText;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        isGameOver = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowGameOver()
    {
        isGameOver = true;

        gameOverText.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!isGameOver) return;

        // Restart with R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Quit to menu with Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}

