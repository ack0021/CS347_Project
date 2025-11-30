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
    }

    public void IncrementKills()
    {
        kills++;
        UpdateCounter(kills);
    }

    private void UpdateCounter(int kills)
    {
        if (zombiesKilledText != null)
            zombiesKilledText.SetText("Zombies Killed: " + kills);
    }
}




