using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    private CharacterDisplay _cD;
    private WeaponDisplay _wD;
    private int costoAttuale;

    private GameObject[] omini = new GameObject[3];
    [Header("Prefab Omini")]
    public GameObject omino;

    public GameObject confirmMenu;

    private void Start()
    {
        _cD = GetComponent<CharacterDisplay>();
        _wD = GetComponent<WeaponDisplay>();
    }
    public void WeaponBtnDisable()
    {
        if (_cD.fullAmmo)
        {
            print("full ammo togli interactable");
            _cD.actualWeaponBtn.interactable = false;
            _cD.actualWeaponTxt.text = _cD.actualWeapon.cost.ToString();

        }
        else
        {

            print("resetCost");
            float sconto = (100 * ArcadeManager.gm.ammo) / _cD.actualWeapon.ammo;
            costoAttuale = (int)(_cD.actualWeapon.cost - (_cD.actualWeapon.cost * (sconto / 100)));

            _cD.actualWeaponTxt.text = costoAttuale.ToString();

        }
    }

    public void ArmorUp()
    {
        _cD.armor.SetActive(true);
        _wD.powerUpBtn[0].interactable = false;
    }

    public void HealsUp()
    {
        ArcadeManager.gm.health += 2;
        ArcadeManager.gm.health = Mathf.Clamp(ArcadeManager.gm.health, 0, 6);

        _cD.ChangeScreen(_cD.teddy);
    }

    public void BuyWeapon(Weapons weapon)
    {

        CheckScore(weapon);

    }

    void CheckScore(Weapons weapon)
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= weapon.cost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
        }
        else if (_cD.actualWeaponBtn == null)
        {
            _cD.altWeapon = weapon;
            ArcadeManager.gm.ammo = weapon.ammo;
            _cD.altWeapon_Img.sprite = weapon.weaponImage;
            _cD.altWeapon_Txt.text = weapon.weaponName;

            _cD.actualChr.GetComponent<OminoInfo>().altWeapon = weapon;


        }

        else
        {
            _cD.actualWeaponBtn.interactable = true;
            _cD.altWeapon = weapon;
            ArcadeManager.gm.ammo = weapon.ammo;
            _cD.altWeapon_Img.sprite = weapon.weaponImage;
            _cD.altWeapon_Txt.text = weapon.weaponName;

            _cD.actualChr.GetComponent<OminoInfo>().altWeapon = weapon;
        }
    }

    public void BuySoldiers(int i)
    {
        if (omini[i] == null)
        {
            confirmMenu.SetActive(true);
        }
        else
        {
            _cD.ChangeScreen(omini[i]);
        }
    }

    public void Accept(int i)
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= ArcadeManager.gm.ominoCost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
        }
        else
        {
            omini[i] = Instantiate(omino, transform);
            _cD.ChangeScreen(omini[i]);

            confirmMenu.SetActive(false);
        }
    }

    public void Refuse()
    {
        confirmMenu.SetActive(false);
    }
        
}
