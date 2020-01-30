using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Displayer_2 : MonoBehaviour
{

    public float maxPriceMultiplier = 1.5f;
    public float actualPriceMultiplier = 1.5f;

    public Button firstSelected;

    [Header("ScriptableObject")]
    public Weapons dakkaGun;

    [Header("Shop Weapon Field")]
    public TMP_Text weaponCost_Txt;
    public TMP_Text ammo_Txt;
    public TMP_Text rateOfFire_Txt;
    public TMP_Text weaponName_Txt;
    public Image gun_Img;
    public Image powerUp_Img;
    public Button buyWeapon_Btn;
    public TMP_Text buyButton_Txt;

    [Header("Character Weapon Field")]
    public TMP_Text scoreValue;
    public TMP_Text charAmmoValue;
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

    public int powerUpPrice;
    public int weaponShopPrice;
    public Weapons actualCharWeapon;
    public Weapons actualShopWeapon;
    public OminoInfo info;
    public GameObject player;
    private bool baseW;

    private void Start()
    {
        info = player.GetComponent<OminoInfo>();
        actualShopWeapon = dakkaGun;

        UpdateScreen(info);
    }
    public void UpdateScreen(OminoInfo newInfo)
    {
        info = newInfo;
        scoreValue.text = ArcadeManager.gm.score.ToString();

        CheckWeapon();
        WeaponShopSetting(actualShopWeapon);
        WeaponCharScreen();
    }

    private void CheckWeapon()
    {
        if (info.altWeapon == null)
        {
            actualCharWeapon = info.baseWeapon;
            baseW = true;
        }
        else
        {
            actualCharWeapon = info.altWeapon;
            baseW = false;
        }
    }

    private void WeaponCharScreen()
    {
        actualWeaponName_Txt.text = actualCharWeapon.weaponName;
        actualWeapon_Img.sprite = actualCharWeapon.weaponImage;

        CheckAmmo();
    }

    private void CheckAmmo()
    {
        if (baseW || info.chrName != "Teddy")
        {
            ammo_Img.gameObject.SetActive(false);

        }
        else
        {
            print("Nell'else, non arma base");
            ammo_Img.gameObject.SetActive(true);
            charAmmoValue.text = ArcadeManager.gm.ammo.ToString();

        }
    }
    public void WeaponShopSetting(Weapons weapon)
    {
        buyWeapon_Btn.interactable = true;

        actualPriceMultiplier = 1;
        actualShopWeapon = weapon;

        weaponCost_Txt.text = weapon.cost.ToString();
        ammo_Txt.text = weapon.ammo.ToString();
        rateOfFire_Txt.text = weapon.rateOfFire.ToString();
        weaponName_Txt.text = weapon.weaponName;
        gun_Img.sprite = weapon.weaponImage;

        buyButton_Txt.text = "Buy for : " + actualShopWeapon.cost;
        if (ArcadeManager.gm.ammo < actualCharWeapon.ammo)
        {
            float sconto = (100 * ArcadeManager.gm.ammo) / actualCharWeapon.ammo;
            int costoAttuale = (int)(actualCharWeapon.cost - (actualCharWeapon.cost * (sconto / 100)));
            weaponShopPrice = costoAttuale;
            buyButton_Txt.text = "Refill Ammo : " + weaponShopPrice;
        }
        else if (ArcadeManager.gm.ammo == actualCharWeapon.ammo)
        {
            print("Full AMmo");
            buyButton_Txt.text = "Buy for : " + actualShopWeapon.cost;
            if (actualShopWeapon == actualCharWeapon)
            {
                buyWeapon_Btn.interactable = false;
            }
        }

    }


    public void HealthScreenSetting(OminoInfo info)
    {
        actualPriceMultiplier = maxPriceMultiplier;

        scoreValue.text = ArcadeManager.gm.score.ToString();

        powerUp_Img.sprite = actualItem_Img.sprite = hearth;
        fill_Sld.color = Color.red;

        float healthValue = (float)(info.health * 100) / ArcadeManager.gm.maxHealth;

        actualItemValue_Sld.value = (int)healthValue;
        actualItemValue_Txt.text = Mathf.Floor(healthValue) + "%";

        powerUp_Txt.text = "Health";

        powerUpCost_Txt.text = CostCalculator(info.health, ArcadeManager.gm.maxHealth, ArcadeManager.gm.maxHPrice);
    }

    public void ArmorScreenSetting(OminoInfo info)
    {
        actualPriceMultiplier = maxPriceMultiplier;
        scoreValue.text = ArcadeManager.gm.score.ToString();

        powerUp_Img.sprite = actualItem_Img.sprite = armor;
        fill_Sld.color = Color.blue;

        float armorValue = (float)(info.armor_int * 100) / ArcadeManager.gm.maxArmor;

        actualItemValue_Sld.value = (int)armorValue;
        actualItemValue_Txt.text = Mathf.Floor(armorValue) + "%";

        powerUp_Txt.text = "Armor";

        powerUpCost_Txt.text = CostCalculator(info.armor_int, ArcadeManager.gm.maxArmor, ArcadeManager.gm.maxAPrice);
    }

    private string CostCalculator(int _actualValue, int _maxValue, int _maxPrice)
    {
        float perc = (float)((_maxValue - _actualValue) * 100) / _maxValue;
        float actualCost = (int)(((perc / 100) * actualPriceMultiplier) * _maxPrice);

        powerUpPrice = (int)actualCost;

        return actualCost.ToString();
    }


}
