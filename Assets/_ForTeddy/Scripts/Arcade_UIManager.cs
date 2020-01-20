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
    public Text newWave;
    public Text endWave;

    [Header("OverlayText Manager")]
    public bool newWaveBool;
    public bool endWaveBool;
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
        if (newWaveBool)
        {
            newWave.text = "Wave N " + GameManager.gm.wave + " Starts!";

            newWave.gameObject.SetActive(true);
            counter += Time.deltaTime/5;
            alpha = Mathf.Lerp(1, 0, counter);
            newWave.color = new Color(newWave.color.r, newWave.color.g, newWave.color.b, alpha);
            print(alpha);
            if(alpha <= 0)
            {
                newWave.gameObject.SetActive(false);
                counter = 0;
                newWaveBool = false;
            }
        }
    }
    public void EndWave()
    {
        if (endWaveBool)
        {
            endWave.gameObject.SetActive(true);
            counter += Time.deltaTime / 5;
            alpha = Mathf.Lerp(1, 0, counter);
            endWave.color = new Color(newWave.color.r, newWave.color.g, newWave.color.b, alpha);
            print(alpha);
            if (alpha <= 0)
            {
                endWave.gameObject.SetActive(false);
                counter = 0;
                endWaveBool = false;
            }
        }
    }

    private void Start()
    {
        newWave.color = new Color(0, 0, 0, 0);
        endWave.color = new Color(0, 0, 0, 0);
    }
    void Update()
    {

        //Update Ammo/Score GameManager Based
        ammoCount.text = GameManager.gm.ammo.ToString();
        scoreCount.text = GameManager.gm.score.ToString();
        
        //Call here, Better if called By GameManager when HealthUpdate
        HealthUI();

        //Update Slider Value
        WaveTimer();

        NewWave();
        EndWave();
    }
}
