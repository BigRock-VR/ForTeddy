using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Weapons")]
public class Weapons : ScriptableObject
{
    public string weaponName;
    public Sprite weaponImage;

    public int cost;
    public float rateOfFire;
    public int ammo;
    public float damage;
}
