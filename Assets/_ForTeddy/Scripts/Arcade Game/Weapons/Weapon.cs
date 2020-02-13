using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public float fireRate;
    public float bulletSpeed = 1000;
    public int damage;
    public int maxAmmoCount;
    public bool isDefaultWeapon;
    public int cost;
    public Sprite weaponImage;

    public enum efireType { SINGLE, SHOTGUN, LASER, EXPLOSION};
    public efireType fireType;
    public GameObject weaponPrefab;
    public GameObject explosionBullet;
}
