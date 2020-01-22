using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject settings;

    
    [Header("Button")]
    public Button audioBtn;


    [Header("Sprites")]
    public Sprite audioOn; 
    public Sprite audioOff;


    bool audioEnabled = true;
    bool menuChange = false;

    private void Start()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }
    //Avvia la partita della scena in VR 
    public void NewGame()
    {
        SceneManager.LoadScene("GameVR"); //Controllare Index scena-Nome scena che sia corretto!! 
    }
    
    //Setti i suoni attivi o meno e in base cambia la sprite nel menu
   public void Mute()
    {
        audioEnabled = !audioEnabled;
        if (audioEnabled) // Audio Enable -- set a sprite
        {
            audioBtn.image.sprite = audioOn;
            audioBtn.GetComponentInChildren<Text>().text = "ON";
        }
        else         //Audio disabled -- change Sprite
        {
            audioBtn.image.sprite = audioOff;
            audioBtn.GetComponentInChildren<Text>().text = "OFF";
        }
    }


    //Cambio menu tra main e Settings
    public void ChangeMenu()
    {
        menuChange = !menuChange;
        if (!menuChange)
        {
            mainMenu.SetActive(true);
            settings.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            settings.SetActive(true);
        }
    }

    //Esci dal gioco 
    public void QuitGame()
    {
        Application.Quit();
    }
}
