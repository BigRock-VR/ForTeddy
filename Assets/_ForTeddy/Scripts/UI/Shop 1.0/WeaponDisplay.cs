using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    [Header("ScriptableObject")]
    public Weapons[] weapon = new Weapons[5];
    public PowerUp[] powerUp = new PowerUp[2];

    [Header("TextField")]
    public Text[] weaponCost_Txt = new Text[4];
    public Text[] ammo_Txt = new Text[4];
    public Text[] rateOfFire_Txt = new Text[4];
    public Text[] weaponName_Txt = new Text[4];

    public Text[] powerUpCost = new Text[2];


    [Header("ImageField")]
    public Image[] gun_Img = new Image[4];
    public Image[] powerUp_Img = new Image[2];


    [Header("Buttons")]
    public Button[] weaponsBtn = new Button[4];
    public Button[] powerUpBtn = new Button[2];

    //in Awake method we setting the default values of all buttons
    void Awake()
    {
        SettingValue();
    }

    //Setting the default values
    public void SettingValue()
    {
        for (int i = 0; i < 4; i++)
        {
            weaponsBtn[i].interactable = true;

            weaponCost_Txt[i].text = weapon[i].cost.ToString();
            ammo_Txt[i].text = weapon[i].ammo.ToString();
            rateOfFire_Txt[i].text = weapon[i].rateOfFire.ToString();
            weaponName_Txt[i].text = weapon[i].name.ToString();

            gun_Img[i].sprite = weapon[i].weaponImage;
        }
        for (int i = 0; i < 2; i++)
        {
            powerUpBtn[i].interactable = true;
            powerUpCost[i].text = powerUp[i].cost.ToString();

            powerUp_Img[i].sprite = powerUp[i].powerUpImage;
        }

    }

    //disable the button of the altready bought weapon
    public void ButtonDisable(Weapons actualWeapon)
    {
        for (int i = 0; i < weapon.Length - 1; i++)
        {
            if (actualWeapon == weapon[i])
            {
                weaponsBtn[i].interactable = false;
                weaponCost_Txt[i].text = weapon[i].cost.ToString();
            }
            else
            {
                weaponsBtn[i].interactable = true;
                weaponCost_Txt[i].text = weapon[i].cost.ToString();
            }
        }
    }

    //reset the costs of the weapon if we set a character != teddy
    public void SettingCost(int costoAttuale, Weapons actualWeapon)
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            if (actualWeapon == weapon[i])
            {
                weaponCost_Txt[i].text = costoAttuale.ToString();
                weaponsBtn[i].interactable = true;
            }
        }
    }
}
