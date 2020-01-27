using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementChrController : MonoBehaviour
{
    [Range(15.0f, 100.0f)]
    public float speed = 15.0f;
    public float aimSensibility = 10.0f;
    private CharacterController chrController = null;
    private Vector3 movementDir = Vector3.zero;
    private Vector3 aimDirection = Vector3.zero;

    void Start()
    {
        chrController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Aim();
    }

    void Move()
    {
        if (GameManager.isVREnable)
        {
            Debug.Log("VR Active");
        }
        else
        {
            Debug.Log("VR NOT ACTIVE");
        }
        movementDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movementDir = transform.TransformDirection(movementDir);
        movementDir *= speed;
        chrController.Move(movementDir * Time.deltaTime);
    }

    void Aim()
    {
        aimDirection = new Vector3(0, Input.GetAxis("Mouse X"), 0) * aimSensibility * Time.deltaTime;
        transform.Rotate(aimDirection);
    }
}
