//#define TESTMODE
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private const int PLAYER_MAX_HP = 1000;
    private const int PLAYER_MAX_ARMOR = 500;
    private const int PLAYER_ARMOR_PCT = 70; // Damage taken will be reduced by this percent


    public int hp;
    public int armor;
    public int score;
    public int scoreMultiplyer = 1;

    public bool hasArmor;
    public bool isDead;
#if TESTMODE
    private bool isGod;
#endif

    private void Start()
    {
        hp = PLAYER_MAX_HP;
        armor = PLAYER_MAX_ARMOR;
        score = 0;
        scoreMultiplyer = 1;
        hasArmor = true;
    }

    
    private void Dead()
    {
        /* TODO DEATH ANIMATION AND UI */
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<WeaponSystem>().enabled = false;
    }
    
    public void ReloadPlayerHP()
    {
        hp = PLAYER_MAX_HP;
    }

    public void ReloadPlayerArmor()
    {
        hp = PLAYER_MAX_ARMOR;
    }

    public void UpdatePlayerScore(int amount)
    {
        score += amount;
    }

    public void TakeDamage(int damage)
    {
#if TESTMODE
        if (isGod)
        {
            return;
        }
#endif
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
            Debug.Log($"TOTAL DAMAGE: {damage}");
            return;
        }

        int armorAbsorbedDmg = (damage * PLAYER_ARMOR_PCT) / 100; // Damage that the armor should absorb
        int hpDamage = damage - armorAbsorbedDmg;

        Debug.Log($"TOTAL DAMAGE: {damage} ABSORBED DAMAGE: {armorAbsorbedDmg} HP DAMAGE: {hpDamage}");


        // If the damage that the armor should abosord is greather than the armor the damage will send to the player HP
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

#if TESTMODE
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 100, 100, 50), "God Mode"))
        {
            isGod = !isGod;
        }
    }
#endif

}
