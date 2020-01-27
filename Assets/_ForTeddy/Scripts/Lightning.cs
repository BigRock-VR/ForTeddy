using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    Light outdoorLight;
    int counter;
    public GameObject soundplayer;
    public float minIntensity = 0.6f;
    public float maxIntensity = 5f;
    public int lightningFrequencyRange = 5;

    void Start()
    {
        outdoorLight = gameObject.GetComponent<Light>();
        StartCoroutine("LightningCycle");
    }

    IEnumerator LightningCycle()
    {

       counter = Random.Range(0,lightningFrequencyRange);
       
        if(counter == 2)

        {
             outdoorLight.intensity = maxIntensity;
            yield return new WaitForSeconds(0.4f);
            outdoorLight.intensity = minIntensity;
            yield return new WaitForSeconds(0.15f);
            outdoorLight.intensity = maxIntensity;
            yield return new WaitForSeconds(0.2f);
            outdoorLight.intensity = minIntensity;
            yield return new WaitForSeconds(1.5f);
            soundplayer.SetActive(true);
            yield return new WaitForSeconds(9);
            soundplayer.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine("LightningCycle");
        yield return null;
    }

    


}

    


