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


    // Start is called before the first frame update
    void Start()
    {
        SettingValue();
    }

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


}
