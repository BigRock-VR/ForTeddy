using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportVR : MonoBehaviour
{
    [Header("VR Inputs")]
    [SerializeField]
    SteamVR_Input_Sources vrInputL;
    [SerializeField]
    SteamVR_Input_Sources vrInputR;
    [SerializeField]
    SteamVR_Input_Sources vrInputAny;

    [Header("Fast Teleport Conf")]
    [SerializeField]
    SteamVR_Action_Single primaryInput; 
    [SerializeField]
    float deadZone_primaryInput;
    [SerializeField]
    float timer_primaryInput;
    float _tPI;
    bool _tCheckPI;
    bool _rdyPI;

    [Header("Charging Teleport Conf")]
    [SerializeField]
    SteamVR_Action_Boolean secondaryInput;

    [Header("Switch Input Conf")]
    [SerializeField]
    SteamVR_Action_Boolean switchInput;
    [SerializeField]
    int modeIndex;
    [SerializeField]
    float changeModeTimer = 1;
    float _changeModeTimer = 1;

    private void LateUpdate()
    {
        // Mode change logics
        if (switchInput.GetStateDown(vrInputL) || switchInput.GetStateDown(vrInputR))
        {
            _changeModeTimer = changeModeTimer;
        }

        if (switchInput.GetState(vrInputL) && switchInput.GetState(vrInputL))
        {
            _changeModeTimer -= Time.deltaTime;
        }

        if (switchInput.GetStateUp(vrInputL) || switchInput.GetStateUp(vrInputL))
        {
            _changeModeTimer = changeModeTimer;
        }

        if (_changeModeTimer <= 0)
        {
            modeIndex += 1;
        }


        // Modes
        if (modeIndex == 0)
        {
            // obbligated tp
        }
        if (modeIndex == 0)
        {
            // free tp
        }
        if (modeIndex == 0)
        {
            // free move
        }
    }

    void ZapTP(bool isConstraintToTPPoint)
    {

        if(primaryInput.GetAxis(vrInputAny) <= deadZone_primaryInput && _tCheckPI)
        {
            _tCheckPI = false;
        }

        if (primaryInput.GetAxis(vrInputAny) >= deadZone_primaryInput)
        {
            if (!_tCheckPI)
            {
                _tPI = timer_primaryInput;
                _tCheckPI = true;
            }
            else
            {
                _tPI -= Time.deltaTime;

                if (_tPI <= 0)
                {
                    // holds the trigger for too long, do charging
                }
                else
                {
                    _rdyPI = true;
                }
            }

            if (_rdyPI)
            {
                _rdyPI = false;
                //if(isConstraintToTPPoint change mask + change tp area display
                //if(Physics.Raycast(from controller, forward, infinite, masks?))
                // { do teleport }
                // { else teleportFailed }
            }
        }
    }

    void HoldTP()
    {

    }
}
