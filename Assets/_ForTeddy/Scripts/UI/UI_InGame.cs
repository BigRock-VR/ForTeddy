using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [Header("Hearth Sprites")]
    public Sprite fullHearth;
    public Sprite halfHearth;
    public Sprite emptyHearth;

    [Header("UI Hearth Images")]
    public Image hearth1;
    public Image hearth2;
    public Image hearth3;

    [Header("UI Ammo")]
    public Text ammoCount;

    [Header("UI Score")]
    public Text scoreCount;

    [Header("Slider WaveTimer")]
    public Slider waveTime;

    [Header("Wave Panels")]
    public GameObject newWave;
    public GameObject endWave;

    public Text newWave_Txt;
    public Text endWave_Txt;

    [Header("Shop UI")]
    public Text shopHeader;
    public GameObject shop;

    public Text shopCoin;

    [Header("In Game Menu")]
    public GameObject gameUI;
    public GameObject pauseMenu;

    public GameObject confirmMenu;
    public GameObject settingsMenu;
    public GameObject chooseName;

    public GameObject mainMenu;
    public GameObject gameOver;

    private bool pauseGame;

    [Header("First Button Selected")]
    public Button dakkaGun;
    private Button back;
    private Button toMainMenu;
    private Button resume;

    private bool canPause;
    private void Start()
    {
        StartSettings();
    }

    void StartSettings()
    {
        gameUI.SetActive(false);
        newWave.SetActive(false);
        endWave.SetActive(false);
        shop.SetActive(false);
        pauseMenu.SetActive(false);
        confirmMenu.SetActive(false);
        settingsMenu.SetActive(false);
        chooseName.SetActive(false);
        gameOver.SetActive(false);

        back = settingsMenu.GetComponentInChildren<Button>();
        toMainMenu = confirmMenu.GetComponentInChildren<Button>();
        resume = pauseMenu.GetComponentInChildren<Button>();
    }
    void Update()
    {
        //Update Ammo/Score GameManager Based
        ammoCount.text = ArcadeManager.gm.ammo.ToString();
        scoreCount.text = ArcadeManager.gm.score.ToString();
        shopCoin.text = scoreCount.text;

        //Call here, Better if called By GameManager when HealthUpdate
        HealthUI();

        //Update Slider Value
        WaveTimer();

        if (Input.GetButtonDown("Cancel") && canPause)
        {
            PauseMenu();
        }
    }

    //HealthManager Update by GameManager Health Value (int 0-6)
    public void HealthUI()
    {
        switch (ArcadeManager.gm.health)
        {
            case 0:
                hearth1.sprite = emptyHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 1:
                hearth1.sprite = halfHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 2:
                hearth1.sprite = fullHearth;
                hearth2.sprite = emptyHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 3:
                hearth1.sprite = fullHearth;
                hearth2.sprite = halfHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 4:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = emptyHearth;
                break;
            case 5:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = halfHearth;
                break;
            case 6:
                hearth1.sprite = fullHearth;
                hearth2.sprite = fullHearth;
                hearth3.sprite = fullHearth;
                break;
            default:
                break;
        }
    }

    //remap from TimeGame in GameManager for In game slider
    public void WaveTimer()
    {
        //Remap Timer Value for SliderLerp
        float newTimer = ArcadeManager.gm.waveTimeActual.Remap(0, ArcadeManager.gm.waveTime, 0, 1);

        waveTime.value = Mathf.Lerp(10, 0, newTimer);
    }

    public void NewWave()
    {
        newWave_Txt.text = "Wave N " + ArcadeManager.gm.wave + " Starts!";

        endWave.SetActive(false);
        shop.GetComponent<Animator>().SetTrigger("closeLerp");

        StartCoroutine(MenuTimer(1));

        ArcadeManager.gm.waveTimeActual = ArcadeManager.gm.waveTime;

    }
    public void EndWave()
    {
        newWave.SetActive(false);
        endWave.SetActive(true);
        StartCoroutine(MenuTimer(0));
    }

    //On AxisDown ("Cancel") -- Menu Pause
    //Or call by "Resume" Button
    public void PauseMenu()
    {
        if (confirmMenu.activeInHierarchy == true)
        {
            ChangeMenu(2);
        }
        else if (settingsMenu.activeInHierarchy == true)
        {
            ChangeMenu(3);
        }
        else {
            pauseGame = !pauseGame;

            if (pauseGame)
            {
                if (shop.activeInHierarchy)
                {
                    pauseMenu.SetActive(false);
                    dakkaGun.Select();
                }
                else
                {
                    pauseMenu.SetActive(false);
                    ArcadeManager.gm.startTimer = true;
                }
            }
            else
            {
                pauseMenu.SetActive(true);
                resume.Select();
                ArcadeManager.gm.startTimer = false;
            }
        }
    }

    //Functions for QuitMenuManagement
    public void ChangeMenu(int i)
    {
        switch (i)
        {
            //From Quit Button
            case 0:
                confirmMenu.SetActive(true);
                toMainMenu.Select();
                break;
            //From Settings Button
            case 1:
                settingsMenu.SetActive(true);
                back.Select();
                break;
            //From Back Button in confirmMenu
            case 2:
                confirmMenu.SetActive(false);
                settingsMenu.SetActive(false);

                resume.Select();
                break;

            //from GameoverState
            case 3:
                gameUI.SetActive(false);
                gameOver.SetActive(true);
                Invoke("GameOver", 1.5f);
                canPause = false;
                break;

            default:
                print("Unideentified input");
                break;
        }
    }
    public void ToMainMenu()
    {
        gameUI.SetActive(false);
        shop.SetActive(false);
        pauseMenu.SetActive(false);
        confirmMenu.SetActive(false);
        chooseName.SetActive(false);

        mainMenu.SetActive(true);
        mainMenu.GetComponentInParent<UI_MainMenu>().newGame_Btn.Select();

        canPause = false;

        ArcadeManager.gm.startTimer = false;
        ArcadeManager.gm.wave = 1;
    }
    public void ToDesktop()
    {
        //Application.Quit();
    }

    public void GameOver()
    {
        chooseName.SetActive(true);
    }

    //Functions for SettingsMenu

    IEnumerator MenuTimer(int i)
    {
        canPause = false;

        //call at EndWave
        if (i == 0)
        {
            yield return new WaitForSeconds(2);
            shop.SetActive(true);

            shopHeader.text = "Next Wave: " + ArcadeManager.gm.wave.ToString();

            dakkaGun.Select();
            gameUI.SetActive(false);

            shop.GetComponent<Animator>().SetTrigger("openLerp");
            yield return new WaitForSeconds(2);
            shop.GetComponent<Animator>().SetTrigger("endAnimation");


            canPause = true;
        }

        //call at NewWave
        if (i == 1)
        {
            gameUI.SetActive(true);
            yield return new WaitForSeconds(2);
            canPause = true;
            shop.SetActive(false);
            ArcadeManager.gm.startTimer = true;
            newWave.SetActive(true);
        }
    }
}
