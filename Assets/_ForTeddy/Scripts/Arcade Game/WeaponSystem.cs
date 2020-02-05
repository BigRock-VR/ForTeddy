#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    //Scritable Objects
    [SerializeField] public Weapon[] weapons = new Weapon[MAX_WEAPONS];
    private GameObject[] weaponObjs = new GameObject[MAX_WEAPONS];

    public Transform weaponSpawnPosition;
    public enum eWeapons {DEFAULT, DAKKAGUN, IMPALLINATOR, ATOMIZER, REKTIFIER};
    public eWeapons currSelectedWeapon = eWeapons.DEFAULT;

    private const int MAX_WEAPONS = 5;
    private float nextTimeToFire = 0.0f;
    public bool isSoldier;

    //Particle Bullets
    private ParticleSystem pSystem;

    void Start()
    {
        InitWeapons(); // Instantiate all the possible weapons
    }

    private void Update()
    {
        if (isSoldier)
        {
            if (GetComponent<SoldierManager>().isAiming)
            {
                Shoot();
            }
            return;
        }
        else
        {
            if (gameObject.GetComponent<PlayerMovement>().isAiming)
            {
                Shoot();
            }
        }

    }

    private void Shoot()
    {
        switch (weapons[(int)currSelectedWeapon].fireType)
        {
            case Weapon.efireType.SHOTGUN:
            case Weapon.efireType.SINGLE:
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + weapons[(int)currSelectedWeapon].fireRate;
                    pSystem.Emit(1);
                }
                break;
            case Weapon.efireType.LASER:

                break;
        }

    }


    void InitWeapons()
    {
        if (weapons.Length > 0)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weaponObjs[i] = Instantiate(weapons[i].weaponPrefab, weaponSpawnPosition.position, transform.rotation, this.transform);

                if (!weapons[i].isDefaultWeapon)
                {
                    weaponObjs[i].SetActive(false);
                }
            }
        }
        GetBulletParticle();
    }

    public void GetBulletParticle()
    {
        pSystem = weaponObjs[(int)currSelectedWeapon].transform.Find("Bullet_PS").GetComponent<ParticleSystem>();
        weaponObjs[(int)currSelectedWeapon].transform.Find("Bullet_PS").GetComponent<ParticleCollision>().damage = weapons[(int)currSelectedWeapon].damage;
    }



    public void SwitchWeapons(eWeapons nextWeapon)
    {
        int oldWeapon = (int)currSelectedWeapon;
        currSelectedWeapon = nextWeapon;
        weaponObjs[oldWeapon].SetActive(false);
        weaponObjs[(int)nextWeapon].SetActive(true);
        GetBulletParticle();
    }


#if DEBUG
    private void OnGUI()
    {
        if (isSoldier)
        {
            return;
        }

        if (GUI.Button(new Rect(10, 10, 100, 50), "Peashooter"))
        {
            SwitchWeapons(eWeapons.DEFAULT);
        }
        if (GUI.Button(new Rect(110, 10, 100, 50), "DakkaGun"))
        {
            SwitchWeapons(eWeapons.DAKKAGUN);
        }
        if (GUI.Button(new Rect(210, 10, 100, 50), "Impallinator"))
        {
            SwitchWeapons(eWeapons.IMPALLINATOR);
        }
        if (GUI.Button(new Rect(310, 10, 100, 50), "Atomizer"))
        {
            SwitchWeapons(eWeapons.ATOMIZER);
        }
        if (GUI.Button(new Rect(410, 10, 100, 50), "Rektifier"))
        {
            SwitchWeapons(eWeapons.REKTIFIER);
        }
    }
#endif
}
