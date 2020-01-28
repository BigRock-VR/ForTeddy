using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform target = null;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public RenderTexture renderTexture;

    private Vector3 offset;                     // The initial offset from the target.
    private Vector3 rotation = new Vector3(45f,-66.0f,0.0f);

    void Start()
    {
        target = GameManager.Instance.player.transform;
        // Calculate the initial offset.
        offset = transform.position - target.position;
        //transform.rotation = Quaternion.Euler(rotation);

        if (renderTexture && GameManager.isVREnable)
        {
            gameObject.GetComponent<Camera>().targetTexture = renderTexture;
        }

    }

    void FixedUpdate()
    {
        if (target)
        {
            // Create a postion the camera is aiming for based on the offset from the target.
            Vector3 targetCamPos = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }

    }
}
