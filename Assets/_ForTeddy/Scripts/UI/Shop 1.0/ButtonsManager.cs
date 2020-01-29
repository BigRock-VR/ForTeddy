using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    /*Here we manage all the buttons in sjop panel*/

    [Header("Prefab Omini")]
    public GameObject[] ominoPrefabs;
    public Transform[] ominiPos = new Transform[4];

    [Header("Menus")]
    public GameObject confirmMenu;
    public GameObject notEnoughtMenu;

    private WeaponDisplay _wD;
    private ShopScreenManager _SSM;
    private GameObject[] omini = new GameObject[4];
    private int position;

    //take reference to other Scripts
    private void Start()
    {
        _wD = GetComponent<WeaponDisplay>();
        _SSM = GetComponent<ShopScreenManager>();
    }

    public void ArmorUp()
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= ArcadeManager.gm.armorCost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
            notEnoughtMenu.SetActive(true);
        }
        else
        {
            _SSM.info.armor = true;
            _wD.powerUpBtn[0].interactable = false;

            _SSM.UpdateScreen(_SSM.actualChr);
        }
    }

    public void HealsUp()
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= ArcadeManager.gm.healthCost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
            notEnoughtMenu.SetActive(true);
        }
        else
        {
            _SSM.info.health += 2;
            _SSM.info.health = Mathf.Clamp(_SSM.info.health, 0, 6);
            _SSM.UpdateScreen(_SSM.actualChr);
        }
    }

    public void BuyWeapon(Weapons weapon)
    {

        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= weapon.cost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
            notEnoughtMenu.SetActive(true);
        }
        else
        {
            CheckPlayer(weapon);
            _SSM.info.altWeapon = weapon;
            _SSM.UpdateScreen(_SSM.actualChr);
        }
    }

    private void CheckPlayer(Weapons weapon)
    {
        if (_SSM.actualChr == _SSM.player)
        {
            ArcadeManager.gm.ammo = weapon.ammo;
        }
    }
    public void BuySoldiers(int i)
    {
        position = i;
        if (omini[i] == null)
        {
            confirmMenu.SetActive(true);
        }
        else
        {
            _SSM.UpdateScreen(omini[i]);
        }
    }

    public void Accept()
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= ArcadeManager.gm.ominoCost;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
            notEnoughtMenu.SetActive(true);
        }
        else
        {
            omini[position] = Instantiate(ominoPrefabs[position], ominiPos[position]);
            _SSM.UpdateScreen(omini[position]);

            confirmMenu.SetActive(false);
        }
    }

    public void Refuse()
    {
        confirmMenu.SetActive(false);
        notEnoughtMenu.SetActive(false);
    }
}
