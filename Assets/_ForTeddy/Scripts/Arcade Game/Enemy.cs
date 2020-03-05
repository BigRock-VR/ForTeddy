using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    /*
     * Mob Information
     */
    [Header("Mob Stats:")]
    [SerializeField] public int MaxHP;
    [SerializeField] public int AttackDamage;
    [SerializeField] public float MovementSpeed;
    [SerializeField] [Range(20.0f, 200.0f)] public float AttackSpeed = 50.0f;
    [SerializeField] public float attackRange = 2;
    [SerializeField] public bool isEnableWaveMult; // Enable Wave Multiplyer and increase enemy damage and hp every wave
    [SerializeField] public bool isDead;
    public Transform visionAnchorPoint; // ENEMY VISION POSITION TO ATTACKING AND FACING THE PLAYER
    [Range(0.2f,10)] public float visionRange = 0.5f;
    public Transform[] attackAnchorPoint; // ENEMY HIT POINT
    [Range(0.1f, 5)] public float hitPointRange = 0.2f;
    public AnimationCurve hitAnimationCurve;
    public ParticleSystem pSystemAttack; // ENEMY ATTACK VFX

    /*
     * DROP INFORMATION
     */
    [Header("Drop Info:")]
    public GameObject pickUpPrefab; // Prefab that can spawn the enemy on death
    [Range(1, 20)] public int minDropPickUp = 1; // The min number of pick ups that can drop the enemy
    [Range(1, 30)] public int maxDropPickUp = 1; // The max number of pick ups that can drop the enemy

    /*
     * Boss Information
     */
    [Header("Boss Stats:")]
    [SerializeField] public bool isBoss;
    private Collider[] hitColliders;
    private readonly float BOSS_ATT_RANGE = 5.0f;
    public enum eTargets { PLAYER, BED, SOLDIER };
    public eTargets targetPriority;


    private WaveManager waveManager;
    private enum eState { TARGET_PLAYER, TARGET_SOLDIER, ATTACK_PLAYER, ATTACK_SOLDIER, ATTACK_BED, DEATH };
    private eState enemyState;
    public int hp;
    private int damage;
    private double attackSpeed;
    private double nextTimeToAttack;
    private NavMeshAgent agent;
    private Transform targetPosition;
    private Animator anim;
    private const float EMISSION_HIT_INTENSITY = 50.0f;
    private bool canDoDamage;
    private bool hasAttack;
    private Collider[] _hitCollider;
    void Start()
    {
        waveManager = GameManager.Instance.waveManager.GetComponent<WaveManager>();

        if (waveManager)
        {
            waveManager.onEndWave += KillAll;
        }

        InitEnemyStats();
        anim = GetComponent<Animator>();
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

        if (!hasAttack)
        {
            CheckCanDoDamage();
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
                if (CanDoAttackAnimation() && !isDead)
                {
                    if (Time.time >= nextTimeToAttack)
                    {
                        transform.LookAt(targetPosition);
                        nextTimeToAttack = Time.time + attackSpeed;
                        anim.SetTrigger("Attack");
                    }
                }
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

    public bool CanDoAttackAnimation()
    {
        bool result = false;
        hitColliders = Physics.OverlapSphere(visionAnchorPoint.position, visionRange, LayerMask.GetMask("Player"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Player") || hitColliders[i].CompareTag("Soldier"))
            {
                result = true;
            }
        }
        return result;
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
                if (CanDoAttackAnimation() && !isDead)
                {
                    anim.SetTrigger("Attack");
                    if (pSystemAttack)
                    {
                        pSystemAttack.Play();
                    }
                }
                break;
            case eState.ATTACK_SOLDIER:
                if (CanDoAttackAnimation() && !isDead)
                {
                    anim.SetTrigger("Attack");
                    if (pSystemAttack)
                    {
                        pSystemAttack.Play();
                    }
                }
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

    private void CheckCanDoDamage()
    {
        if (canDoDamage)
        {
            for (int i = 0; i < attackAnchorPoint.Length; i++)
            {
                _hitCollider = Physics.OverlapSphere(attackAnchorPoint[i].position, hitPointRange);
                for (int j = 0; j < _hitCollider.Length; j++)
                {
                    if (_hitCollider[j].CompareTag("Player"))
                    {
                        if (!_hitCollider[j].gameObject.GetComponent<PlayerManager>().isDead)
                        {
                            _hitCollider[j].gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
                            hasAttack = true;
                        }
                    }
                    if (_hitCollider[j].CompareTag("Soldier"))
                    {
                        if (!_hitCollider[j].gameObject.GetComponent<SoldierManager>().isDead)
                        {
                            _hitCollider[j].gameObject.GetComponent<SoldierManager>().TakeDamage(damage);
                            hasAttack = true;
                        }
                    }
                }
            }
        }
    }

    public void SetCanDoDamage()
    {
        canDoDamage = true;
    }

    public void UnSetCanDoDamage()
    {
        canDoDamage = false;
        hasAttack = false;
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;

        Material mat = GetComponent<SkinnedMeshRenderer>().material;

        //float currEmissionAmount = mat.GetFloat("_EmissionScale");
        //float nextEmissionAmount = (float)damage / MaxHP;
        //nextEmissionAmount += currEmissionAmount;

        //StartCoroutine(EmissionEffect(hitPoint, 0.5f, currEmissionAmount, nextEmissionAmount));
        StartCoroutine(EmissionEffect(mat, 0.5f));
        Debug.Log($"Taking damage: {damage}, HP: {hp}");
    }

    public void TakeDamage(int damage, Vector4 hitPoint)
    {
        hp -= damage;

        Material mat = GetComponent<SkinnedMeshRenderer>().material;

        //float currEmissionAmount = mat.GetFloat("_EmissionScale");
        //float nextEmissionAmount = (float)damage / MaxHP;
        //nextEmissionAmount += currEmissionAmount;

        //StartCoroutine(EmissionEffect(hitPoint, 0.5f, currEmissionAmount, nextEmissionAmount));
        StartCoroutine(EmissionEffect(mat, 0.5f));
        Debug.Log($"Taking damage: {damage}, HP: {hp}");
    }
    IEnumerator EmissionEffect(Material mat, float delay)
    {
        float timer = 0.0f;
        if (hp <= 0 && !isDead)
        {
            OnDeath();

            StartCoroutine(PlayFullDissolveEffect(2.0f));
        }

        while (timer < 1.0f && !isDead)
        {
            timer += Time.deltaTime / delay;
            // Hit Effect on the mob;
            mat.SetColor("_Emicolor", Color.Lerp(Color.white, Color.red * EMISSION_HIT_INTENSITY, hitAnimationCurve.Evaluate(timer * 4.0f)));
            yield return null;
        }

    }

    /*
     * EMISSION CORROSION BASE ON THE ENEMY HP
     */
    IEnumerator EmissionEffect(Vector4 hitPoint, float delay, float currEmission, float nextEmission)
    {
        float timer = 0.0f;
        Material mat = GetComponent<SkinnedMeshRenderer>().material;
        Vector4 localHitPoint = mat.GetVector("_DissolveStart");

        if (localHitPoint == Vector4.zero)
        {
            mat.SetVector("_DissolveStart", hitPoint);
            mat.SetVector("_DissolveEnd", hitPoint * -1);
            mat.SetFloat("_isHitting", 1);
        }

        if (hp <= 0 && !isDead)
        {
            OnDeath();
            StartCoroutine(PlayFullDissolveEffect(2.0f));
        }

        while (timer < 1.0f && !isDead)
        {
            timer += Time.deltaTime / delay;
            mat.SetFloat("_EmissionScale", Mathf.Lerp(currEmission, nextEmission, timer));
            // Hit Effect on the mob;
            mat.SetColor("_Emicolor",Color.Lerp(Color.white, Color.red * EMISSION_HIT_INTENSITY, hitAnimationCurve.Evaluate(timer*4.0f)));
            yield return null;
        }

    }

    private void OnDeath(bool isEndWave = false)
    {
        if (isBoss && !isDead)
        {
            waveManager.UpdateWave();
            isDead = true;
            enemyState = eState.DEATH;
            agent.isStopped = true;
        }

        isDead = true;
        enemyState = eState.DEATH;
        agent.isStopped = true;
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        if (!isEndWave)
        {
            DropPickUp();
        }

        CapsuleCollider enemyCollider = GetComponent<CapsuleCollider>();

        if (enemyCollider)
        {
            enemyCollider.isTrigger = true;
        }
    }

    private void DropPickUp()
    {
        if (maxDropPickUp > 1)
        {
            int dropPct = UnityEngine.Random.Range(minDropPickUp, maxDropPickUp);
            for (int i = 0; i < dropPct; i++)
            {
                Vector2 randRadius = UnityEngine.Random.insideUnitCircle;
                Vector3 randPosition = new Vector3(transform.position.x + randRadius.x, transform.position.y, transform.position.z + randRadius.y);
                Instantiate(pickUpPrefab, randPosition, Quaternion.identity, waveManager.coinsContainer.transform);
            }
        }
        else
        {
            Instantiate(pickUpPrefab, transform.position, Quaternion.identity, waveManager.coinsContainer.transform);
        }
    }

    IEnumerator PlayFullDissolveEffect(float delay)
    {
        float timer = 0.0f;
        Material mat = GetComponent<SkinnedMeshRenderer>().material;
        Vector4 localHitPoint = mat.GetVector("_DissolveStart");
        mat.SetFloat("_isDissolving", 1);
        float emissionScale = mat.GetFloat("_EmissionScale");
        OnDeath(true);
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

            if (emissionScale == 0)
            {
                mat.SetFloat("_EmissionScale", Mathf.Lerp(0, 1, timer));
            }
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
            this.MaxHP = this.MaxHP + waveManager.waveCount;
            hp = this.MaxHP;
            damage = this.AttackDamage; //+ waveManager.waveCount;
            attackSpeed = Math.Round(1.0f / (this.AttackSpeed / 100), 2); // Attack Speed in percent value
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = this.MovementSpeed;
            agent.stoppingDistance = attackRange;
            enemyState = eState.TARGET_PLAYER;
        }
        else
        {
            hp = this.MaxHP;
            damage = this.AttackDamage;
            attackSpeed = Math.Round(1.0f / (this.AttackSpeed / 100), 2); // Attack Speed in percent value
            agent = this.GetComponent<NavMeshAgent>();
            agent.speed = this.MovementSpeed;
            agent.stoppingDistance = attackRange;
            enemyState = eState.TARGET_PLAYER;
        }

    }
   
    public void KillAll()
    {
        StartCoroutine(PlayFullDissolveEffect(2.0f));
    }

    private void OnDrawGizmosSelected()
    {
        if (gameObject)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(visionAnchorPoint.position,visionRange);
            Gizmos.color = Color.red;
            for (int i = 0; i < attackAnchorPoint.Length; i++)
            {
                Gizmos.DrawWireSphere(attackAnchorPoint[i].position, hitPointRange);
            }
        }
    }
}
