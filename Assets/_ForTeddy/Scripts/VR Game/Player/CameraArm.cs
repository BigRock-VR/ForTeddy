using UnityEngine;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

// used to turn around main axis VR Camera

public class CameraArm : MonoBehaviour
{
    Player player;

    public void doCameraRotation(float rotation, float modifier)
    {
        if (modifier == 0)
        { return; }

        //to use vrCam position & it's offset from its parent
        player = Player.instance;

        rotation *= modifier;

        //
        Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
        player.trackingOriginTransform.position -= playerFeetOffset;
        player.transform.Rotate(Vector3.up, rotation);
        playerFeetOffset = Quaternion.Euler(0.0f, rotation, 0.0f) * playerFeetOffset;
        player.trackingOriginTransform.position += playerFeetOffset;

    }

}
