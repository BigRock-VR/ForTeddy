using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapTargetToObject : MonoBehaviour
{
    [SerializeField]
    List<Rigidbody> targets;

    [SerializeField]
    Transform oldParent;
    
    [SerializeField]
    Vector3 Rotation;

    [SerializeField]
    Transform snapPoint;

    [SerializeField]
    public Rigidbody possibleTarget;

    [SerializeField]
    public bool isFilled;



    private void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (targets.Contains(rb))
        {
            print("this target : " + rb.gameObject.name + " is in list");
            possibleTarget = rb;
            oldParent = possibleTarget.transform.parent;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //print("the target is colliding with : " + other);
        if (possibleTarget == other.attachedRigidbody && possibleTarget.useGravity == true && !isFilled)
        {
            isFilled = true;
            print("this target : " + possibleTarget.gameObject.name + " had been attached");
            possibleTarget.isKinematic = true;
            possibleTarget.transform.parent = snapPoint;
            possibleTarget.transform.localPosition = Vector3.zero;
            possibleTarget.transform.localRotation = Quaternion.Euler(Rotation);
        }
        if (possibleTarget.useGravity == false && isFilled)
        {
            possibleTarget.isKinematic = false;
            isFilled = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(possibleTarget != null && possibleTarget.transform.parent == transform)
        {
            possibleTarget.transform.parent = oldParent;
            possibleTarget.isKinematic = false;
            possibleTarget.useGravity = true;
            possibleTarget = null;
            isFilled = false;
        }

    }
}
