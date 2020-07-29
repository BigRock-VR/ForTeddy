using System.Collections;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;    // The speed that the player will move at.
    [SerializeField][Range(0.1f, 1.0f)]public float aimSensibility = 0.2f;   // The rotation sensibility of the joypad.

    [SerializeField]
    SteamVR_Action_Vector2 inputL, inputR;

    [SerializeField]
    SteamVR_Input_Sources VRInput;

    private Animator anim;

    private Rigidbody rb;
    public bool isAiming;
    //public Material[] lightMat = new Material[2];
    //[Range(0.0f,10.0f)]public float lightRadius;
    //[Range(0.0f, 10.0f)]public float lightSmothness;

    private float rotateTimer;
    private Vector3 movementDir, aimDir;
    private float joyPadThreShold = 0.1f;
    private float joyPadThreSholdN = -0.1f;
    private PlayerManager p_Manager;
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        p_Manager = GetComponent<PlayerManager>();
    }


    private void Update()
    {
        if (p_Manager.isDead)
        {
            isAiming = false;
            return;
        }

        if (GameManager.isVREnable)
        {
            // Store the input axes.
            // changed the inputs so that u can use VR pads
            movementDir.x = inputL.GetAxis(VRInput).x;
            movementDir.z = inputL.GetAxis(VRInput).y;
            aimDir.x = inputR.GetAxis(VRInput).x;
            aimDir.z = inputR.GetAxis(VRInput).y;
            //movementDir = new Vector3(inputL.GetAxis(VRInput).x, 0, inputL.GetAxis(VRInput).y);
            //aimDir = new Vector3(inputR.GetAxis(VRInput).x, 0, inputR.GetAxis(VRInput).y);


            AnimationCTRL();
            //UpdatePlayerLight();
        }
        else
        {
            // Joystick Input
            movementDir.x = Input.GetAxisRaw("Horizontal");
            movementDir.z = Input.GetAxisRaw("Vertical");
            aimDir.x = Input.GetAxisRaw("AimX");
            aimDir.z = Input.GetAxisRaw("AimY");

            AnimationCTRL();
            //UpdatePlayerLight();
        }
    }

    void FixedUpdate()
    {
        // Move the player around the scene.
        if (movementDir != Vector3.zero)
        {
            Move(movementDir);
        }
        if (aimDir.x >= joyPadThreShold ||
            aimDir.z >= joyPadThreShold ||
            aimDir.x <= joyPadThreSholdN ||
            aimDir.z <= joyPadThreSholdN)
        {
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
        //Vector3 playerToJoystick = (transform.position + dir) - transform.position;
        //playerToJoystick.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the joystick.
        Quaternion newRotatation = Quaternion.LookRotation(dir);
        // Set the player's rotation to this new rotation.

        rotateTimer += Time.deltaTime / aimSensibility;
        rb.MoveRotation(Quaternion.Lerp(transform.rotation, newRotatation, rotateTimer));

        if (rotateTimer > 1)
        {
            rotateTimer = 0;
            isAiming = true;
        }
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


    //public void UpdatePlayerLight()
    //{
    //    for (int i = 0; i < lightMat.Length; i++)
    //    {
    //        lightMat[i].SetVector("PlayerMask_Position", transform.position);
    //        lightMat[i].SetFloat("PlayerMask_Radius", lightRadius);
    //        lightMat[i].SetFloat("PlayerMask_Softness", lightSmothness);
    //    }
    //}

}
