using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public string name;
    public float fireRate;
    public float bulletSpeed = 1000;
    public int damage;
    public uint maxAmmoCount;
    public bool isDefaultWeapon;

    public enum efireType { SINGLE, SHOTGUN, LASER};
    public efireType fireType;
    public GameObject weaponPrefab;
}
