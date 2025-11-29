using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int totalZombiesThisRound = 20; // total zombies for this round
    private int totalSpawned = 0;
    private int totalKilled = 0;

    public event Action OnRoundEnd;

    public bool CanSpawnZombie()
    {
        // Only allow spawn if we haven’t reached the limit
        return totalSpawned < totalZombiesThisRound;
    }

    public void ZombieSpawned()
    {
        totalSpawned++;
    }

    public void ZombieKilled()
    {
        totalKilled++;

        // If all zombies are killed, end the round
        if (totalKilled >= totalZombiesThisRound)
        {
            OnRoundEnd?.Invoke();
        }
    }

    public int ZombiesRemaining()
    {
        return totalZombiesThisRound - totalKilled;
    }
}

