#define TESTMODE
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    //Scritable Objects
    [SerializeField] public Weapon[] weapons = new Weapon[MAX_WEAPONS];
    private GameObject[] weaponObjs = new GameObject[MAX_WEAPONS];

    public Transform[] weaponSpawnPositions = new Transform[MAX_WEAPONS];
    public enum eWeapons {DEFAULT, DAKKAGUN, IMPALLINATOR, ATOMIZER, REKTIFIER};
    public eWeapons currSelectedWeapon = eWeapons.DEFAULT;


    private Animator anim;
    private const int MAX_WEAPONS = 5;
    private float nextTimeToFire = 0.0f;
    public bool isSoldier;
    public float currAmmo = 0.0f;
    //Particle Bullets
    private ParticleSystem pSystem;
    //Rektifier Logics
    private Transform bulletSpawnPosition;
    private bool isSwitchingWeapon;

    void Start()
    {
        anim = GetComponent<Animator>();
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
        switch (weapons[GetCurrSelectedWeapon()].fireType)
        {
            case Weapon.efireType.SHOTGUN:
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + weapons[GetCurrSelectedWeapon()].fireRate;
                    CheckCurrentAmmo(); // DECREASE AMMO 
                    pSystem.Play();
                    anim.SetTrigger("isShooting");
                }
                break;
            case Weapon.efireType.SINGLE:
                if (Time.time >= nextTimeToFire)
                {
                    if (!weapons[GetCurrSelectedWeapon()].isDefaultWeapon)
                    {
                        CheckCurrentAmmo(); // DECREASE AMMO IF IS NOT PEASHOTER
                    }

                    nextTimeToFire = Time.time + weapons[GetCurrSelectedWeapon()].fireRate;
                    pSystem.Play();
                }
                break;
            // ATOMIZER WEAPON
            case Weapon.efireType.LASER:
                CheckLaserAmmo();
                if (currAmmo > 0)
                {
                    currAmmo -= Time.deltaTime;
                    RaycastHit[] hits;
                    // Draw a Ray with a range that trigger every object in the radius
                    hits = Physics.RaycastAll(weaponSpawnPositions[GetCurrSelectedWeapon()].position, transform.forward, 10.0f);
                    Debug.DrawRay(weaponSpawnPositions[GetCurrSelectedWeapon()].position, transform.forward * 10.0f, Color.yellow);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.CompareTag("Enemy"))
                        {
                            // Return the hit poitin in object space
                            Vector4 hitPointLocal = hits[i].transform.InverseTransformPoint(hits[i].point);
                            hits[i].transform.GetComponent<Enemy>().TakeDamage(weapons[GetCurrSelectedWeapon()].damage, hitPointLocal);
                        }
                    }
                }
                break;
            // REKTIFIER WEAPON
            case Weapon.efireType.EXPLOSION:
                if (Time.time >= nextTimeToFire)
                {
                    CheckCurrentAmmo();
                    pSystem.Play();
                    nextTimeToFire = Time.time + weapons[GetCurrSelectedWeapon()].fireRate;
                    var _bullet = Instantiate(weapons[GetCurrSelectedWeapon()].explosionBullet, bulletSpawnPosition.position, Quaternion.identity, null);
                    _bullet.GetComponent<RektifierExplosion>().direction = transform.forward;
                }
                break;
        }

    }


    private void CheckLaserAmmo()
    {
        if(currAmmo > 0)
        {
            return;
        }
        if (currAmmo < 0 && !isSwitchingWeapon)
        {
            isSwitchingWeapon = true;
            Invoke("SwitchWeapons", 0.5f);
        }
    }
    private void CheckCurrentAmmo()
    {
        float tmp = --currAmmo;
        if (tmp <= 0.0f && !isSwitchingWeapon)
        {
            isSwitchingWeapon = true;
            currAmmo = 0.0f;
            Invoke("SwitchWeapons", 0.5f);
        }
        else
        {
            currAmmo = tmp;
        }

    }


    void InitWeapons()
    {
        if (weapons.Length > 0)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weaponObjs[i] = Instantiate(weapons[i].weaponPrefab, weaponSpawnPositions[i].transform);

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
        pSystem = weaponObjs[GetCurrSelectedWeapon()].transform.Find("Bullet_PS").GetComponent<ParticleSystem>();
        bulletSpawnPosition = weaponObjs[GetCurrSelectedWeapon()].transform.Find("Bullet_PS").transform;
        // Set Up the weapon damage to the single particle
        weaponObjs[GetCurrSelectedWeapon()].transform.Find("Bullet_PS").GetComponent<ParticleCollision>().damage = weapons[GetCurrSelectedWeapon()].damage;
    }

    public int GetCurrSelectedWeapon()
    {
        return (int)currSelectedWeapon;
    }


    public void SwitchWeapons()
    {
        int oldWeapon = GetCurrSelectedWeapon();
        SwitchAnimationLayer(eWeapons.DEFAULT);
        nextTimeToFire = 0;
        currAmmo = weapons[(int)eWeapons.DEFAULT].maxAmmoCount;
        currSelectedWeapon = eWeapons.DEFAULT;
        weaponObjs[oldWeapon].SetActive(false);
        weaponObjs[(int)eWeapons.DEFAULT].SetActive(true);
        GetBulletParticle();
        isSwitchingWeapon = false;
    }

    public void SwitchWeapons(eWeapons nextWeapon)
    {
        int oldWeapon = GetCurrSelectedWeapon();
        SwitchAnimationLayer(nextWeapon);
        nextTimeToFire = 0;
        currAmmo = weapons[(int)nextWeapon].maxAmmoCount;
        currSelectedWeapon = nextWeapon;
        weaponObjs[oldWeapon].SetActive(false);
        weaponObjs[(int)nextWeapon].SetActive(true);
        GetBulletParticle();
        isSwitchingWeapon = false;
    }

    private void SwitchAnimationLayer(eWeapons nextWeapon)
    {
        int layer = 0;

        switch (nextWeapon)
        {

            case eWeapons.DEFAULT:
                layer = 1;
                break;
            case eWeapons.DAKKAGUN:
                layer = 2;
                break;
            case eWeapons.IMPALLINATOR:
                layer = 3;
                break;
            case eWeapons.ATOMIZER:
                layer = 4;
                break;
            case eWeapons.REKTIFIER:
                layer = 5;
                break;
            default:
                break;
        }
        anim.SetLayerWeight(layer, 1);
        anim.SetLayerWeight(GetCurrSelectedWeapon() + 1, 0);
    }


#if TESTMODE
    private void OnGUI()
    {
        if (isSoldier)
        {
            return;
        }

        if (GUI.Button(new Rect(10, 210, 70, 25), "Peashooter"))
        {
            SwitchWeapons(eWeapons.DEFAULT);
        }
        if (GUI.Button(new Rect(10, 230, 70, 25), "DakkaGun"))
        {
            SwitchWeapons(eWeapons.DAKKAGUN);
        }
        if (GUI.Button(new Rect(10, 250, 70, 25), "Impallinator"))
        {
            SwitchWeapons(eWeapons.IMPALLINATOR);
        }
        if (GUI.Button(new Rect(10, 280, 70, 25), "Atomizer"))
        {
            SwitchWeapons(eWeapons.ATOMIZER);
        }
        if (GUI.Button(new Rect(10, 300, 70, 25), "Rektifier"))
        {
            SwitchWeapons(eWeapons.REKTIFIER);
        }
    }
#endif
}
