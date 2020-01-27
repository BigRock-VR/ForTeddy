using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    Light lampLight;
    int counter;
    public float minIntensity = 0f;
    public float maxIntensity = 1.15f;
    public int FrequencyRange = 5;

    void Start()
    {
        lampLight = gameObject.GetComponent<Light>();
        lampLight.intensity = maxIntensity;
        StartCoroutine("LightCycle");
    }

    IEnumerator LightCycle()
    {

        counter = Random.Range(0, FrequencyRange);

        if (counter == 2)

        {
            lampLight.intensity = minIntensity;
            yield return new WaitForSeconds(0.1f);
            lampLight.intensity = maxIntensity;
            yield return new WaitForSeconds(0.05f);
            lampLight.intensity = minIntensity;
            yield return new WaitForSeconds(2f);
            lampLight.intensity = maxIntensity;
            yield return new WaitForSeconds(5f);
           
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine("LightCycle");
        yield return null;
    }

}
