using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    public Characters teddy;

    [Header("Hearth Sprites")]
    public Sprite fullHearth;
    public Sprite halfHearth;
    public Sprite emptyHearth;

    [Header("UI Hearth Images")]
    public Image hearth1;
    public Image hearth2;
    public Image hearth3;

    private Weapons altWeapon;

    public Text altWeapon_Txt;
    public Image altWeapon_Img;
    public Text chrName_Txt;
    public Image chr_Img;
    public GameObject armor;
    public Text ammo_Txt;
    public Image ammo_Img;

    private void Start()
    {
        ChangeScreen(teddy);
    }
    public void ChangeScreen(Characters chr)
    {
        altWeapon = chr.altWeapon;

        PlayerCheck(chr);
        WeaponCheck(chr);
        HealthSetting(chr);
        ArmorCheck(chr);

        chrName_Txt.text = chr.chrName;
        chr_Img.sprite = chr.ChrImg;
    }

    private void PlayerCheck(Characters chr)
    {
        if(chr.chrName == "Teddy")
        {
            ammo_Txt.text = GameManager.gm.ammo.ToString();
            ammo_Img.enabled = true;
        }
        else
        {
            ammo_Txt.text = null;
            ammo_Img.enabled = false;
        }
    }
    
    private void WeaponCheck(Characters chr)
    {
        if (altWeapon != null)
        {
            altWeapon_Img.sprite = altWeapon.weaponImage;
            altWeapon_Txt.text = altWeapon.weaponName;
        }
        else
        {
            altWeapon = chr.baseWeapon;
            altWeapon_Img.sprite = altWeapon.weaponImage;
            altWeapon_Txt.text = altWeapon.weaponName;
        }
    }

    private void HealthSetting(Characters chr)
    {
        switch (chr.health)
        {
            case 0:
                hearth1.sprite = emptyHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 1:
                hearth1.sprite = halfHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 2:
                hearth1.sprite = fullHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 3:
                hearth1.sprite = fullHearth;
                hearth2.sprite = halfHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 4:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 5:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = halfHearth;
                break;
            case 6:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = fullHearth;
                break;
            default:
                break;
        }
    }

    private void ArmorCheck(Characters chr)
    {
        if (chr.armor)
        {
            armor.SetActive(true);
        }
        else
        {
            armor.SetActive(false);
        }
    }
}
