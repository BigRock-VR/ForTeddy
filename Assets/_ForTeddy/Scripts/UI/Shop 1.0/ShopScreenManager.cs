using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopScreenManager : MonoBehaviour
{
    [Header("Hearth Sprites")]
    public Sprite fullHearth;
    public Sprite halfHearth;
    public Sprite emptyHearth;

    [Header("UI Hearth Images")]
    public Image hearth1;
    public Image hearth2;
    public Image hearth3;

    [Header("Player Instantiate")]
    public GameObject player;

    [Header("References from Hierarchy")]
    public Text altWeapon_Txt;
    public Image altWeapon_Img;

    public Text chrName_Txt;
    public Image chr_Img;

    public Text ammo_Txt;
    public Image ammo_Img;

    public GameObject armor;

    [HideInInspector]
    public GameObject actualChr;
    [HideInInspector]
    public OminoInfo info;
    [HideInInspector]
    public Weapons actualWeapon;

    //Reference to Weapon Display Script
    //which has all the information about weapon and PowerUps buttons
    private WeaponDisplay _wD;

    //Bool to know if the actual weapon is the base weapon.
    private bool baseW;

    private void Start()
    {
        //Get the reference from otherScript
        _wD = GetComponent<WeaponDisplay>();

        //Call the funcion to update the screen
        UpdateScreen(player);
    }


    //Update the info in the screen
    public void UpdateScreen(GameObject chr)
    {
        //set the actual selected Character and take the reference to his info
        actualChr = chr;
        info = chr.GetComponent<OminoInfo>();


        WeaponSetting();
        UpdateInfo();

        HealthCheck();
    }

    //setting if the actual weapon is the base weapon or another
    //disabling the actual weapon button if playerammo is full 
    private void WeaponSetting()
    {
        if (info.altWeapon == null)
        {
            actualWeapon = info.baseWeapon;
            _wD.ButtonDisable(actualWeapon);
            baseW = true;
        }
        else
        {
            actualWeapon = info.altWeapon;
            _wD.ButtonDisable(actualWeapon);
            baseW = false;
        }
    }

    //Update the info like 
    //weapon name and image
    //character name and image
    private void UpdateInfo()
    {
        altWeapon_Txt.text = actualWeapon.weaponName;
        altWeapon_Img.sprite = actualWeapon.weaponImage;

        chrName_Txt.text = info.chrName;
        chr_Img.sprite = info.chrImg;

        CheckAmmo();
        CheckArmor();
    }

    //check if the character is teddy or another and set the ammo image and value
    private void CheckAmmo()
    {
        if (baseW || info.chrName != "Teddy")
        {
            ammo_Txt.text = null;
            ammo_Img.gameObject.SetActive(false);
        }

        else
        {
            ammo_Img.gameObject.SetActive(true);
            ammo_Txt.text = ArcadeManager.gm.ammo.ToString();

            //set the cost of the actual weapon if the ammo isn't full

            if (ArcadeManager.gm.ammo < actualWeapon.ammo)
            {
                float sconto = (100 * ArcadeManager.gm.ammo) / actualWeapon.ammo;
                int costoAttuale = (int)(actualWeapon.cost - (actualWeapon.cost * (sconto / 100)));

                _wD.SettingCost(costoAttuale, actualWeapon);
            }
        }
    }

    //checked if the character has armor and set the button to active or not
    private void CheckArmor()
    {
        if (info.armor)
        {
            armor.SetActive(true);
            _wD.powerUpBtn[0].interactable = false;
        }
        else
        {
            armor.SetActive(false);
            _wD.powerUpBtn[0].interactable = true;
        }
    }

    //check character life, set the sprite of the hearths
    //if life is full disable the button
    private void HealthCheck()
    {

        switch (info.health)
        {
            case 0:
                hearth1.sprite = emptyHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 1:
                hearth1.sprite = halfHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 2:
                hearth1.sprite = fullHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 3:
                hearth1.sprite = fullHearth;
                hearth2.sprite = halfHearth;
                hearth3.sprite = emptyHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 4:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = emptyHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 5:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = halfHearth;

                _wD.powerUpBtn[1].interactable = true;

                break;
            case 6:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = fullHearth;

                _wD.powerUpBtn[1].interactable = false;

                break;
            default:
                break;
        }
    }
}
