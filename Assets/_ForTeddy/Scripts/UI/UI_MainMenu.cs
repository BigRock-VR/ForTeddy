using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject controlsMenu;
    public GameObject rankingMenu;

    [HideInInspector]
    public Button newGame_Btn;
    private Button back_Btn;
    private Button backScore_Btn;

    private void Start()
    {
        StartSettings();
        FMODBGPlayer.SetGamePlayMusic();
    }

    void StartSettings()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
        rankingMenu.SetActive(false);

        newGame_Btn = mainMenu.GetComponentInChildren<Button>();
        back_Btn = controlsMenu.GetComponentInChildren<Button>();
        backScore_Btn = rankingMenu.GetComponentInChildren<Button>();

        newGame_Btn.Select();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NewGame()
    {
        mainMenu.SetActive(false);
        FMODBGPlayer.SetGamePlayMusic();
        GameManager.Instance.waveManager.GetComponent<WaveManager>().StartGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    //Cambio menu tra main e Settings
    public void ChangeMenu(int i)
    {
        switch (i)
        {
            //BackButton
            case 0:
                mainMenu.SetActive(true);
                controlsMenu.SetActive(false);
                rankingMenu.SetActive(false);
                newGame_Btn.Select();
                break;
            //Entry Settings
            case 1:
                controlsMenu.SetActive(true);
                back_Btn.Select();
                break;
            //LeaderBoard Button
            case 2:
                rankingMenu.SetActive(true);
                backScore_Btn.Select();
                break;
            default:
                break;
        }
    }
}
