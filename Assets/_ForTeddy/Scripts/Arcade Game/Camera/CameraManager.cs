using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public RenderTexture renderTexture;
    void Start()
    {
        if (renderTexture && GameManager.isVREnable)
        {
            gameObject.GetComponent<Camera>().targetTexture = renderTexture;
        }
    }
}
