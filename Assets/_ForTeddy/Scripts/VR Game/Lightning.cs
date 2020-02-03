using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// added some little randomize on the common thunders + added extension for hard thunder event
public class Lightning : MonoBehaviour
{
    Light outdoorLight;

    [SerializeField]
    AudioSource soundplayer;

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

    void Start()
    {
        outdoorLight = gameObject.GetComponent<Light>();
        outdoorLight.enabled = false;
    }

    public void DoLighting()
    {
        StartCoroutine(LightningCycle(true));
    }

    IEnumerator LightningCycle(bool isHardThunder)
    {
        outdoorLight.enabled = true;

        _rWait = randomWait * Random.Range(0.33f, 0.47f);
        _minInt = minIntensity * Random.Range(0.5f, 1.5f);
        _maxInt = maxIntensity * Random.Range(0.5f, 1.5f);

        if (isHardThunder)
        {
            _rWait *= 0.1f;
            _maxInt *= 2f;
            _minInt *= 0.5f;
            soundplayer.volume = 0.969f;
        }
        else
        {
            soundplayer.volume = 0.333f * Random.Range(0.5f,1.5f);
        }

        outdoorLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait);
        outdoorLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait/3);
        outdoorLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait/2);
        outdoorLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait*0.3f);
        soundplayer.Play();
        //print("thundering" + isHardThunder);
        outdoorLight.enabled = false;
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

    


