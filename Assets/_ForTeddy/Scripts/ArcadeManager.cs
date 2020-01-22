using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManager : MonoBehaviour
{
    //GameManager
    public static ArcadeManager gm;
    //GameManager

    public GameObject UiMng;

    public int ammo;
    public int health;
    public int score;
    public int wave;

    public float waveTime = 180f;
    public float waveTimeActual;
    public bool startTimer = true;

    private void Awake()
    {
        if(gm == null)
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

            if(waveTimeActual <= 0)
            {
                UiMng.GetComponent<Arcade_UIManager>().EndWave();
                wave++;
                startTimer = false;
            }
        }
    }
}
