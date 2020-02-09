using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TeleportVR : MonoBehaviour
{
    [Header("VR Inputs")]
    [SerializeField]
    SteamVR_Input_Sources vrInputL;
    [SerializeField]
    SteamVR_Input_Sources vrInputR;
    [SerializeField]
    Transform leftHand;
    [SerializeField]
    Transform rightHand;
    [SerializeField]
    GameObject freeTPGround;

    [Header("Fast Teleport Conf")]
    [SerializeField]
    SteamVR_Action_Single primaryInput;

    [Header("Switch Input Conf")]
    [SerializeField]
    SteamVR_Action_Boolean switchInput;
    [SerializeField]
    float changeModeTimer = 1;

    
    [Header("Debug")]
    [SerializeField]
    int modeIndex;
    [SerializeField]
    float _changeModeTimer = 1;
    [SerializeField]
    bool isSwitched;
    [SerializeField]
    VRTeleportArea tpArea;
    [SerializeField]
    Player player;
    [SerializeField]
    float minDistanceFromHead;
    [SerializeField]
    bool teleporting = false;

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
            isSwitched = true;

            if (modeIndex == 2)
            {
                modeIndex = 0;
            }
            else
            {
                modeIndex += 1;
            }
        }


        // Modes
        if (isSwitched)
        {
            var allTPArea = FindObjectsOfType<TeleportVR>();
            var touchMovement = GetComponent<MovementVR>();
            if (modeIndex == 0)
            {
                isSwitched = false;
                _changeModeTimer = changeModeTimer;
                touchMovement.canMove = false;
                freeTPGround.SetActive(false);
                foreach(TeleportVR tp in allTPArea)
                {
                    tp.gameObject.SetActive(true);
                }
            }
            if (modeIndex == 1)
            {
                isSwitched = false;
                _changeModeTimer = changeModeTimer;
                touchMovement.canMove = false;
                freeTPGround.SetActive(true);
                foreach (TeleportVR tp in allTPArea)
                {
                    tp.gameObject.SetActive(true);
                }
            }
            if (modeIndex == 2)
            {
                isSwitched = false;
                _changeModeTimer = changeModeTimer;
                touchMovement.canMove = true;
                freeTPGround.SetActive(false);
                foreach (TeleportVR tp in allTPArea)
                {
                    tp.gameObject.SetActive(false);
                }
            }
        }


        if (modeIndex == 0 || modeIndex == 1)
        {
            var disL = leftHand.position - player.hmdTransform.position;
            disL.y = 0;
            var disR = rightHand.position - player.hmdTransform.position;
            disR.y = 0;

            var distL = disL.magnitude;
            var distR = disR.magnitude;

            if (distL >= minDistanceFromHead || distR >= minDistanceFromHead)
            {

                if (distL >= distR)
                {
                    Debug.DrawRay(leftHand.position, leftHand.forward, Color.yellow);
                    TeleportChecks(leftHand);
                }
                else
                {
                    Debug.DrawRay(rightHand.position, rightHand.forward, Color.blue);
                    TeleportChecks(rightHand);
                }
            }

        }

    }
    void TeleportChecks(Transform Hand)
    {
        Ray ray = new Ray(Hand.position, Hand.forward);

        if (Physics.Raycast(ray, out RaycastHit info, 17) && !teleporting) // checks script presence to get TPlights ground & TPArea has to be on layer 17
        {
            if (primaryInput.GetAxis(vrInputL) > 0)
            {
                if (modeIndex == 0)
                {
                    //print(info.collider.name);
                    if (info.transform.GetComponent<VRTeleportArea>() && tpArea == null)
                    {
                        print("found a destination, trying to turn it on");
                        tpArea = info.transform.GetComponent<VRTeleportArea>();
                        tpArea.checkDestination();
                    }
                    if (!info.transform.GetComponent<VRTeleportArea>() && tpArea != null)
                    {
                        print("removing destination");
                        tpArea.deselectDestination();
                        tpArea = null;
                    }
                    if (primaryInput.GetAxis(vrInputL) <= 0.666f && tpArea != null)
                    {
                        teleporting = true;
                        DoTeleport(true, tpArea.transform.position);
                    }
                }
                if (modeIndex == 1)
                {
                    if (primaryInput.GetAxis(vrInputL) <= 0.666f)
                    {
                        teleporting = true;
                        DoTeleport(false, info.point);
                    }
                }
            }
        }
    }

    void DoTeleport(bool _isConstraintToTPPoint, Vector3 destination)
    {
        print("doing teleport");
        if (_isConstraintToTPPoint)
        {
            print("i hit a fixed tp location");
            player.transform.position = destination;
        }
        else
        {
            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position = destination + playerFeetOffset;
        }

        teleporting = false;
    }

    private void Start()
    {
        player = Player.instance;
    }
}
    

