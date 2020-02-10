using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public RenderTexture renderTexture;

    private RaycastHit _rayHit;
    public Transform playerSphere;
    private Vector3 sphereMaxRadius = new Vector3(3, 3, 3);
    void Start()
    {
        if (renderTexture && GameManager.isVREnable)
        {
            gameObject.GetComponent<Camera>().targetTexture = renderTexture;
        }
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, (playerSphere.position - transform.position).normalized, out _rayHit, Mathf.Infinity))
        {
            if (_rayHit.transform.CompareTag("Env"))
            {
                playerSphere.localScale = sphereMaxRadius;
            }
            else
            {
                playerSphere.localScale = Vector3.zero;
            }
        }
    }
}
