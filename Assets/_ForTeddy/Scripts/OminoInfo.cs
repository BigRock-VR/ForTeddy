using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OminoInfo : MonoBehaviour
{
    public Weapons baseWeapon;
    public Weapons altWeapon;
    public Characters chr;


    public int health = 6;
    public bool armor = false;

    public string chrName;
    public Sprite chrImg;




    private void Start()
    {
        chrName = chr.chrName;
        chrImg = chr.ChrImg;

        baseWeapon = chr.baseWeapon;
    }
}
