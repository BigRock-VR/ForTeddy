using System.Collections;
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

    void LateUpdate()
    {
        inputMovement = movementThumbstick.GetAxis(VR_Input);
        inputLook = lookThumbstick.GetAxis(VR_Input);

        // check if not exeeded max speed
        if (playerRigidBody.velocity.magnitude <= maxSpeed)
        {
            // movement L
            if (inputMovement.x != 0 || inputMovement.y != 0)
            {
                //transform.position += transform.TransformDirection(new Vector3(inputMovement.x, 0, inputMovement.y));
                playerRigidBody.AddForce(transform.forward * inputMovement.y * Time.deltaTime * speedModifier, ForceMode.VelocityChange);
                playerRigidBody.AddForce(transform.right * inputMovement.x * Time.deltaTime * speedModifier, ForceMode.VelocityChange);
            }

            // look R
            if (inputLook.x != 0 || inputLook.y != 0)
            {
                transform.Rotate(new Vector3(0, inputLook.x, 0) * Time.deltaTime * turnModifier);
            }
        }
        if (runPressed.GetStateDown(VR_Input))
        {
            speedModifier *= 1.82f;
        }
        if (runPressed.GetStateUp(VR_Input))
        {
            speedModifier /= 1.82f;

        }
        if(focusPressed.GetStateDown(VR_Input))
        {
            turnModifier *= 2;
        }
        if(focusPressed.GetStateUp(VR_Input))
        {
            turnModifier /= 2;
        }
    }
}

