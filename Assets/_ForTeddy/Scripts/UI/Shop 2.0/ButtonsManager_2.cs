using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonsManager_2 : MonoBehaviour
{
    /*Here we manage all the buttons in sjop panel*/

    [Header("Prefab Omini")]
    public GameObject[] ominoPrefabs;
    public Transform[] ominiPos = new Transform[4];

    [Header("Menus")]
    public GameObject confirmMenu;
    public GameObject notEnoughtMenu;

    public GameObject powerUpMenu;
    public GameObject weaponsMenu;

    private Displayer_2 _D;
    private GameObject[] omini = new GameObject[4];
    public Button[] soldier_Btn = new Button[4];
    private int position;

    private int openMenu;
    private int justPressButton;

    //in Awake method we setting the default values of all buttons
    void Awake()
    {
        _D = GetComponent<Displayer_2>();
    }

    public void ChooseLeftMenu(int i)
    {
        switch (i)
        {
            case 0:
                weaponsMenu.SetActive(true);
                powerUpMenu.SetActive(false);
                _D.UpdateScreen(_D.info);

                openMenu = 0;
                break;
            case 1:
                weaponsMenu.SetActive(false);
                powerUpMenu.SetActive(true);
                _D.HealthScreenSetting(_D.info);

                openMenu = 1;
                break;
            case 2:
                weaponsMenu.SetActive(false);
                powerUpMenu.SetActive(true);
                _D.ArmorScreenSetting(_D.info);

                openMenu = 2;
                break;
            default:
                break;

        }
    }

    public void BuyObject()
    {
        switch (openMenu)
        {
            case 0:
                //Compro l'arma
                CheckScore(_D.weaponShopPrice);
                break;
            case 1:
            case 2:
                //compro vita/Armor
                CheckScore(_D.powerUpPrice);
                break;
            default:
                break;
        }
    }

    public void BuySoldiers(int i)
    {
        position = i;
        if (omini[i] == null)
        {
            confirmMenu.SetActive(true);
            confirmMenu.GetComponentInChildren<Button>().Select();
        }
        else
        {
            _D.UpdateScreen(omini[i].GetComponent<OminoInfo>());
        }
    }

    private void CheckScore(int price)
    {
        int temp = ArcadeManager.gm.score;
        ArcadeManager.gm.score -= price;
        if (ArcadeManager.gm.score < 0)
        {
            ArcadeManager.gm.score = temp;
            notEnoughtMenu.SetActive(true);
            notEnoughtMenu.GetComponentInChildren<Button>().Select();
        }
        else if (openMenu == 0)
        {
            _D.info.altWeapon = _D.actualShopWeapon;
            ArcadeManager.gm.ammo = _D.actualShopWeapon.ammo;

            _D.firstSelected.Select();
            _D.UpdateScreen(_D.info);
        }
        else if (openMenu == 1)
        {
            _D.info.health = ArcadeManager.gm.maxHealth;

            _D.HealthScreenSetting(_D.info);
        }
        else if (openMenu == 2)
        {
            _D.info.armor_int = ArcadeManager.gm.maxArmor;

            _D.ArmorScreenSetting(_D.info);
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
            _D.UpdateScreen(omini[position].GetComponent<OminoInfo>());
            ColorBlock colors = soldier_Btn[position].colors;
            colors.normalColor = Color.blue;

            soldier_Btn[position].colors = colors;
            confirmMenu.SetActive(false);

            _D.firstSelected.Select();

        }
    }

    public void Refuse()
    {
        confirmMenu.SetActive(false);
        notEnoughtMenu.SetActive(false);

        _D.firstSelected.Select();
    }

}
