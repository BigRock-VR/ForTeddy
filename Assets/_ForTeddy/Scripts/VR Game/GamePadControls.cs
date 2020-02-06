using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GamePadControls : MonoBehaviour
{

    public PlayerMovement playerGO;

    [SerializeField]
    Transform padL, padR;

    [SerializeField]
    Transform restL, restR;

    [SerializeField]
    bool isLeftTriggered, isLeftGrabbed, isRightTriggered, isRightGrabbed, isPlaying;

    [SerializeField]
    SteamVR_ActionSet arcadeSet, VRSet;

    [SerializeField]
    SteamVR_Action_Single primaryInput;

    [SerializeField]
    SteamVR_Action_Boolean secondaryInput;

    [SerializeField]
    SteamVR_Input_Sources vrInputL, vrInputR;

    [SerializeField]
    Rigidbody rbPad1, rbPad2;
    private bool canDoPhysicalM, canDoPhysicalL;
    bool isLeft, isRight;

    void Start()
    {
        if (!GameObject.FindWithTag("Player"))
        {
            return;
        }
        playerGO = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerGO)
        {
            return;
        }

        if(canDoPhysicalL || canDoPhysicalM)
        {
            DoPhysicalMovement();
        }

        if(isLeft)
        {
            if (secondaryInput.GetStateDown(vrInputL))
            {
                isLeftGrabbed = !isLeftGrabbed;

                if (isLeftGrabbed)
                {
                    print("is kinematic " + isLeftGrabbed);
                    rbPad1.isKinematic = true;
                }
                else
                {
                    rbPad1.isKinematic = false;
                }
            }


        }

        if(isRight)
        {
            if (secondaryInput.GetStateDown(vrInputR))
            {
                isRightGrabbed = !isRightGrabbed;

                if(isRightGrabbed)
                {
                    print("is kinematic " + isRightGrabbed);
                    rbPad2.isKinematic = true;
                }
                else
                {
                    rbPad2.isKinematic = false;
                }
            }
        }

        if (isLeftTriggered && isLeftGrabbed && !canDoPhysicalM)
        {
            canDoPhysicalM = true;
        }
        if (isLeftTriggered && isLeftGrabbed && !canDoPhysicalL)
        {
            canDoPhysicalL = true;
        }

        if (isLeftTriggered && rbPad1.isKinematic && !isPlaying)
        {
            isPlaying = true;
        }
        
        if (isRightTriggered && rbPad2.isKinematic && !isPlaying)
        {
            isPlaying = true;
        }
    }

    void DoPhysicalMovement()
    {
        if (canDoPhysicalM)
        {
            Vector3 dirL = restL.localPosition - padL.localPosition;
            dirL.y = 0;

            if (!Mathf.Approximately(dirL.magnitude, (float)3e-05) || !Mathf.Approximately(dirL.magnitude, -(float)3e-05))
            {
                dirL *= 25f;
                playerGO.Move(dirL);
            }
        }

        if (canDoPhysicalL)
        {
            Vector3 dirR = restR.localPosition - padR.localPosition;
            dirR.y = 0;

            if (!Mathf.Approximately(dirR.magnitude, (float)3e-05) || !Mathf.Approximately(dirR.magnitude, -(float)3e-05))
            {
                playerGO.Aim(new Vector3(dirR.x, 0, dirR.z));
            }
        }
    }


    public void isLeftGrab()
    {
        isLeft = !isLeft;

        if (primaryInput.GetAxis(vrInputL) == 1)
        {
            isLeftTriggered = true;
            //rb.isKinematic = true;
        }
        else
        {
            isLeftTriggered = false;
            isLeftGrabbed = false;
            rbPad1.isKinematic = false;
        }

        print("is left " + isLeft);
        activateArcadeSet();
    }

    public void isRightGrab()
    {
        isRight = !isRight;

        if (primaryInput.GetAxis(vrInputR) == 1)
        {
            isRightTriggered = true;
            //rb.isKinematic = true;
        }
        else
        {
            isRightTriggered = false;
            isRightGrabbed = false;
            rbPad2.isKinematic = false;
        }

        print("is right " + isRight);
        activateArcadeSet();
    }
//!isLeftGrabbed && !isLeftTriggered && !isRightTriggered && !isRightGrabbed
    void activateArcadeSet()
    {
        if (!isLeftGrabbed && !isLeftTriggered)
        {
            //isPlaying = false;
            canDoPhysicalL = false;
            rbPad1.isKinematic = false;
        }
        if (!isRightGrabbed && !isRightTriggered)
        {
            //isPlaying = false;
            canDoPhysicalM = false;
            rbPad2.isKinematic = false;
        }
        if(!isRightGrabbed && !isRightTriggered && !isLeftGrabbed && !isLeftTriggered)
        {
            isPlaying = false;
        }



        if (isPlaying)
        {
            print("i am in play mode");
            //canDoPhysical = false;
            arcadeSet.Activate(SteamVR_Input_Sources.Any, 99, false);
            VRSet.Deactivate(SteamVR_Input_Sources.Any);
            //rb.isKinematic = true;
            //rb.useGravity = false;
        }
        else
        {
            print("i am in vr mode");
            //canDoPhysical = true;
            VRSet.Activate(SteamVR_Input_Sources.Any, 99, false);
            arcadeSet.Deactivate(SteamVR_Input_Sources.Any);
            //rb.isKinematic = false;
            //rb.useGravity = true;
        }
    }
}
