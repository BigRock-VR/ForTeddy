using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem pSystem;
    public List<ParticleCollisionEvent> collisionEvents; //Store all the collission events of the particle
    public delegate void EnemyCollision(Enemy enemy);
    public event EnemyCollision onCollisionWithEnemy;

    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int collisionEventCount = pSystem.GetCollisionEvents(other, collisionEvents);
        Debug.Log($"Particle Collision with {other.transform.name}, count of collisions {collisionEventCount}");

        if (other.CompareTag("Enemy"))
        {
            Enemy _enemy = other.GetComponent<Enemy>();

            if (_enemy)
            {
                onCollisionWithEnemy(_enemy); // SEND COLLISION EVENT
            }
        }
    }
}
