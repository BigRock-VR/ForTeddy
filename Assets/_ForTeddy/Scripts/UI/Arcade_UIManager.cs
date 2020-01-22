using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Arcade_UIManager : MonoBehaviour
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

    private bool pauseGame;
    [Header("First Button Selected")]
    public Button dakkaGun;
    private Button back;
    private Button toMainMenu;
    private Button resume;

    private bool changingMenu;
    private void Start()
    {
        StartSettings();
    }

    void StartSettings()
    {
        endWave.SetActive(false);
        newWave.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        confirmMenu.SetActive(false);


        back = settingsMenu.GetComponentInChildren<Button>();
        toMainMenu = confirmMenu.GetComponentInChildren<Button>();
        resume = pauseMenu.GetComponentInChildren<Button>();
    }
    void Update()
    {
        //Update Ammo/Score GameManager Based
        ammoCount.text = GameManager.gm.ammo.ToString();
        scoreCount.text = GameManager.gm.score.ToString();
        shopCoin.text = scoreCount.text;

        //Call here, Better if called By GameManager when HealthUpdate
        HealthUI();

        //Update Slider Value
        WaveTimer();

        if (Input.GetButtonDown("Cancel") && changingMenu == false)
        {
            PauseMenu();
        }
    }

    //HealthManager Update by GameManager Health Value (int 0-6)
    public void HealthUI()
    {
        switch (GameManager.gm.health)
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

    //remap from TimeGame for In game slider
    public void WaveTimer()
    {
        //Remap Timer Value for SliderLerp
        float newTimer = GameManager.gm.waveTimeActual.Remap(0, GameManager.gm.waveTime, 0, 1);

        waveTime.value = Mathf.Lerp(10, 0, newTimer);
    }

    public void NewWave()
    {
        newWave_Txt.text = "Wave N " + GameManager.gm.wave + " Starts!";

        endWave.SetActive(false);
        shop.GetComponent<Animator>().SetTrigger("closeLerp");

        StartCoroutine(MenuTimer(1));

        GameManager.gm.waveTimeActual = GameManager.gm.waveTime;

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

            if (!pauseGame)
            {
                if (shop.activeInHierarchy)
                {
                    pauseMenu.SetActive(false);
                    dakkaGun.Select();
                }
                else
                {
                    //gameUI.SetActive(true);
                    pauseMenu.SetActive(false);
                    GameManager.gm.startTimer = true;
                }
            }
            else
            {
                //gameUI.SetActive(false);
                pauseMenu.SetActive(true);
                resume.Select();
                GameManager.gm.startTimer = false;
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
                resume.Select();
                break;
            //From Back Button in SettingsMenu
            case 3:
                settingsMenu.SetActive(false);
                resume.Select();
                break;
            default:
                print("Unideentified input");
                break;
        }
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ToDesktop()
    {
        Application.Quit();
    }

    //Functions for SettingsMenu

    IEnumerator MenuTimer(int i)
    {
        changingMenu = true;
        //call at EndWave
        if (i == 0)
        {
            yield return new WaitForSeconds(2);
            shop.SetActive(true);

            shopHeader.text = "Next Wave: " + GameManager.gm.wave.ToString();

            dakkaGun.Select();
            gameUI.SetActive(false);

            shop.GetComponent<Animator>().SetTrigger("openLerp");
            yield return new WaitForSeconds(2);
            shop.GetComponent<Animator>().SetTrigger("endAnimation");
            changingMenu = false;
        }
        //call at NewWave
        if (i == 1)
        {
            gameUI.SetActive(true);
            yield return new WaitForSeconds(2);
            changingMenu = false;
            shop.SetActive(false);
            GameManager.gm.startTimer = true;
            newWave.SetActive(true);
        }

    }
}
