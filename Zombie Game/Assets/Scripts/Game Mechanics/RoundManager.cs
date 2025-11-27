using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int totalZombiesThisRound = 20; // total zombies for this round
    private int totalSpawned = 0;

    public event Action OnRoundEnd;

    public bool CanSpawnZombie()
    {
        // Only allow spawn if we haven’t reached the limit
        return totalSpawned < totalZombiesThisRound;
    }

    public void ZombieSpawned()
    {
        totalSpawned++;

        // If we reached the limit, end the round
        if (totalSpawned >= totalZombiesThisRound)
        {
            OnRoundEnd?.Invoke();
        }
    }

    public int ZombiesRemaining()
    {
        return totalZombiesThisRound - totalSpawned;
    }
}

