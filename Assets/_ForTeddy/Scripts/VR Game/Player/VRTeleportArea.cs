using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeleportArea : MonoBehaviour
{
    [SerializeField]
    bool isPlayerIn;

    [SerializeField]
    bool isActive;

    [SerializeField]
    Light tpLight;
    public void checkDestination()
    {
        print("checking this destination : " + gameObject.name);
        if(!isPlayerIn && !isActive)
        {
            print("activating light");
            isActive = true;
            tpLight.gameObject.SetActive(true);
        }
    }

    public void deselectDestination()
    {
        print("disabling light");
        tpLight.gameObject.SetActive(false);
        isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other);
        if (other.gameObject.name == "HeadCollider")
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HeadCollider")
        {
            isPlayerIn = false;
        }
    }
}
