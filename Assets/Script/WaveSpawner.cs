using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // Drag your enemy prefab here in the Inspector!
    public float enemySpawnInterval = 1.5f; // Time between spawning individual enemies in a wave

    [Header("Wave Settings")]
    public int enemiesPerWave = 5; // How many enemies per wave
    public float timeBetweenWaves = 5f; // Time to wait after a wave finishes before the next one starts

    // Internal tracking variables
    private Transform[] spawnPoints; // Array to hold all child spawn points
    private int currentWaveNumber = 0;
    private int enemiesCurrentlyAlive = 0; // Tracks enemies that have been spawned and are still active
    private int enemiesToSpawnThisWave; // Tracks how many enemies still need to be spawned for the current wave

    private bool waveInProgress = false; // True if a wave is currently being spawned or enemies from it are alive

    void Start()
    {
        // Get all child transforms (our spawn points)
        List<Transform> pointsList = new List<Transform>();
        foreach (Transform child in transform)
        {
            pointsList.Add(child);
        }
        spawnPoints = pointsList.ToArray();

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points found as children of the Spawner! Please add empty GameObjects as children.", this);
            enabled = false; // Disable the script if no spawn points are found
            return;
        }

        Debug.Log("Spawner Initialized with " + spawnPoints.Length + " spawn points.");
        // Start the first wave immediately
        StartNextWave();
    }

    void Update()
    {
        // If a wave is not in progress (meaning all enemies from the previous wave are cleared
        // and we are not in the delay period for the next wave), check if we should start the next.
        // This 'Update' check is an alternative/backup to relying solely on EnemyDestroyed
        // for triggering the next wave, which can be useful if enemies are removed by other means.
        // However, with our coroutines, the primary trigger will still be EnemyDestroyed.
        if (!waveInProgress && enemiesCurrentlyAlive <= 0 && currentWaveNumber > 0)
        {
            // This condition ideally shouldn't be met frequently if coroutines manage timing,
            // but it acts as a safety net. The coroutine-based triggering is more reliable.
            // Debug.Log("DEBUG: Wave not in progress and no enemies alive. Considering next wave.");
        }
    }


    // This method is called by your Shooter2DController when an enemy is destroyed.
    public void EnemyDestroyed()
    {
        enemiesCurrentlyAlive--;
        Debug.Log($"Enemy destroyed. Enemies currently alive: {enemiesCurrentlyAlive}.");

        // Only consider starting the next wave if the current one was active
        // and all enemies that were spawned (and not yet destroyed) are now gone.
        // We ensure enemiesToSpawnThisWave is 0 meaning all enemies for this wave were launched.
        if (enemiesCurrentlyAlive <= 0 && enemiesToSpawnThisWave <= 0 && waveInProgress)
        {
            Debug.Log($"Wave {currentWaveNumber} cleared! All {enemiesPerWave} enemies spawned and destroyed.");
            waveInProgress = false; // Mark wave as truly finished (spawned and cleared)
            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    // Initiates the process to start a new wave
    void StartNextWave()// *Alex kallar på den*
    {
        currentWaveNumber++;
        enemiesToSpawnThisWave = enemiesPerWave; // Reset for the new wave
        enemiesCurrentlyAlive = 0; // Reset count of active enemies (they will be counted as spawned)
        waveInProgress = true; // A new wave is starting

        Debug.Log($"Starting Wave {currentWaveNumber}. Will spawn {enemiesPerWave} enemies.");
        StartCoroutine(SpawnWaveRoutine());
    }

    // Coroutine to handle spawning enemies within a wave
    IEnumerator SpawnWaveRoutine()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            enemiesCurrentlyAlive++; // Increment count of enemies currently active
            enemiesToSpawnThisWave--; // Decrement count of enemies still to be launched from this wave
            Debug.Log($"Spawned enemy {i + 1}. Enemies to spawn: {enemiesToSpawnThisWave}. Enemies alive: {enemiesCurrentlyAlive}");

            yield return new WaitForSeconds(enemySpawnInterval);
        }
        Debug.Log($"All {enemiesPerWave} enemies for Wave {currentWaveNumber} have been launched.");
        // At this point, all enemies for the wave have been *spawned*.
        // The wave is still "in progress" because we're waiting for enemiesCurrentlyAlive to reach 0.
        // The transition to the next wave delay happens in EnemyDestroyed().
    }

    // Coroutine to handle the delay between waves
    IEnumerator StartNextWaveAfterDelay()
    {
        Debug.Log($"Waiting for {timeBetweenWaves} seconds before Wave {currentWaveNumber + 1}.");
        yield return new WaitForSeconds(timeBetweenWaves);
        StartNextWave();
    }

    // Spawns a single enemy at a random spawn point
    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab not assigned to Spawner! Please assign it in the Inspector.", this);
            return;
        }
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points available! Ensure your Spawner has child GameObjects.", this);
            return;
        }

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }
}