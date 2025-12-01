using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
