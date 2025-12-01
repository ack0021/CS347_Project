using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Round settings")]
    [Tooltip("Round 1 zombie count")]
    public int baseZombieCount = 10;
    [Tooltip("How many extra zombies are added each round")]
    public int zombiesPerRoundIncrease = 5;

    [Header("Optional")]
    public int startRound = 1;

    [HideInInspector] private int currentRound;
    [HideInInspector] private int totalZombiesThisRound;
    [HideInInspector] private int totalSpawned;
    [HideInInspector] private int totalKilled;

    public event Action<int> OnRoundEnd;

    private void Awake()
    {
        currentRound = Mathf.Max(1, startRound);
    }

    private void Start()
    {
        StartNewRound();
    }

    private void StartNewRound()
    {
        totalSpawned = 0;
        totalKilled = 0;
        totalZombiesThisRound = baseZombieCount + (currentRound - 1) * zombiesPerRoundIncrease;
    }

    public void ZombieSpawned()
    {
        totalSpawned++;
    }

    public void ZombieKilled()
    {
        totalKilled++;

        bool allSpawned = totalSpawned >= totalZombiesThisRound;
        bool allKilled = totalKilled >= totalZombiesThisRound;

        if (allSpawned && allKilled)
        {
            OnRoundEnd?.Invoke(currentRound);
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        currentRound++;
        StartNewRound();
    }

    public int CurrentRound => currentRound;
    public int TotalZombiesThisRound => totalZombiesThisRound;
    public int TotalSpawned => totalSpawned;
    public int TotalKilled => totalKilled;

    // Whether the round still has quota left for spawning
    public bool CanSpawnMoreThisRound()
    {
        return totalSpawned < totalZombiesThisRound;
    }
}
