using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager_2 : MonoBehaviour
{
    [Header("Player Instantiate")]
    public GameObject player;

    [Header("References from Hierarchy")]
    public Text altWeapon_Txt;
    public Image altWeapon_Img;

    public Text chrName_Txt;
    public Image chr_Img;

    public Text ammo_Txt;
    public Image ammo_Img;

    [HideInInspector]
    public GameObject actualChr;
    [HideInInspector]
    public OminoInfo info;
    [HideInInspector]
    public Weapons actualWeapon;

    //Reference to Displayer_2 Script
    //which has all the information about weapon and PowerUps buttons
    private Displayer_2 _D;

    //Bool to know if the actual weapon is the base weapon.
    private bool baseW;

    private void Start()
    {
        //Get the reference from otherScript
        _D = GetComponent<Displayer_2>();

        //Call the funcion to update the screen
        UpdateScreen(player);
    }

    public void UpdateScreen(GameObject chr)
    {
        //set the actual selected Character and take the reference to his info
        actualChr = chr;
        info = chr.GetComponent<OminoInfo>();


    }

    
}
