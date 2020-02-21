//#define ENABLE_ROTATION
using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] public int coinAmount = 10;
    public MeshRenderer mesh;
#if ENABLE_ROTATION
    [Range(0, 5)] public float rotationSpeed = 1.0f;
    public enum eRotationType {X, Y, Z};
    public eRotationType rotationType = eRotationType.Z;
    private Vector3 rotationDir;
#endif
    public AnimationCurve animationCurve;
    public ParticleSystem pickUpParticle;
    public ParticleSystem collectableParticle, smokeParticle0;
    public ParticleSystem smokeParticle1, smokeParticle2;
    private bool isPicked;
    private float time;

    void Start()
    {

#if ENABLE_ROTATION
        GetRotationOrient();
#endif
        collectableParticle.Play();
        time = animationCurve.keys[animationCurve.keys.Length - 1].time;
        StartCoroutine(InitiSize());
    }

#if ENABLE_ROTATION
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
#endif
    IEnumerator InitiSize()
    {
        float t = 0.0f; // TIME
        while (t < 1)
        {
            t += Time.deltaTime / time;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(t));
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player)
        {
            if (isPicked)
            {
                return;
            }

            player.UpdatePlayerScore(coinAmount);
            isPicked = true;
            mesh.enabled = false;
            pickUpParticle.Play();
            var collectableMain = collectableParticle.main;
            var smokeMain0 = smokeParticle0.main;
            var smokeMain1 = smokeParticle1.main;
            var smokeMain2 = smokeParticle2.main;
            collectableMain.loop = false;
            smokeMain0.loop = false;
            smokeMain1.loop = false;
            smokeMain2.loop = false;

            Destroy(gameObject, 3.0f);
        }
    }
}
