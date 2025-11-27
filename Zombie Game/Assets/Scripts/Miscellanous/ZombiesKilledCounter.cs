using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombiesKilledCounter : MonoBehaviour
{
    public TextMeshProUGUI zombiesKilledText;
    public RoundManager roundManager;

    private int kills = 0;

    void Start()
    {
        if (roundManager == null)
            roundManager = FindObjectOfType<RoundManager>();

        if (roundManager != null)
            roundManager.OnRoundEnd += UpdateCounter; // optional: if you want to do something at the end
    }

    public void IncrementKills()
    {
        kills++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (zombiesKilledText != null)
            zombiesKilledText.SetText("Zombies Killed: " + kills);
    }
}



