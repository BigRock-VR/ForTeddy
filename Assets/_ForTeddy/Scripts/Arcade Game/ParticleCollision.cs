using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem pSystem;
    public List<ParticleCollisionEvent> collisionEvents; //Store all the collission events of the particle
    public int damage;
    public float pushForce;

    private const float PUSH_FORCE_MULTIPLIER = 1000.0f;


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
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (_enemy)
            {
                //_enemy.TakeDamage(damage); // SEND COLLISION EVENT
                Vector4 hitPointLocal = collisionEvents[collisionEventCount-1].normal;
                _enemy.TakeDamage(damage * collisionEventCount, hitPointLocal);
                if (!_enemy.isBoss)
                {
                    rb.AddForce((-collisionEvents[collisionEventCount-1].normal) * (pushForce * PUSH_FORCE_MULTIPLIER ) * Time.deltaTime, ForceMode.Impulse);
                }
            }
        }
    }
}
