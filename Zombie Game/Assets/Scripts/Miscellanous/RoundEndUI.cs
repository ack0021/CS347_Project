using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundEndUI : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public TestPlayerMovement playerMovement;
    public RoundManager roundManager;

    void Start()
    {
        victoryText.gameObject.SetActive(false);

        roundManager.OnRoundEnd += HandleRoundEnd;
    }

    private void HandleRoundEnd()
    {
        playerMovement.canMove = false;
        victoryText.gameObject.SetActive(true);
        victoryText.text = "VICTORY!";
    }
}
