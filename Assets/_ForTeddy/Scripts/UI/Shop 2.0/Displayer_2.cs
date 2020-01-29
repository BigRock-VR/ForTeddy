using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Displayer_2 : MonoBehaviour
{
    [Header("ScriptableObject")]
    public Weapons weapon;

    [Header("TextField")]
    public TMP_Text weaponCost_Txt;
    public TMP_Text ammo_Txt;
    public TMP_Text rateOfFire_Txt;
    public TMP_Text weaponName_Txt;

    [Header("ImageField")]
    public Image gun_Img;
    public Image powerUp_Img;

    [Header("Character Weapon Field")]
    public TMP_Text scoreValue;
    public TMP_Text actualAmmoValue;
    public TMP_Text actualWeaponName_Txt;
    public Image actualWeapon_Img;
    public Image ammo_Img;

    [Header("Character Item Field")]
    public Slider actualItemValue_Sld;
    public Image fill_Sld;
    public TMP_Text actualItemValue_Txt;
    public TMP_Text actualItemName_Txt;
    public Image actualItem_Img;

    [Header("Buy PowerUps")]
    public TMP_Text powerUp_Txt;
    public TMP_Text powerUpCost_Txt;

    [Header("Sprites")]
    public Sprite hearth;
    public Sprite armor;

    public void WeaponScreenSetting(OminoInfo info)
    {



    }

    public void HealthScreenSetting(OminoInfo info)
    {

        powerUp_Img.sprite = actualItem_Img.sprite = hearth;
        fill_Sld.color = Color.red;

        powerUp_Txt.text = "Health";

        powerUpCost_Txt.text = CostCalculator(info.health, ArcadeManager.gm.maxHealth);
    }

    public void ArmorScreenSetting(OminoInfo info)
    {

        powerUp_Img.sprite = actualItem_Img.sprite = armor;
        fill_Sld.color = Color.blue;

        powerUp_Txt.text = "Armor";

        powerUpCost_Txt.text = CostCalculator(info.armor_int, ArcadeManager.gm.maxArmor);
    }

    private string CostCalculator(int actualValue, int MaxValue)
    {
        print("actual Value" + actualValue);
        print("Max Value" + MaxValue);


        float multiplier = 1.5f;
        float healthPerc = ((actualValue * 100) / MaxValue);
        float scont = MaxValue - ((healthPerc / 100) * MaxValue);
        float actualCost = scont * (multiplier * (healthPerc / 100));

        print("Health percent " + healthPerc);
        print("sCost " + scont);
        print("Actual Cost " + actualCost);

        return actualCost.ToString();

    }
}
