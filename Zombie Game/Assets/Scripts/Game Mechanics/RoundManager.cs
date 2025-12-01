using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int startRound = 1;

    private int currentRound;
    private int totalZombiesThisRound;
    private int totalSpawned;
    private int totalKilled;

    public event Action<int> OnRoundEnd;
    public event Action OnRoundStart;

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

        totalZombiesThisRound = CalculateZombiesForRound(currentRound);

        Debug.Log($"Round {currentRound} starting — Zombies: {totalZombiesThisRound}");

        OnRoundStart?.Invoke();
    }

    private int CalculateZombiesForRound(int round)
    {
        if (round <= 10)
            return round * 6 + 6;

        return Mathf.FloorToInt(24 * Mathf.Pow(round - 10, 0.15f));
    }

    public void ZombieSpawned()
    {
        totalSpawned++;
    }

    public void ZombieKilled()
    {
        totalKilled++;

        if (totalSpawned >= totalZombiesThisRound &&
            totalKilled >= totalZombiesThisRound)
        {
            if (UpgradeSystem.instance != null)
                UpgradeSystem.instance.GiveUpgrades();

            OnRoundEnd?.Invoke(currentRound);

            currentRound++;

            StartCoroutine(DelayNextRound());
        }
    }

    private IEnumerator DelayNextRound()
    {
        yield return new WaitForSeconds(0.1f); // small delay to allow music switch
        StartNewRound();
    }

    public int CurrentRound => currentRound;

    public bool CanSpawnMoreThisRound()
    {
        return totalSpawned < totalZombiesThisRound;
    }
}




