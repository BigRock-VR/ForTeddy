using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManager : MonoBehaviour
{
    //GameManager
    public static ArcadeManager gm;
    //GameManager

    [Header("UI Manager")]
    public UI_InGame UiMng;

    [Header("Player Stats")]
    public int ammo;
    public int maxHealth = 1000;
    public int maxHPrice = 500;
    public int maxAPrice = 750;
    public int maxArmor = 500;
    public int health;
    public int score;

    [Header("Wave Info")]
    public int wave;
    public float waveTime = 180f;
    public float waveTimeActual;

    public bool startTimer;

    [Header("Costs")]
    public int ominoCost = 500;
    public int armorCost = 50;
    public int healthCost = 150;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        waveTimeActual = waveTime;
        wave = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            waveTimeActual = Mathf.Clamp(waveTimeActual, 0, waveTime);
            waveTimeActual -= Time.deltaTime;

            if (waveTimeActual <= 0)
            {
                UiMng.EndWave();
                wave++;
                startTimer = false;
            }
            if (health <= 0)
            {
                UiMng.ChangeMenu(3);
                startTimer = false;
            }
        }

        if(wave == 4)
        {
            UiMng.chooseName.SetActive(true);
        }
    }
}
