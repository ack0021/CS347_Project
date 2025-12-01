using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Prefab & spawn")]
    public GameObject enemyPrefab;
    [Tooltip("Optional: multiple spawn points. If empty, spawns at this object's position.")]
    public Transform[] spawnPoints;

    [Header("Spawning control")]
    public float spawnInterval = 1.5f;
    [Tooltip("How many zombies can be alive at once")]
    public int maxActiveEnemies = 5;

    [Header("References")]
    public RoundManager roundManager;
    public Canvas uiCanvas;

    private float timer;
    private int activeEnemies;
    private bool isSpawning = true;

    private void Start()
    {
        if (roundManager == null)
            roundManager = FindObjectOfType<RoundManager>();

        if (roundManager != null)
            roundManager.OnRoundEnd += OnRoundEnd;
    }

    private void Update()
    {
        if (!isSpawning || enemyPrefab == null || roundManager == null)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && roundManager.CanSpawnMoreThisRound() && activeEnemies < maxActiveEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = GetSpawnPoint();

        // ----------- FIX 1: GROUND CHECK -----------
        Vector3 spawnPos = spawnPoint.position;

        // Cast downward to detect floor
        if (Physics.Raycast(spawnPos + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 10f))
        {
            spawnPos = hit.point;
        }
        else
        {
            // Fallback: force Y = 0 if level ground
            spawnPos.y = 0f;
        }
        // -------------------------------------------

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
        if (enemyObj == null)
            return;

        // Configure enemy
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.spawner = this;
            enemy.uiCanvas = uiCanvas;
        }

        activeEnemies++;
        roundManager.ZombieSpawned();
    }

    private Transform GetSpawnPoint()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int idx = Random.Range(0, spawnPoints.Length);
            return spawnPoints[idx];
        }
        return this.transform;
    }

    // Called by Enemy when it dies
    public void EnemyDied()
    {
        activeEnemies = Mathf.Max(activeEnemies - 1, 0);

        if (roundManager != null)
            roundManager.ZombieKilled();
    }

    private void OnRoundEnd(int round)
    {
        isSpawning = false;
        activeEnemies = 0;

        StartCoroutine(ResumeSpawningAfterDelay(2f));
    }

    private IEnumerator ResumeSpawningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = true;
    }

    public int ActiveEnemiesCount => activeEnemies;
}
