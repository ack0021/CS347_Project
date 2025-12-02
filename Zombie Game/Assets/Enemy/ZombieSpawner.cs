using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int maxActiveEnemies = 24;

    [Header("Scaling (Rounds)")]
    public int maxScalingRound = 10;
    public float startHealth = 100f;
    public float maxHealth = 1000f;
    public float startSpeed = 4f;
    public float maxSpeed = 7f;

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

        float dynamicSpawnInterval = Mathf.Lerp(2f, 0.15f, roundManager.CurrentRound / 30f);

        if (timer >= dynamicSpawnInterval &&
            roundManager.CanSpawnMoreThisRound() &&
            activeEnemies < maxActiveEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = p.position;

        GameObject enemyObj = Instantiate(enemyPrefab, pos, p.rotation);

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.spawner = this;
            enemy.uiCanvas = uiCanvas;

            float t = Mathf.Clamp01((float)roundManager.CurrentRound / maxScalingRound);

            enemy.maxHealth = Mathf.Lerp(startHealth, maxHealth, t);
            enemy.moveSpeed = Mathf.Lerp(startSpeed, maxSpeed, t);
            enemy.SetHealthToMax();
        }

        activeEnemies++;
        roundManager.ZombieSpawned();
    }

    public void EnemyDied()
    {
        activeEnemies = Mathf.Max(activeEnemies - 1, 0);
        roundManager.ZombieKilled();
    }

    private void OnRoundEnd(int r)
    {
        isSpawning = false;
        activeEnemies = 0;
        StartCoroutine(ResumeSpawningAfterDelay(3f));
    }

    private IEnumerator ResumeSpawningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = true;
    }
}



