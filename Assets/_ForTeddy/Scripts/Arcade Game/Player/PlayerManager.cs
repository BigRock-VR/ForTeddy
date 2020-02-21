//#define TESTMODE
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public readonly int PLAYER_MAX_HP = 1000;
    public readonly int PLAYER_MAX_ARMOR = 500;
    private const int PLAYER_ARMOR_PCT = 70; // Damage taken will be reduced by this percent


    public int hp = 1;
    public int armor;
    public int score;
    public int scoreMultiplyer = 1;

    public bool hasArmor;
    public bool isDead;
    public Renderer meshRenderer;
    public AnimationCurve hitAnimationCurve;
    private Material mat;

    public delegate void DeathEvent();
    public event DeathEvent onPlayerDeath;
#if TESTMODE
    private bool isGod;
#endif

    private void Start()
    {
        hp = PLAYER_MAX_HP;
        armor = 0;
        score = 0;
        scoreMultiplyer = 1;
        mat = meshRenderer.material;
    }

    
    private void Dead()
    {
        onPlayerDeath?.Invoke();
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<WeaponSystem>().enabled = false;
    }
    
    public void ReloadPlayerHP()
    {
        hp = PLAYER_MAX_HP;
    }

    public void ReloadPlayerArmor()
    {
        armor = PLAYER_MAX_ARMOR;
        hasArmor = true;
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
        StartCoroutine(PlayHitEffect());
        if (!hasArmor && !isDead)
        {
            int nextHp = hp - damage;

            if (nextHp <= 0 && !isDead)
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
    IEnumerator PlayHitEffect()
    {
        float t = 0.0f; // time
        mat.SetInt("_fresnelscale0off1on", 1);

        if (hasArmor)
        {
            mat.SetInt("_Colorchanger", 1);
        }
        else
        {
            mat.SetInt("_Colorchanger", 0);
        }

        while (t < 1)
        {
            t += Time.deltaTime * 2;
            mat.SetFloat("_Fresnel_Power", Mathf.Lerp(0, 1, hitAnimationCurve.Evaluate(t)));
            yield return null;
        }
        mat.SetInt("_fresnelscale0off1on", 0);
        mat.SetFloat("_Fresnel_Power", 0);
    }
}
