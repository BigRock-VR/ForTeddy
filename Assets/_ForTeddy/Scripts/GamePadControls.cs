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
    bool isLeftGrabbed, isRightGrabbed, isPlaying;
    // Start is called before the first frame update

    [SerializeField]
    SteamVR_ActionSet arcadeSet, VRSet;

    Rigidbody rb;
    private bool canDoPhysical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        if(canDoPhysical)
        {
        DoPhysicalMovement();

        }
    }

    void DoPhysicalMovement()
    {
        Vector3 dirL = restL.localPosition - padL.localPosition;
        dirL.y = 0;

        Vector3 dirR = restR.localPosition - padR.localPosition;
        dirR.y = 0;

        //print("L: " + dirL + " , R: " + dirR);
        //print(dirL.magnitude + " and this is for aim : " + dirR.magnitude);
        if (!Mathf.Approximately(dirL.magnitude, (float)3e-05) || !Mathf.Approximately(dirL.magnitude, -(float)3e-05))
        {
            //dirL = dirL.normalized;
            dirL *= 100f;
            playerGO.Move(dirL);
        }
        if (!Mathf.Approximately(dirR.magnitude, (float)3e-05) || !Mathf.Approximately(dirR.magnitude, -(float)3e-05))
        {
            //dirR = dirR.normalized;
            //dirR *= 100f;
            playerGO.Aim(new Vector3(dirR.x, 0, dirR.z));
        }
    }

    public void isLeftGrab()
    {
        isLeftGrabbed = !isLeftGrabbed;
        activateArcadeSet();
    }

    public void isRightGrab()
    {
        isRightGrabbed = !isRightGrabbed;
        activateArcadeSet();
    }

    void activateArcadeSet()
    {
        if (isLeftGrabbed && isRightGrabbed)
        {
            isPlaying = true;
        }
        else
        {
            isPlaying = false;
        }

        if (isPlaying)
        {
            //print("i am in play mode");
            canDoPhysical = false;
            arcadeSet.Activate(SteamVR_Input_Sources.Any, 99, false);
            VRSet.Deactivate(SteamVR_Input_Sources.Any);
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else
        {
            //print("i am in vr mode");
            canDoPhysical = true;
            VRSet.Activate(SteamVR_Input_Sources.Any, 99, false);
            arcadeSet.Deactivate(SteamVR_Input_Sources.Any);
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
