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
    Rigidbody possibleTarget;

    [SerializeField]
    bool isFilled;



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

            print("this target : " + possibleTarget.gameObject.name + " had been attached");
            possibleTarget.isKinematic = true;
            possibleTarget.transform.parent = snapPoint;
            possibleTarget.transform.localPosition = Vector3.zero;
            var x = possibleTarget.transform.localRotation = Quaternion.Euler(Rotation);
            isFilled = true;
        }

        if (isFilled && possibleTarget.useGravity == true)
        {
            isFilled = false;
            possibleTarget.isKinematic = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(possibleTarget != null && possibleTarget.transform.parent == transform)
        {
            possibleTarget.transform.parent = oldParent;
            possibleTarget = null;
            isFilled = false;
        }

    }
}
