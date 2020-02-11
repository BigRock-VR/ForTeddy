using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// added some little randomize on the common thunders + added extension for hard thunder event
public class Lightning : MonoBehaviour
{
    Light thunderLight;

    [SerializeField]
    AudioController audioControl;

    [SerializeField]
    float minIntensity = 0.6f;
    float _minInt;

    [SerializeField]
    float maxIntensity = 5f;
    float _maxInt;

    [SerializeField][Range(0.5f,1.5f)]
    float randomWait = 1;
    float _rWait;

    [SerializeField][Range(0.1f,1)]
    float lightningFrequency = 1f;

    [SerializeField]
    List<Transform> thunderOrigins;

    float volume;

    void Start()
    {
        thunderLight = gameObject.GetComponent<Light>();
        thunderLight.enabled = false;
    }

    public void DoLighting()
    {
        StartCoroutine(LightningCycle(true));
    }

    IEnumerator LightningCycle(bool isHardThunder)
    {
        thunderLight.enabled = true;

        _rWait = randomWait * Random.Range(0.33f, 0.47f);
        _minInt = minIntensity * Random.Range(0.5f, 1.5f);
        _maxInt = maxIntensity * Random.Range(0.5f, 1.5f);

        if (isHardThunder)
        {
            _rWait *= 0.1f;
            _maxInt *= 2f;
            _minInt *= 0.5f;
            volume = 0.969f;
        }
        else
        {
            volume = 0.333f * Random.Range(0.5f,1.5f);
        }

        thunderLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait);
        thunderLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait/3);
        thunderLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait/2);
        thunderLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait*0.3f);
        audioControl.RandomThunder(volume, thunderOrigins[Random.Range(0,thunderOrigins.Count)]);
        thunderLight.enabled = false;
        yield return null;
    }

    private void FixedUpdate()
    {
        if(Random.Range(0f,10000f) <= lightningFrequency)
        {
            StartCoroutine(LightningCycle(false));
        }
    }



}

    


