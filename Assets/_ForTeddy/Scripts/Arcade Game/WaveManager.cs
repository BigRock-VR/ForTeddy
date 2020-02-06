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


    public delegate void EndWave();
    public event EndWave onEndWave;

    private float waveEndTimer;
    private GameObject enemyContainer;
    [HideInInspector] public GameObject coinsContainer;

    void Start()
    {
        waveEndTimer = waveTimer;
        timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy);

        enemyContainer = new GameObject("[EnemyContainer]");
        enemyContainer.transform.parent = transform;

        coinsContainer = new GameObject("[CoinsContainer]");
        coinsContainer.transform.parent = transform;
    }

    public void setCanSpawnEnemy(bool status)
    {
        canSpawnEnemy = status;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawnEnemy)
        {
            return;
        }

        if(waveEndTimer <= 0)
        {
            canSpawnEnemy = false;

            if (enemyContainer.transform.childCount > 0)
            {
                onEndWave();
            }

            ++waveCount; // Increase the wave counter
            waveTimer += waveCount; // Increase the wave timer with the current wave counter
            waveEndTimer = waveTimer; // Assign the new wave timer to the current wave
            waveMaxEnemy += waveCount; // Increase the wave enemy to spawn
            timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy); // Calculate how many enemy spawn in the next wave base on the max enemy
        }

        // Set the next time to spawn enemy
        if(Time.time >= nextTimeToSpawn && canSpawnEnemy)
        {
            nextTimeToSpawn = Time.time + timeBetweenSpawn; 
            SpawnEnemyAtRandPosition();
        }

        waveEndTimer -= Time.deltaTime; // Decrease the loca WAVE TIMER

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


}
