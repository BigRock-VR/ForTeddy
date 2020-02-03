using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
using Valve.VR;

public class PlayerCollisions : MonoBehaviour
{
    public Transform head;

    Vector3 headPos;

    [SerializeField]
    private CapsuleCollider capsuleCollider, capsuleTrigger;
    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField][Range(0.4f, 0.99f)]
    float playerHeigth = 0.5f;

    [SerializeField]
    bool isColliding;

    void FixedUpdate()
    {

        headPos = transform.InverseTransformPoint(head.transform.position);

        if (!isColliding)
        {
            Vector3 center = headPos;
            center.y = capsuleCollider.height / 2 + playerHeigth;
            capsuleCollider.center = center;
        }

        boxCollider.center = headPos;

        capsuleTrigger.height = headPos.y - playerHeigth;
        headPos.y = capsuleTrigger.height / 2 + playerHeigth;
        capsuleTrigger.center = headPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Bedroom2")
        {
            return;
        }

        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Bedroom2")
        {
            return;
        }

        isColliding = false;

    }
}

