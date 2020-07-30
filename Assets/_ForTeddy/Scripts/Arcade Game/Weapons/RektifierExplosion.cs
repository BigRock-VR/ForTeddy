using System.Collections;
using UnityEngine;
using Cinemachine;

public class RektifierExplosion : MonoBehaviour
{
    public float maxSpeed = 6.0f;
    public AnimationCurve explosionCurve;
    public float explosionRadius = 6.0f;
    public bool isExploded;
    public float attractionForce = 40000.0f;
    public float explosionForce = 6000.0f;
    public int explosionDamage = 10;
    [HideInInspector] public Vector3 direction;
    public ParticleSystem pSystemExplosion, pSystemMissile;
    public ParticleSystem pSystemBase;
    //private ParticleSystem.Particle[] particles;
    private float currExplosionRadius = 0.0f;
    private float minSpeed = 2.0f;
    private float speed;
    private float explosionTimer = 0.0f;
    private float explosionDuration = 3.0f;
    private Collider[] hitColliders;
    private CinemachineImpulseSource cameraImpulse;
    void Start()
    {
        speed = maxSpeed;
        pSystemBase.Play();
        explosionDuration = explosionCurve.keys[explosionCurve.keys.Length - 1].time;
        cameraImpulse = GetComponent<CinemachineImpulseSource>();
        //InitializeIfNeeded();
    }

    private void FixedUpdate()
    {
        if (explosionTimer < explosionDuration && !isExploded)
        {
            speed = Mathf.Clamp(speed, minSpeed, speed - (Time.deltaTime * 3.0f));

            transform.position += direction * Time.fixedDeltaTime * speed;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, explosionCurve.Evaluate(explosionTimer));
            currExplosionRadius = Mathf.Lerp(0, explosionRadius, explosionCurve.Evaluate(explosionTimer));
            explosionTimer += Time.fixedDeltaTime;
            hitColliders = Physics.OverlapSphere(transform.position, currExplosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i])
                {
                    if (hitColliders[i].transform.CompareTag("Enemy"))
                    {
                        Vector3 forceDirection = transform.position - hitColliders[i].transform.position;
                        Enemy e = hitColliders[i].transform.GetComponent<Enemy>();
                        float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                        if (distance > 1 && !e.isBoss)
                        {
                            Rigidbody rb = hitColliders[i].attachedRigidbody;
                            rb.AddForce(forceDirection.normalized * (attractionForce * Time.fixedDeltaTime) / distance);
                        }
                        else
                        {
                            if (!e.isDead && !e.isBoss)
                            {
                                //KIll the enemy that are in the center of the explosion radius
                                e?.TakeDamage(e.MaxHP, new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
                            }
                        }

                    }
                }
            }
        }

        if (explosionTimer > explosionDuration && !isExploded)
        {
            DisableBaseParticle();
            pSystemExplosion.Play();
            isExploded = true;
            StartCoroutine(ApplyExplosionDamage());
            Destroy(gameObject, 5.0f);
        }

    }

    private void DisableBaseParticle()
    {
        var pSystemMain = pSystemBase.main;
        pSystemMain.loop = false;

        for (int i = 0; i < pSystemBase.transform.childCount; i++)
        {
            var childParticle = pSystemBase.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (childParticle != null)
            {
                var particleMain = childParticle.main;
                particleMain.loop = false;
            }
        }
    }

    // Initialize the particle array
    //void InitializeIfNeeded()
    //{
    //    if (particles == null || particles.Length < 50)
    //    {
    //        particles = new ParticleSystem.Particle[50];
    //    }
    //}


    // Move the particles to the frist enemy target stille alive in the explosion radius
    //IEnumerator ParticlesAiming()
    //{
    //    float timer = 0.0f;
    //    while (timer < 1)
    //    {
    //        timer += Time.fixedDeltaTime / 2.0f;
    //        int numParticlesAlive = pSystemMissile.GetParticles(particles);

    //        for (int i = 0; i < numParticlesAlive; i++)
    //        {
    //            if (enemyTargets[0] != null)
    //            {
    //                particles[i].position = Vector3.Lerp(particles[i].position, enemyTargets[0].position, timer);
    //            }
    //        }

    //        pSystem.SetParticles(particles, numParticlesAlive);
    //        yield return null;
    //    }
    //    Destroy(gameObject);
    //}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currExplosionRadius);
    }

    IEnumerator ApplyExplosionDamage()
    {
        float t = 0.0f; // TIMER
        currExplosionRadius = 0.0f;
        while (t < 1)
        {
            t += Time.deltaTime / 2.0f; //  2 SECONDS OF EXPLOSION

            if (t >= 0.5f)
            {
                cameraImpulse.GenerateImpulse();
            }

            hitColliders = Physics.OverlapSphere(transform.position, currExplosionRadius);
            currExplosionRadius = Mathf.Lerp(0, explosionRadius, t);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i] != null && hitColliders[i].CompareTag("Enemy"))
                {
                    Enemy e = hitColliders[i].gameObject.GetComponent<Enemy>();
                    if (!e.isDead)
                    {
                        e.TakeDamage(explosionDamage);
                        hitColliders[i].attachedRigidbody.AddForce((-hitColliders[i].transform.forward) * (explosionForce * Time.deltaTime), ForceMode.Impulse);
                    }
                }
            }
            yield return null;
        }
    }
}
