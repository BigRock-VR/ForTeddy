using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]public Transform[] spawnPositions;
    [SerializeField]public GameObject[] enemyTypes;
    [SerializeField]public Transform[] soldierPositions;

    public int waveCount = 0;
    public float waveTimer = 20.0f;
    public bool canSpawnEnemy;
    public float timeBetweenSpawn = 0.0f;
    public float nextTimeToSpawn = 0.0f;
    public int waveMaxEnemy = 10;

    public bool isBossSpawned;

    // GAME EVENTS
    public delegate void EndWave();
    public event EndWave onEndWave;

    private float waveEndTimer;
    private GameObject enemyContainer;
    [HideInInspector] public GameObject coinsContainer;

    // BOSS WAVE
    public GameObject bossPrefab;
    private readonly int BOSS_WAVE_INTERVAL = 5; // Spawn the boss every x wave;
    void Start()
    {
        waveEndTimer = waveTimer;
        timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy);

        enemyContainer = new GameObject("[EnemyContainer]");
        enemyContainer.transform.parent = transform;

        coinsContainer = new GameObject("[CoinsContainer]");
        coinsContainer.transform.parent = transform;
    }

    private void StartWave()
    {
        canSpawnEnemy = true;
    }

    public void StartGame()
    {
        Invoke("StartWave", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawnEnemy)
        {
            return;
        }


        if (isBossWave() && !isBossSpawned)
        {
            SpawnBosAtPosition(spawnPositions[spawnPositions.Length].position);
        }

        if(waveEndTimer <= 0)
        {
            canSpawnEnemy = false;

            if (enemyContainer.transform.childCount > 0)
            {
                onEndWave();
            }

            UpdateWave();
        }

        // Set the next time to spawn enemy
        if(Time.time >= nextTimeToSpawn && canSpawnEnemy)
        {
            nextTimeToSpawn = Time.time + timeBetweenSpawn; 
            SpawnEnemyAtRandPosition();
        }

        waveEndTimer -= Time.deltaTime; // Decrease the loca WAVE TIMER

    }

    public void UpdateWave()
    {
        ++waveCount; // Increase the wave counter
        waveTimer += waveCount; // Increase the wave timer with the current wave counter
        waveEndTimer = waveTimer; // Assign the new wave timer to the current wave
        waveMaxEnemy += waveCount; // Increase the wave enemy to spawn
        timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy); // Calculate how many enemy spawn in the next wave base on the max enemy
    }

    public void StartNextWave()
    {
        canSpawnEnemy = true;
    }

    private void SpawnEnemyAtRandPosition()
    {
        int randomSpawnPos = UnityEngine.Random.Range(0, spawnPositions.Length);
        int randomEnemy = UnityEngine.Random.Range(0, enemyTypes.Length);
        Instantiate(enemyTypes[randomEnemy], spawnPositions[randomSpawnPos].position, Quaternion.identity, enemyContainer.transform);
    }


    private void SpawnBosAtPosition(Vector3 pos)
    {
        Instantiate(bossPrefab, pos, Quaternion.identity, enemyContainer.transform);
        isBossSpawned = true;
    }

    public bool isBossWave()
    {
        return (waveCount % BOSS_WAVE_INTERVAL) == 0;
    }
}
