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
    [SerializeField]
    SphereCollider headCollider;

    [SerializeField][Range(0.4f, 0.99f)]
    float playerHeigth = 0.5f;

    [SerializeField]
    bool isColliding;

    [SerializeField]
    bool isTrigger;

    private void Start()
    {
        if(GetComponent<CapsuleCollider>())
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            isTrigger= true;
        }
        if (GetComponent<SphereCollider>())
        {
            isTrigger = false;
        }
    }
    void FixedUpdate()
    {

        if (isTrigger)
        {
            headPos = transform.InverseTransformPoint(head.transform.position);
            capsuleTrigger.height = headPos.y - playerHeigth;
            headPos.y = capsuleTrigger.height / 2 + playerHeigth;
            capsuleTrigger.center = headPos;
        }

        //if (!isColliding)
        //{
        //   Vector3 center = headPos;
        //    center.y = capsuleCollider.height / 2 + playerHeigth;
        //    capsuleCollider.center = center;
        //}

        //boxCollider.center = headPos;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.name == "Bedroom2")
        //{
        //    return;
        //}
        //
        //if(collision.collider)
        if(isTrigger)
        {
            return;
        }

        isColliding = true;
        SteamVR_Fade.Start(Color.black, 0.5f);
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.name == "Bedroom2")
        //{
        //    return;
        //}
        //
        if (isTrigger)
        {
            return;
        }

        isColliding = false;
        SteamVR_Fade.Start(Color.clear, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
        {
            if (other.gameObject.layer == 16) // place all entry that should "hurt" on layer 16
            {
                SteamVR_Fade.Start(Color.red, 3f);
                Invoke("FadeToClean", 1);
            }
        }
    }

    void FadeToClean()
    {
        SteamVR_Fade.Start(Color.clear, 0.05f);
    }
}

