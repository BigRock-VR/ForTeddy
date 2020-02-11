using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats:")]
    [SerializeField] public int MaxHP;
    [SerializeField] public int AttackDamage;
    [SerializeField] public float MovementSpeed;
    [SerializeField] [Range(50.0f, 400.0f)] public float AttackSpeed = 50.0f;
    [SerializeField] public int attackRange = 2;
    [SerializeField] public bool isEnableWaveMult; // Enable Wave Multiplyer and increase enemy damage and hp every wave
    [SerializeField] public bool isDead;

    // BOSS LOGICS
    [SerializeField] public bool isBoss;
    private Collider[] hitColliders;
    private readonly float BOSS_ATT_RANGE = 5.0f;
    public enum eTargets { PLAYER, BED, SOLDIER };
    public eTargets targetPriority;

    private WaveManager waveManager;

    [Range(0.5f, 2.0f)]
    public float deathDelay = 0.5f;

    public GameObject pickUpPrefab; // Prefab that can spawn the enemy on death


    private enum eState { TARGET_PLAYER, TARGET_SOLDIER, ATTACK_PLAYER, ATTACK_SOLDIER, ATTACK_BED, DEATH };
    private eState enemyState;
    private int hp;
    private int damage;
    private double attackSpeed;
    private double nextTimeToAttack;
    private NavMeshAgent agent;
    private Transform targetPosition;

    void Start()
    {
        waveManager = GameManager.Instance.waveManager.GetComponent<WaveManager>();

        if (waveManager)
        {
            waveManager.onEndWave += KillAll;
        }

        InitEnemyStats();
    }


    private void Update()
    {
        if (isBoss)
        {
            CheckBossPriority();
        }
        else
        {
            CheckEnemyState();
        }

    }

    /*
     * BOSS LOGICS
     */
    private void CheckBossState()
    {
        hitColliders = Physics.OverlapSphere(transform.position, BOSS_ATT_RANGE);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform.CompareTag("Soldier"))
            {
                enemyState = eState.TARGET_SOLDIER;
                targetPosition = hitColliders[i].transform;
            }
            else if (hitColliders[i].transform.CompareTag("Player"))
            {
                enemyState = eState.TARGET_PLAYER;
                targetPosition = hitColliders[i].transform;
            }
        }

        EnemyAI();

    }

    private void CheckBossPriority()
    {
        switch (targetPriority)
        {
            case eTargets.PLAYER:
                targetPosition = GameManager.Instance.player?.transform;
                Debug.DrawLine(transform.position, targetPosition.position, Color.blue);
                agent.SetDestination(targetPosition.position);
                break;
            case eTargets.BED:
                targetPosition = waveManager.bedPositions;
                Debug.DrawLine(transform.position, targetPosition.position, Color.yellow);
                agent.SetDestination(targetPosition.position);
                break;
            case eTargets.SOLDIER:
                float distance = GetDistanceFromSoldiers();
                Debug.DrawLine(transform.position, targetPosition.position, Color.red);
                agent.SetDestination(targetPosition.position);
                break;
        }
    }

    private void CheckEnemyState()
    {
        float playerDist = GetDistanceFromPlayer();
        float soldierDist = GetDistanceFromSoldiers();
        if (playerDist < soldierDist)
        {
            enemyState = eState.TARGET_PLAYER;
            targetPosition = GameManager.Instance.player.transform;

            if (playerDist <= attackRange && !GameManager.Instance.player.GetComponent<PlayerManager>().isDead)
            {
                if (Time.time >= nextTimeToAttack)
                {
                    nextTimeToAttack = Time.time + attackSpeed;
                    enemyState = eState.ATTACK_PLAYER;
                }
            }
        }
        else
        {
            enemyState = eState.TARGET_SOLDIER;

            if (soldierDist <= attackRange)
            {
                if (Time.time >= nextTimeToAttack)
                {
                    nextTimeToAttack = Time.time + attackSpeed;
                    enemyState = eState.ATTACK_SOLDIER;
                }
            }
        }

        EnemyAI();
    }

    private void EnemyAI()
    {
        switch (enemyState)
        {
            case eState.TARGET_PLAYER:
                Debug.DrawLine(transform.position, targetPosition.position, Color.red);
                agent.SetDestination(targetPosition.position);
                break;

            case eState.TARGET_SOLDIER:
                Debug.DrawLine(transform.position, targetPosition.position, Color.blue);
                agent.SetDestination(targetPosition.position);
                break;

            case eState.ATTACK_PLAYER:
                GameManager.Instance.player.GetComponent<PlayerManager>().TakeDamage(damage);
                /* TO DO ANIMATION */
                break;

            case eState.ATTACK_SOLDIER:
                targetPosition.GetComponent<SoldierManager>().TakeDamage(damage);
                /* TO DO ANIMATION */
                break;

            case eState.DEATH:
                if (isBoss)
                {
                    waveManager.UpdateWave();
                }
                agent.isStopped = true;
                break;
            default:
                break;
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Taking damage: {damage}, HP: {hp}");
        if (hp <= 0)
        {
            isDead = true;
            enemyState = eState.DEATH;
            waveManager.onEndWave -= KillAll;
            Dead();
        }
    }


    // Method Overload for the hit point effect
    public void TakeDamage(int damage, Vector4 hitPoint)
    {
        hp -= damage;

        Material mat = GetComponent<MeshRenderer>().material;

        float currEmissionAmount = mat.GetFloat("_EmissionScale");
        float nextEmissionAmount = (float)damage / MaxHP;
        nextEmissionAmount += currEmissionAmount;

        StartCoroutine(EmissionEffect(hitPoint, 0.7f, currEmissionAmount, nextEmissionAmount));
        Debug.Log($"Taking damage: {damage}, HP: {hp}");
    }

    IEnumerator EmissionEffect(Vector4 hitPoint, float delay, float currEmission, float nextEmission)
    {
        float timer = 0.0f;
        Material mat = GetComponent<MeshRenderer>().material;
        Vector4 localHitPoint = mat.GetVector("_DissolveStart");

        if (localHitPoint == Vector4.zero)
        {
            mat.SetVector("_DissolveStart", hitPoint);
            mat.SetVector("_DissolveEnd", hitPoint * -1);
            mat.SetFloat("_isHitting", 1);
        }

        if (hp <= 0 && !isDead)
        {
            isDead = true;
            enemyState = eState.DEATH;
            Vector3 pickUpSpawnPos = transform.position;
            pickUpSpawnPos.y += 10.0f;
            Instantiate(pickUpPrefab, pickUpSpawnPos, Quaternion.identity, waveManager.coinsContainer.transform);
            StartCoroutine(PlayFullDissolveEffect(2.0f));
        }

        while (timer < 1.0f && !isDead)
        {
            timer += Time.deltaTime / delay;
            mat.SetFloat("_EmissionScale", Mathf.Lerp(currEmission, nextEmission, timer));
            yield return null;
        }

    }
    IEnumerator PlayFullDissolveEffect(float delay)
    {
        float timer = 0.0f;
        Material mat = GetComponent<MeshRenderer>().material;
        Vector4 localHitPoint = mat.GetVector("_DissolveStart");
        mat.SetFloat("_isDissolving", 1);

        if (localHitPoint == Vector4.zero)
        {
            mat.SetVector("_DissolveStart", new Vector4(0.0f, 0.5f, 0.0f, 0.0f));
            mat.SetVector("_DissolveEnd", new Vector4(0.0f, -0.5f, 0.0f, 1.0f));
            mat.SetFloat("_isHitting", 1);
            mat.SetFloat("_isDissolving", 1);
        }

        while (timer < 1.0f)
        {
            timer += Time.deltaTime / delay;
            mat.SetFloat("_DissolveScale", Mathf.Lerp(0, 1, timer));
            yield return null;
        }
        waveManager.onEndWave -= KillAll;
        Destroy(gameObject);
    }

    private float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
    }

    private float GetDistanceFromSoldiers()
    {
        float result = float.MaxValue;

        foreach (var soldier in waveManager.soldierPositions)
        {
            if (soldier.gameObject.activeInHierarchy)
            {
                float minDistance = Vector3.Distance(transform.position, soldier.position);

                if (minDistance < result)
                {
                    result = minDistance;
                    targetPosition = soldier;
                }
            }

        }

        return result;
    }

    private void InitEnemyStats()
    {
        if (isEnableWaveMult)
        {
            hp = this.MaxHP + waveManager.waveCount;
            damage = this.AttackDamage + waveManager.waveCount;
            attackSpeed = Math.Round(1.0f / (this.AttackSpeed / 100), 2); // Attack Speed in percent value
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = this.MovementSpeed;
            enemyState = eState.TARGET_PLAYER;
        }
        else
        {
            hp = this.MaxHP;
            damage = this.AttackDamage;
            attackSpeed = Math.Round(1.0f / (this.AttackSpeed / 100), 2); // Attack Speed in percent value
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = this.MovementSpeed;
            enemyState = eState.TARGET_PLAYER;
        }

    }
    public void Dead()
    {
        Destroy(gameObject, deathDelay);
    }
   
    public void KillAll()
    {
        StartCoroutine(PlayFullDissolveEffect(2.0f));
    }
}
