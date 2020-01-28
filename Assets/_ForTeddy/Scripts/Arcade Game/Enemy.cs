using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] public int Health = 10;
    [SerializeField] public int Damage = 1;
    [SerializeField] public float speed = 5.0f;
    public bool enableWaveMultiply;
    public WaveManager waveManager;

    [Range(0.5f, 2.0f)]
    public float deathDelay = 0.5f;

    private int hp;
    private int damage;
    private NavMeshAgent agent;
    public float distanceFromPlayer;
    private Vector3 targetPosition;

    void Start()
    {
        waveManager = GameManager.Instance.waveManager.GetComponent<WaveManager>();

        if (waveManager)
        {
            waveManager.onEndWave += Dead;
        }

        hp = Health;
        damage = Damage;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SetTarget(GameManager.Instance.player.transform);
    }

    private void Update()
    {
        if (GameManager.Instance.player)
        {
            distanceFromPlayer = GetDistanceFromPlayer();

            //if (distanceFromPlayer > 1)
            //{
            //    SetTarget(GameManager.Instance.player.transform);
            //}
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


    private void SetTarget(Transform target)
    {
        targetPosition = target.position;
        agent.SetDestination(targetPosition);
    }

    private float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, GameManager.Instance.player.transform.position);
    }

    public void Dead()
    {
        Destroy(gameObject, deathDelay);
    }
}
