using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Info:")]
    [SerializeField]public Transform[] spawnPositions;
    [SerializeField]public GameObject[] enemyTypes;
    [SerializeField]public Transform[] soldierPositions;
    [SerializeField]public Transform bedPositions;
    public PlayerManager playerManager;
    // BOSS WAVE
    [Header("Boss Info:")]
    [SerializeField]public GameObject bossPrefab;
    private readonly int BOSS_WAVE_INTERVAL = 5; // Spawn the boss every x wave;
    private int midMobPct = 10; // Mid mob percentage of spawn; 
    public bool isBossSpawned;

    [Header("Wave Info:")]
    public int waveCount = 0;
    public float waveTimer = 20.0f;
    public bool canSpawnEnemy;
    public float timeBetweenSpawn = 0.0f;
    public float nextTimeToSpawn = 0.0f;
    public int waveMaxEnemy = 10;


    // GAME EVENTS
    public delegate void EndWave();
    public event EndWave onEndWave;

    public delegate void OpenShop();
    public event EndWave onOpenShop;

    public delegate void GameStart();
    public event GameStart onStartGame;

    public float waveEndTimer;
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
        playerManager.onPlayerDeath += PlayerManager_onPlayerDeath;
    }

    private void PlayerManager_onPlayerDeath()
    {
        canSpawnEnemy = false;
        playerManager.onPlayerDeath -= PlayerManager_onPlayerDeath;
    }

    private void StartWave()
    {
        canSpawnEnemy = true;
    }

    public void StartGame()
    {
        onStartGame?.Invoke();
        Invoke("StartWave", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawnEnemy)
        {
            return;
        }


        if (isBossWave())
        {
            if (!isBossSpawned)
            {
                SpawnBosAtPosition(spawnPositions[0].position);
                return;
            }
            return;
        }

        if(waveEndTimer <= 0)
        {

            if (enemyContainer.transform.childCount > 0)
            {
                onEndWave();
            }

            UpdateWave();
        }

        // Set the next time to spawn enemy
        if(Time.time >= nextTimeToSpawn && canSpawnEnemy && !isBossWave())
        {
            nextTimeToSpawn = Time.time + timeBetweenSpawn; 
            SpawnEnemyAtRandPosition();
        }

        waveEndTimer -= Time.deltaTime; // Decrease the loca WAVE TIMER

    }

    public void UpdateWave()
    {
        if (!playerManager.isDead)
        {
            onOpenShop?.Invoke();
        }

        isBossSpawned = false;
        canSpawnEnemy = false;
        isBossSpawned = false;
        ++waveCount; // Increase the wave counter
        waveTimer += waveCount; // Increase the wave timer with the current wave counter
        waveEndTimer = waveTimer; // Assign the new wave timer to the current wave
        waveMaxEnemy += waveCount; // Increase the wave enemy to spawn
        timeBetweenSpawn = Mathf.Abs(waveEndTimer / waveMaxEnemy); // Calculate how many enemy spawn in the next wave base on the max enemy
    }

    private void SpawnEnemyAtRandPosition()
    {
        int randomSpawnPos = Random.Range(0, spawnPositions.Length);
        int randomEnemy = Random.Range(0, enemyTypes.Length);
        int percent = Random.Range(1, 100);
        if (percent <= midMobPct)
        {
            Instantiate(enemyTypes[1], spawnPositions[randomSpawnPos].position, Quaternion.identity, enemyContainer.transform);
        }
        else
        {
            Instantiate(enemyTypes[0], spawnPositions[randomSpawnPos].position, Quaternion.identity, enemyContainer.transform);
        }
    }


    private void SpawnBosAtPosition(Vector3 pos)
    {
        isBossSpawned = true;
        Instantiate(bossPrefab, pos, Quaternion.identity, enemyContainer.transform); 
    }

    public bool isBossWave()
    {
        return (waveCount % BOSS_WAVE_INTERVAL) == 0;
    }
}
