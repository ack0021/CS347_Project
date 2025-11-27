using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Canvas uiCanvas;         // assign your Canvas here
    public float spawnInterval = 2f;
    public int maxEnemies = 10;

    private float timer;
    private int enemyCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && enemyCount < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, transform.position, transform.rotation);

        // Assign canvas to the spawned enemy
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.uiCanvas = uiCanvas;
        }

        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--;
    }
}
