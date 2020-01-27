using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraArm : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    bool doesRotate;
    [SerializeField]
    float timer = 0;

    private void Start()
    {
        if (GetComponentInChildren<Camera>() != null)
        {
            cam = GetComponentInChildren<Camera>();
        }
    }

    public void doCameraRotation(float rotation, float modifier, bool doingRotation)
    {
        doesRotate = doingRotation;

        cam.transform.localPosition = Vector3.zero;

        if (timer <= 1 && doesRotate)
        {
            timer += Time.deltaTime * 0.1f;

            transform.Rotate(new Vector3(0, rotation, 0) * timer * modifier);
        }
        else if (timer > 1 && doesRotate)
        {
            timer = 1;
            transform.Rotate(new Vector3(0, rotation, 0) * timer * modifier);
        }
        else if (!doesRotate)
        {
            cam.transform.localPosition = Vector3.zero;

        }
    }

}
