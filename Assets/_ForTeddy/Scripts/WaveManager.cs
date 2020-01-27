using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]public Transform[] spawnPositions;
    [SerializeField] public GameObject[] enemyTypes;
    public float waveEndTimer = 60.0f;
    public bool canSpawnEnemy;
    public float timeBetweenSpawn = 0.0f;
    public float nextTimeToSpawn = 0.0f;
    public int waveMaxEnemy = 10;
    public delegate void EndWave();
    public event EndWave onEndWave;


    private GameObject enemyContainer;

    void Start()
    {
        timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy);
        enemyContainer = new GameObject("[EnemyContainer]");
        enemyContainer.transform.parent = transform;
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
            waveEndTimer = 60.0f;
            onEndWave();
        }
        if(Time.time >= nextTimeToSpawn)
        {
            nextTimeToSpawn = Time.time + timeBetweenSpawn;
            SpawnEnemyAtRandPosition();
        }
        waveEndTimer -= Time.deltaTime;

    }

    private void SpawnEnemyAtRandPosition()
    {
        int randomSpawnPos = UnityEngine.Random.Range(0, spawnPositions.Length);
        int randomEnemy = UnityEngine.Random.Range(0, enemyTypes.Length);
        Instantiate(enemyTypes[randomEnemy], spawnPositions[randomSpawnPos].position, Quaternion.identity, enemyContainer.transform);
    }

}
