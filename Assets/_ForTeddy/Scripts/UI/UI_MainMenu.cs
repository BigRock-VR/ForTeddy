using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject settings;
    public GameObject highScore;

    
    [Header("Button")]
    public Button audioBtn;

    [HideInInspector]
    public Button newGame_Btn;
    private Button back_Btn;
    private Button no_Btn;
    private Button backScore_Btn;

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
        highScore.SetActive(false);

        newGame_Btn = mainMenu.GetComponentInChildren<Button>();
        back_Btn = settings.GetComponentInChildren<Button>();
        backScore_Btn = highScore.GetComponentInChildren<Button>();

        newGame_Btn.Select();
    }
    

    //Avvia la partita della scena in VR 
    public void NewGame()
    {
        mainMenu.SetActive(false);
        GameManager.Instance.waveManager.GetComponent<WaveManager>().StartGame();
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
            //BackButton
            case 0:
                mainMenu.SetActive(true);
                settings.SetActive(false);
                highScore.SetActive(false);

                newGame_Btn.Select();
                break;

            //Entry Settings
            case 1:
                settings.SetActive(true);

                back_Btn.Select();
                break;
            //LeaderBoard Button
            case 3:
                highScore.SetActive(true);
                backScore_Btn.Select();
                break;

            default:
                break;
        }
    }
}
