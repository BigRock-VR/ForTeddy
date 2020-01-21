using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    [Header("WavePanels")]
    public GameObject newWave;
    public GameObject endWave;

    public Text newWave_Txt;
    public Text endWave_Txt;

    [Header("ShopUI")]
    public Text shopHeader;
    public GameObject shop;

    public Text shopCoin;

    [Header("OverlayText Manager")]
    float alpha;
    float counter;
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

    private void Start()
    {
        endWave.SetActive(false);
        newWave.SetActive(false);
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

    }

    IEnumerator MenuTimer(int i)
    {
        if (i == 0)
        {
            yield return new WaitForSeconds(2);
            //Activate the shop screen
            shopHeader.text = "Next Wave: " + GameManager.gm.wave.ToString();
            shop.SetActive(true);
            shop.GetComponent<Animator>().SetTrigger("openLerp");
        }
        if(i==1)
        {
            yield return new WaitForSeconds(2);
            GameManager.gm.startTimer = true;
            newWave.SetActive(true);
            shop.SetActive(false);
        }
             
    }
}
