using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    public RoundManager roundManager;

    void Start()
    {
        if (roundManager == null)
            roundManager = FindObjectOfType<RoundManager>();
    }

    void Update()
    {
        UpdateCounter(roundManager.CurrentRound);
    }

    private void UpdateCounter(int round)
    {
        if (roundText != null)
            roundText.SetText("Round " + round);
    }
}
