using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    [Header("ScriptableObject")]
    public Weapons weapon;

    [Header("TextField")]
    public   Text cost_Txt;
    public Text ammo_Txt;
    public Text rateOfFire_Txt;
    public Text weaponName_Txt;

    [Header("ImageField")]
    public Image gun_Img;

    
    // Start is called before the first frame update
    void Start()
    {
        cost_Txt.text = weapon.cost.ToString();
        ammo_Txt.text = weapon.ammo.ToString();
        rateOfFire_Txt.text = weapon.rateOfFire.ToString();
        weaponName_Txt.text = weapon.name.ToString();

        gun_Img.sprite = weapon.weaponImage;
    }

    
}
