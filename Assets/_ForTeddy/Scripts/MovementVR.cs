﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MovementVR : MonoBehaviour
{

    [SerializeField]
    [Range(0.75f, 1000f)]
    private float speedModifier;
    [SerializeField]
    [Range(1f, 100f)]
    private float turnModifier;

    // STEAM VR VARIABLES
    [SerializeField]
    SteamVR_Input_Sources VR_Input;
    [SerializeField]
    SteamVR_Action_Vector2 movementThumbstick;
    [SerializeField]
    SteamVR_Action_Vector2 lookThumbstick;
    [SerializeField]
    SteamVR_Action_Boolean runPressed, focusPressed;

    [SerializeField]
    Vector2 inputMovement, inputLook;
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    Rigidbody playerRigidBody;
    CameraArm cameraArm;

    private void Start()
    {
        cameraArm = playerRigidBody.GetComponent<CameraArm>();
    }
    void LateUpdate()
    {
        inputMovement = movementThumbstick.GetAxis(VR_Input);
        inputLook = lookThumbstick.GetAxis(VR_Input);

        print(inputMovement);
        // check if not exeeded max speed
        if (playerRigidBody.velocity.magnitude <= maxSpeed)
        {
            // movement L
            if (inputMovement.x != 0 || inputMovement.y != 0)
            {
                playerRigidBody.AddForce(playerRigidBody.transform.forward * inputMovement.y * Time.deltaTime * speedModifier, ForceMode.VelocityChange);
                playerRigidBody.AddForce(playerRigidBody.transform.right * inputMovement.x * Time.deltaTime * speedModifier, ForceMode.VelocityChange);
            }

            // look R
            if (inputLook.x != 0 || inputLook.y != 0)
            {
                cameraArm.doCameraRotation(inputLook.x, turnModifier, true);
            }
            else
            {
                cameraArm.doCameraRotation(0, 0, false);
            }
        }
        if (runPressed.GetStateDown(VR_Input))
        {
            speedModifier *= 4f;
        }
        if (runPressed.GetStateUp(VR_Input))
        {
            speedModifier /= 4f;

        }
        if (focusPressed.GetStateDown(VR_Input))
        {
            turnModifier *= 5f;
        }
        if (focusPressed.GetStateUp(VR_Input))
        {
            turnModifier /= 5f;
        }
    }
}

