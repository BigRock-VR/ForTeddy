﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RektifierExplosion : MonoBehaviour
{
    public float maxSpeed = 6.0f;
    private float minSpeed = 2.0f;
    private float speed;
    public float explosionTime = 3.0f;
    public float explosionRadius = 10.0f;
    public bool isExploded;
    public float attractionForce = 100.0f;
    private Collider[] hitColliders;
    public List<Transform> enemyTargets = new List<Transform>();

    public Vector3 direction;
    public ParticleSystem pSystem;
    private ParticleSystem.Particle[] particles;
    void Start()
    {
        speed = maxSpeed;
        pSystem = GetComponent<ParticleSystem>();
        InitializeIfNeeded();
    }

    private void FixedUpdate()
    {
        if (explosionTime > 0 && !isExploded)
        {
            speed = Mathf.Clamp(speed, minSpeed, speed - (Time.deltaTime * 3.0f));
            transform.position += direction * Time.fixedDeltaTime * speed;
            explosionTime -= Time.fixedDeltaTime;
            hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i])
                {
                    if (hitColliders[i].transform.CompareTag("Enemy"))
                    {
                        Vector3 forceDirection = transform.position - hitColliders[i].transform.position;
                        Enemy e = hitColliders[i].transform.GetComponent<Enemy>();
                        float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                        if (distance > 1)
                        {
                            Rigidbody rb = hitColliders[i].attachedRigidbody;
                            rb.AddForce(forceDirection.normalized * (attractionForce * Time.fixedDeltaTime) / distance );
                        }
                        else
                        {
                            //KIll the enemy that are in the center of the explosion radius
                            e?.KillAll();
                        }

                    }
                }
            }
        }

        if (explosionTime <= 0 && !isExploded)
        {
            pSystem.Play();
            isExploded = true;
            GetEnemyTargets();
            if (enemyTargets.Count > 0)
            {
                StartCoroutine(ParticlesAiming());
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
    // Initialize the particle array
    void InitializeIfNeeded()
    {
        if (particles == null || particles.Length < pSystem.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[pSystem.main.maxParticles];
        }
    }


    // Move the particles to the frist enemy target stille alive in the explosion radius
    IEnumerator ParticlesAiming()
    {
        float timer = 0.0f;
        while (timer < 1)
        {
            timer += Time.fixedDeltaTime / 2.0f;
            int numParticlesAlive = pSystem.GetParticles(particles);

            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].position = Vector3.Lerp(particles[i].position, enemyTargets[0].position, timer);
            }

            pSystem.SetParticles(particles, numParticlesAlive);
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void GetEnemyTargets()
    {
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i])
            {
                if (hitColliders[i].transform.CompareTag("Enemy"))
                {
                    enemyTargets.Add(hitColliders[i].transform);
                }
            }
        }
    }
}