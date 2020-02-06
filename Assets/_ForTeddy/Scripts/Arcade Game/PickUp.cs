using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Range(1, 5)] public float rotationSpeed = 1.0f;
    public enum eRotationType {X, Y, Z};
    [SerializeField]public int coinAmount = 10;
    public eRotationType rotationType = eRotationType.Z;
    public ParticleSystem pickUpParticle;

    private Vector3 rotationDir;
    void Start()
    {
        GetRotationOrient();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationDir * Time.deltaTime * rotationSpeed);
    }

    public void GetRotationOrient()
    {
        switch (rotationType)
        {
            case eRotationType.X:
                rotationDir = new Vector3(90, 0, 0);
                break;
            case eRotationType.Y:
                rotationDir = new Vector3(0, 90, 0);
                break;
            case eRotationType.Z:
                rotationDir = new Vector3(0, 0, 90);
                break;
            default:
                rotationDir = new Vector3(0, 0, 90);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player)
        {
            player.UpdatePlayerScore(coinAmount);
            gameObject.SetActive(false);
        }
    }
}
