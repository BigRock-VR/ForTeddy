using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    public GameObject teddy;
    [Header("Hearth Sprites")]
    public Sprite fullHearth;
    public Sprite halfHearth;
    public Sprite emptyHearth;

    [Header("UI Hearth Images")]
    public Image hearth1;
    public Image hearth2;
    public Image hearth3;

    [Header("References from Hierarchy")]
    public Text altWeapon_Txt;
    public Image altWeapon_Img;

    public Text chrName_Txt;
    public Image chr_Img;

    public Text ammo_Txt;
    public Image ammo_Img;

    public GameObject armor;

    [Header("soldier Selection Buttons")]
    public Button[] soldiers_Btn = new Button[4];

    [HideInInspector]
    public bool fullHealth;
    public bool fullAmmo;
    public bool buyArmor;
    public Weapons altWeapon;
    public GameObject actualChr;

    public Button actualWeaponBtn;
    public Text actualWeaponTxt;
    public Weapons actualWeapon;

    public ButtonsManager _bM;
    public WeaponDisplay _wD;
    private void Start()
    {
        _wD = GetComponent<WeaponDisplay>();
        _bM = GetComponent<ButtonsManager>();
        ChangeScreen(teddy);
    }
    public void ChangeScreen(GameObject chr)
    {
        OminoInfo info = chr.GetComponent<OminoInfo>();
        actualChr = chr;

        Reset();

        PlayerCheck(info);
        HealthSetting(info);
        ArmorCheck(info);
        ShopCheck(info);

        chrName_Txt.text = info.chrName;
        chr_Img.sprite = info.chrImg;
    }
    private void Reset()
    {
        _wD.SettingValue();
    }
    private void PlayerCheck(OminoInfo chr)
    {
        if (chr.chrName == "Teddy" && chr.altWeapon != null)
        {
            print("hahahaahah teddy è teddy");
            ammo_Txt.text = ArcadeManager.gm.ammo.ToString();
            for (int i = 0; i < _wD.weapon.Length; i++)
            {
                if (altWeapon == _wD.weapon[i])
                {
                print("ma non pigòia ò'arma");

                    actualWeapon = _wD.weapon[i];
                    actualWeaponBtn = _wD.weaponsBtn[i];
                    actualWeaponTxt = _wD.weaponCost_Txt[i];

                    if (ArcadeManager.gm.ammo == _wD.weapon[i].ammo)
                    {
                        
                        fullAmmo = true;
                    }
                    else
                    {
                        fullAmmo = false;
                    }
                }
            }
        }
        else
        {
            ammo_Txt.text = null;
            fullAmmo = true;

            actualWeapon = altWeapon;
        }
    }

    private void ShopCheck(OminoInfo chr)
    {
        if (altWeapon != null)
        {
            altWeapon_Img.sprite = altWeapon.weaponImage;
            altWeapon_Txt.text = altWeapon.weaponName;
            if (altWeapon != chr.baseWeapon)
            {
                _bM.WeaponBtnDisable();
            }
        }
        else
        {
            altWeapon = chr.baseWeapon;
            altWeapon_Img.sprite = altWeapon.weaponImage;
            altWeapon_Txt.text = altWeapon.weaponName;

            for (int i = 0; i < _wD.weaponsBtn.Length; i++)
            {
                _wD.weaponsBtn[i].interactable = true;
            }
        }
    }



    private void HealthSetting(OminoInfo chr)
    {
        int h;
        if (chr.name == "Teddy")
        {
            h = ArcadeManager.gm.health;
        }
        else
        {
            h = chr.health;
        }
        switch (h)
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

    private void ArmorCheck(OminoInfo chr)
    {
        if (chr.armor)
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
}
