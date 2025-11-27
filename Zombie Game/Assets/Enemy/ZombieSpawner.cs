using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Canvas uiCanvas;
    public float spawnInterval = 2f;

    private float timer = 0f;
    private int activeEnemies = 0;
    public int enemyKilled = 0;

    public RoundManager roundManager; // assign in inspector or find dynamically

    void Update()
    {
        timer += Time.deltaTime;

        // Only spawn if enough time passed AND round allows it
        if (timer >= spawnInterval && roundManager != null && roundManager.CanSpawnZombie())
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, transform.position, transform.rotation);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.spawner = this;
            enemy.uiCanvas = uiCanvas;
        }

        activeEnemies++;
        roundManager.ZombieSpawned();
    }

    public void EnemyDied()
    {
        activeEnemies = Mathf.Max(activeEnemies - 1, 0);
        enemyKilled++;
    }
}



