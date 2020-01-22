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
    public GameObject confirmQuit;
    public GameObject dissing;

    
    [Header("Button")]
    public Button audioBtn;

    private Button newGame_Btn;
    private Button back_Btn;
    private Button no_Btn;


    [Header("Sprites")]
    public Sprite audioOn; 
    public Sprite audioOff;


    bool audioEnabled = true;

    private void Start()
    {
        StartSettings();
    }

    void StartSettings()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        dissing.SetActive(false);
        confirmQuit.SetActive(false);

        newGame_Btn = mainMenu.GetComponentInChildren<Button>();
        back_Btn = settings.GetComponentInChildren<Button>();
        no_Btn = confirmQuit.GetComponentInChildren<Button>();

        newGame_Btn.Select();
    }

    //Avvia la partita della scena in VR 
    public void NewGame()
    {
        SceneManager.LoadScene(1); 
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
    public void ChangeMenu(int i)
    {
        switch (i)
        {
            case 0:
                mainMenu.SetActive(true);
                settings.SetActive(false);
                confirmQuit.SetActive(false);

                newGame_Btn.Select();
                break;
            case 1:
                mainMenu.SetActive(false);
                settings.SetActive(true);

                back_Btn.Select();
                break;
            case 2:
                confirmQuit.SetActive(true);

                no_Btn.Select();
                break;
        }
    }

    //Esci dal gioco 
    public void QuitGame()
    {
        Application.Quit();
    }
}
