//#define TESTMODE
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

    //Particle Bullets
    private ParticleSystem pSystem;

    // Atomizer Laser Logics
    private float laserTimer;

    //Rektifier Logics
    private Transform bulletSpawnPosition;

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
                    pSystem.Play();
                    anim.SetTrigger("isShooting");
                }
                break;
            case Weapon.efireType.SINGLE:
                if (Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + weapons[GetCurrSelectedWeapon()].fireRate;
                    //pSystem.Emit(1);
                    pSystem.Play();
                }
                break;
            // ATOMIZER WEAPON
            case Weapon.efireType.LASER:

                if (laserTimer == 0)
                {
                    laserTimer = weapons[GetCurrSelectedWeapon()].fireRate;
                }

                if (laserTimer > 0)
                {
                    laserTimer -= Time.deltaTime;
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
                    pSystem.Play();
                    nextTimeToFire = Time.time + weapons[GetCurrSelectedWeapon()].fireRate;
                    var _bullet = Instantiate(weapons[GetCurrSelectedWeapon()].explosionBullet, bulletSpawnPosition.position, Quaternion.identity, null);
                    _bullet.GetComponent<RektifierExplosion>().direction = transform.forward;
                }
                break;
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
    public void SwitchWeapons(eWeapons nextWeapon)
    {
        int oldWeapon = GetCurrSelectedWeapon();
        SwitchAnimationLayer(nextWeapon);
        nextTimeToFire = 0;
        laserTimer = 0;
        currSelectedWeapon = nextWeapon;
        weaponObjs[oldWeapon].SetActive(false);
        weaponObjs[(int)nextWeapon].SetActive(true);
        GetBulletParticle();
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
