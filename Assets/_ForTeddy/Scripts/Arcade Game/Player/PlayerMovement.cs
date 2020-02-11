using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;    // The speed that the player will move at.
    [SerializeField]public float aimSensibility = 10.0f;   // The rotation sensibility of the joypad.

    [SerializeField]
    SteamVR_Action_Vector2 inputL, inputR;

    [SerializeField]
    SteamVR_Input_Sources VRInput;

    private Animator anim;

    private Rigidbody rb;
    public bool isAiming;
    private Vector3 movementDir, aimDir;
    public Material[] lightMat = new Material[2];
    [Range(0.0f,10.0f)]public float lightRadius;
    [Range(0.0f, 10.0f)]public float lightSmothness;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {

        if (GameManager.isVREnable)
        {
            // Store the input axes.
            // changed the inputs so that u can use VR pads
            movementDir = new Vector3(inputL.GetAxis(VRInput).x, 0, inputL.GetAxis(VRInput).y);
            aimDir = new Vector3(inputR.GetAxis(VRInput).x, 0, inputR.GetAxis(VRInput).y);


            AnimationCTRL();
            UpdatePlayerLight();
        }
        else
        {
            // Joystick Input
            movementDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            aimDir = new Vector3(Input.GetAxisRaw("AimX"), 0, Input.GetAxisRaw("AimY"));

            AnimationCTRL();
            UpdatePlayerLight();
        }

        // Move the player around the scene.
        if (movementDir != Vector3.zero)
        {
            Move(movementDir);
        }
        if (aimDir != Vector3.zero)
        {
            isAiming = true;
            Aim(aimDir);
        }
        else
        {
            isAiming = false;
        }
    }

    public void Move(Vector3 dir)
    {
        // I removed the normalization, so that Analog input will have a better groove
        // Normalise the movement vector and make it proportional to the speed per second.
        dir *= speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        rb.MovePosition(transform.position + dir);
    }

    public void Aim(Vector3 dir)
    {
        Vector3 playerToJoystick = (transform.position + dir) - transform.position;

        playerToJoystick.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the joystick.
        Quaternion newRotatation = Quaternion.LookRotation(playerToJoystick);
        // Set the player's rotation to this new rotation.
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, newRotatation, Time.deltaTime * aimSensibility));
    }

    public void AnimationCTRL()
    {
        //Player ruotato verso avanti
        if (transform.rotation.eulerAngles.y >= 315 || transform.rotation.eulerAngles.y <= 45)
        {
            anim.SetFloat("VelZ", movementDir.z);
            anim.SetFloat("VelX", movementDir.x);
        }

        //Player rivolto a destra
        if (transform.rotation.eulerAngles.y > 45 && transform.rotation.eulerAngles.y < 135)
        {
            anim.SetFloat("VelZ", movementDir.x);
            anim.SetFloat("VelX", -movementDir.z);
        }

        //player rivolto in basso
        if (transform.rotation.eulerAngles.y >= 135 && transform.rotation.eulerAngles.y <= 225)
        {
            anim.SetFloat("VelZ", -movementDir.z);
            anim.SetFloat("VelX", -movementDir.x);
        }

        //player rivolto a sinistra
        if (transform.rotation.eulerAngles.y > 225 && transform.rotation.eulerAngles.y < 315)
        {
            anim.SetFloat("VelZ", -movementDir.x);
            anim.SetFloat("VelX", movementDir.z);
        }
    }

    public void UpdatePlayerLight()
    {
        for (int i = 0; i < lightMat.Length; i++)
        {
            lightMat[i].SetVector("PlayerMask_Position", transform.position);
            lightMat[i].SetFloat("PlayerMask_Radius", lightRadius);
            lightMat[i].SetFloat("PlayerMask_Softness", lightSmothness);
        }
    }
}
