using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OminoInfo : MonoBehaviour
{

    //All the info about an instantiate Character

    public Weapons baseWeapon;
    public Weapons altWeapon;
    public Characters chr;

    public int health = 500;
    public int armor_int = 250;
    public bool armor;

    public string chrName;
    public Sprite chrImg;
    public Sprite ammoImg;

    private void Awake()
    {
        chrName = chr.chrName;
        chrImg = chr.ChrImg;

        baseWeapon = chr.baseWeapon;
    }
}
