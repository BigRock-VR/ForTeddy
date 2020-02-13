using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class SoldierManager : MonoBehaviour
{
    public readonly int SOLDIER_MAX_HP = 1000;
    public readonly int SOLDIER_MAX_ARMOR = 500;
    private const int SOLDIER_ARMOR_PCT = 70; // Damage taken will be reduced by this percent

    public int hp;
    public int armor;

    public bool hasArmor;
    public bool isDead;
    public bool isAiming;

    public List<Transform> enemyTargets = new List<Transform>();

    public Transform currTarget;

    private void Start()
    {
        hp = SOLDIER_MAX_HP;
        armor = SOLDIER_MAX_ARMOR;
        hasArmor = true;
    }

    private void Update()
    {
        if (currTarget && !currTarget.GetComponent<Enemy>().isDead)
        {
            if (!isAiming)
            {
                isAiming = true;
                Vector3 targetPosition = new Vector3(currTarget.transform.position.x, transform.position.y, currTarget.transform.position.z);
                transform.LookAt(targetPosition);
            }
            else
            {
                Vector3 targetPosition = new Vector3(currTarget.transform.position.x, transform.position.y, currTarget.transform.position.z);
                transform.LookAt(targetPosition);
            }
        }
        else
        {
            currTarget = getNextTarget();
            isAiming = false;
        }
    }

    public void ReloadSoldierHP()
    {
        hp = SOLDIER_MAX_HP;
    }

    public void ReloadSoldierArmor()
    {
        hp = SOLDIER_MAX_ARMOR;
    }

    private Transform getNextTarget()
    {
        Transform result = null;
        for (int i = 0; i < enemyTargets.Count; i++)
        {
            if (!enemyTargets[i])
            {
                enemyTargets.RemoveAt(i);
                continue;
            }

            result = enemyTargets[i].transform;
        }
        return result;
    }

    /* TRIGGER DETECTION */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyTargets.Add(other.transform);
            if (!currTarget)
            {
                currTarget = other.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemyTargets.Contains(other.transform))
            {
                enemyTargets.Remove(other.transform);
            }
        }
    }

    public void TakeDamage(int damage)
    {

        if (!hasArmor && !isDead)
        {
            int nextHp = hp - damage;

            if (nextHp <= 0)
            {
                nextHp = 0;
                isDead = true;
                Dead();
            }

            hp = nextHp;
            Debug.Log($"(SOLDIER): HP DAMAGE: {damage}");
            return;
        }

        int armorAbsorbedDmg = (damage * SOLDIER_ARMOR_PCT) / 100; // Damage that the armor should absorb
        int hpDamage = damage - armorAbsorbedDmg;

        Debug.Log($"(SOLDIER): TOTAL DAMAGE: {damage} ABSORBED DAMAGE: {armorAbsorbedDmg} HP DAMAGE: {hpDamage}");


        // If the damage that the armor should abosord is greather than the armor the damage will send to the soldier HP
        if (armorAbsorbedDmg >= armor)
        {
            int wastedArmorDamage = Mathf.Abs(armor - armorAbsorbedDmg);
            armor = 0;
            hasArmor = false;
            TakeDamage(wastedArmorDamage);
            return;
        }

        armor -= armorAbsorbedDmg;
        hp -= hpDamage;
    }

    private void Dead()
    {
        /* TODO DEATH ANIMATION AND UI */
        gameObject.SetActive(false);
        //gameObject.GetComponent<WeaponSystem>().enabled = false;
    }
}
