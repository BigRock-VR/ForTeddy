using FMODUnity;
using UnityEngine;

public class FMODPlaySound : MonoBehaviour
{
    [EventRef] [SerializeField] private string m_SoundPath = "";
    public void Play(Vector3 worldPosition)
    {
        RuntimeManager.PlayOneShot(m_SoundPath, worldPosition);
    }

    public void Play()
    {
        RuntimeManager.PlayOneShot(m_SoundPath);
    }
}
