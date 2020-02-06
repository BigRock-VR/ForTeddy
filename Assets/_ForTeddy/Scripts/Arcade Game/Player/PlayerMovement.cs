using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;    // The speed that the player will move at.
    public float aimSensibility = 200.0f;   // The rotation sensibility of the joypad.

    [SerializeField]
    SteamVR_Action_Vector2 inputL, inputR;

    [SerializeField]
    SteamVR_Input_Sources VRInput;


    private Animator anim;

    private Rigidbody rb;
    public bool isAiming;
    private Vector3 movementDir, aimDir;
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
            anim.SetFloat("VelZ", movementDir.z);
            anim.SetFloat("VelX", movementDir.x);
        }
        else
        {
            // Joystick Input
            movementDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            aimDir = new Vector3(Input.GetAxisRaw("AimX"), 0, Input.GetAxisRaw("AimY"));
            anim.SetFloat("VelZ", movementDir.z);
            anim.SetFloat("VelX", movementDir.x);
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


        // Animate the player.
        //Animating(h, v);
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
        rb.MoveRotation(newRotatation);
    }

    //void Animating(float h, float v)
    //{
    //    // Create a boolean that is true if either of the input axes is non-zero.
    //    bool walking = h != 0f || v != 0f;

    //    // Tell the animator whether or not the player is walking.
    //    anim.SetBool("IsWalking", walking);
    //}
}
