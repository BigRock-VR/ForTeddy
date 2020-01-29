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
    private ShopManager_2 _SM;
    private GameObject[] omini = new GameObject[4];
    private int position;

    private OminoInfo info;
    public GameObject player;

    //in Awake method we setting the default values of all buttons
    void Awake()
    {
        _D = GetComponent<Displayer_2>();
        //_SM = GetComponent<ShopManager_2>();

        info = player.GetComponent<OminoInfo>();
    }

    public void ChooseMenu(int i)
    {
        switch (i)
        {
            case 0:
                weaponsMenu.SetActive(true);
                powerUpMenu.SetActive(false);
                _D.WeaponScreenSetting(info);
                break;
            case 1:
                weaponsMenu.SetActive(false);
                powerUpMenu.SetActive(true);
                _D.HealthScreenSetting(info);
                break;
            case 2:
                weaponsMenu.SetActive(false);
                powerUpMenu.SetActive(true);
                _D.ArmorScreenSetting(info);
                break;
            default:
                break;

        }
    }
}
