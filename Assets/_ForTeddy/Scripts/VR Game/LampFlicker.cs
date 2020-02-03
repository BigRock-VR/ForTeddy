using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// added some little randomize on the common flickers + added extension for flicker event
public class LampFlicker : MonoBehaviour
{
    Light lampLight;

    [SerializeField]
    AudioSource soundplayer;

    [SerializeField]
    float minIntensity = 0f;
    float _minInt;

    [SerializeField]
    float maxIntensity = 1.15f;
    float _maxInt;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    float randomWait = 1f;
    float _rWait;

    [SerializeField]
    [Range(0.1f, 1)]
    float flickeringFrequency = 1f;

    void Start()
    {
        lampLight = gameObject.GetComponent<Light>();
    }

    public void DoFlicker()
    {
        StartCoroutine(FlickeringCycle(true));
    }

    IEnumerator FlickeringCycle(bool isFlickered)
    {
        _rWait = randomWait * Random.Range(0.05f, 0.15f);
        _minInt = minIntensity * Random.Range(0.5f, 1.5f);
        _maxInt = maxIntensity * Random.Range(0.5f, 1.5f);

        if (isFlickered)
        {
            _rWait *= 3f;
            _maxInt *= 0.7f;
            _minInt = 0.5f;
            soundplayer.volume = 1f;
            StartCoroutine(FlickeringCycle(false));
        }
        else
        {
            soundplayer.volume = 0.333f * Random.Range(0.5f, 1.5f);
        }

        soundplayer.Play();
        lampLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait);
        lampLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait / Random.Range(1.5f, 2.5f));
        lampLight.intensity = _maxInt;
        yield return new WaitForSeconds(_rWait * Random.Range(1.5f, 2.5f));
        lampLight.intensity = _minInt;
        yield return new WaitForSeconds(_rWait * Random.Range(3f, 7f));
        lampLight.intensity = _maxInt;
        //print("flickering" + isFlickered);
        yield return null;
    }

    private void FixedUpdate()
    {
        if (Random.Range(0f, 10000f) <= flickeringFrequency)
        {
            StartCoroutine(FlickeringCycle(false));
        }
    }

}




