using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aura2API;

public class VRTeleportArea : MonoBehaviour
{
    [SerializeField]
    bool isPlayerIn;

    [SerializeField]
    public bool isActive;
    public bool isUsingFog;

    [SerializeField]
    Light tpLight;
    [SerializeField]
    AuraVolume tpFog;

    [SerializeField]
    float degree;

    [SerializeField]
    List<VRTeleportArea> otherTPArea;

    public void checkDestination()
    {
        if(!isPlayerIn && !isActive)
        {
            isUsingFog = true;
            isActive = true;
        }
    }

    public void deselectDestination()
    {
        isActive = false;
    }

    private void LateUpdate()
    {


        if (isActive)
        {
            if (tpLight.intensity <= 2.5f)
            {
                tpLight.intensity += 1 / degree;
            }
            if (tpFog.densityInjection.strength <= 17)
            {
                tpFog.densityInjection.strength += 1 / degree * 7.77f;
            }

        }
        else
        {
            for (int i = 0; i < otherTPArea.Count; i++)
            {
                if (otherTPArea[i].isUsingFog == true)
                {
                    isUsingFog = false;
                }
            }
            if (tpLight.intensity > 0)
            {
                tpLight.intensity -= 1 / degree;
            }
            if (isUsingFog)
            {
                if (tpFog.densityInjection.strength > 0)
                {
                    tpFog.densityInjection.strength -= 1 / degree * 7.77f;
                }
                else
                {
                    isUsingFog = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "SteamVRObjects")
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "SteamVRObjects")
        {
            isPlayerIn = false;
            isActive = false;
        }
    }

}
