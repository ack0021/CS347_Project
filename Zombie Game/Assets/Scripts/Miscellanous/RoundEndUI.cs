using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundEndUI : MonoBehaviour
{
    public TextMeshProUGUI victoryText;
    public PlayerMovement playerMovement;
    public RoundManager roundManager;

    void Start()
    {
        victoryText.gameObject.SetActive(false);

        // Fix: this event passes an int, so our method must accept one
        roundManager.OnRoundEnd += HandleRoundEnd;
    }

    // FIXED: matches Action<int>
    private void HandleRoundEnd(int roundNumber)
    {
        playerMovement.canMove = false;
        victoryText.gameObject.SetActive(true);
        victoryText.text = "VICTORY!";
    }
}

