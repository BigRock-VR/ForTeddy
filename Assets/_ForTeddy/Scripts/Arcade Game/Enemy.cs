using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats:")]
    [SerializeField] public int Health = 10;
    [SerializeField] public int AttackDamage = 1;
    [SerializeField] public float MovementSpeed = 4.0f;
    [SerializeField] [Range(50.0f, 400.0f)] public float AttackSpeed = 50.0f;
    [SerializeField] public int attackRange = 2;

    public WaveManager waveManager;

    [Range(0.5f, 2.0f)]
    public float deathDelay = 0.5f;


    private enum eState { TARGET_PLAYER, TARGET_SOLDIER, ATTACK_PLAYER, ATTACK_SOLDIER, DEATH };
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
            waveManager.onEndWave += Dead;
        }

        hp = this.Health;
        damage = this.AttackDamage;
        attackSpeed = Math.Round(1.0f / (this.AttackSpeed / 100), 2); // Attack Speed in percent value
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = this.MovementSpeed;
        enemyState = eState.TARGET_PLAYER;
    }

    private void Update()
    {
        CheckEnemyState();
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
                Debug.Log("Death");
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
            Dead();
        }
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

    public void Dead()
    {
        waveManager.onEndWave -= Dead;
        Destroy(gameObject, deathDelay);
    }
}
