using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    FMOD.Studio.EventInstance Volume;
    FMOD.Studio.EventInstance ThunderType;
    FMOD.Studio.EventInstance SqueakType;
    FMOD.Studio.EventInstance FlickerType;

    FMOD.Studio.EventDescription VolumeDescription;
    FMOD.Studio.EventDescription ThunderTypeDescription;
    FMOD.Studio.EventDescription SqueakTypeDescription;
    FMOD.Studio.EventDescription FlickerTypeDescription;
    FMOD.Studio.PARAMETER_DESCRIPTION pd;
    FMOD.Studio.PARAMETER_ID pID_V, pID_T, pID_S, pID_F;

    [SerializeField]
    GameObject thunderstormLoopOrigin;

    // Start is called before the first frame update
    void Start()
    {
        //starts background noise
        //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/VR_Thunderstorm", thunderstormLoopOrigin);


        //get thunders ready to be controlled
        ThunderType = FMODUnity.RuntimeManager.CreateInstance("event:/VR_Thunderstorm");
        ThunderType.start();

        //get squeaks sounds ready to be controlled
        SqueakType = FMODUnity.RuntimeManager.CreateInstance("event:/VR_Squeak");
        SqueakType.start();

        //get flickering sounds ready to be controlled
        FlickerType = FMODUnity.RuntimeManager.CreateInstance("event:/VR_Flicker");
        FlickerType.start();

        // get creepy sounds ready to be controlled
        Volume = FMODUnity.RuntimeManager.CreateInstance("event:/VR_Environment");
        Volume.start();


        //gets param from thunders tracks
        ThunderTypeDescription = FMODUnity.RuntimeManager.GetEventDescription("event:/VR_Thunderstorm");
        ThunderTypeDescription.getParameterDescriptionByName("ThunderType", out pd);
        pID_T = pd.id;

        // gets param from squeaking tracks
        SqueakTypeDescription = FMODUnity.RuntimeManager.GetEventDescription("event:/VR_Squeak");
        SqueakTypeDescription.getParameterDescriptionByName("SqueakType", out pd);
        pID_S = pd.id;

        // gets param from squeaking tracks
        FlickerTypeDescription = FMODUnity.RuntimeManager.GetEventDescription("event:/VR_Flicker");
        FlickerTypeDescription.getParameterDescriptionByName("FlickerType", out pd);
        pID_F = pd.id;

        // gets param from track
        VolumeDescription = FMODUnity.RuntimeManager.GetEventDescription("event:/VR_Environment");
        VolumeDescription.getParameterDescriptionByName("Volume", out pd);
        pID_V = pd.id;

    }

    public void RandomThunder(float vol, Transform thunderOrigin)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ThunderType, thunderOrigin, thunderOrigin.GetComponent<Rigidbody>());
        ThunderType.setVolume(vol);
        ThunderType.setParameterByID(pID_T, Random.Range(1,4));
    }

    public void RandomFlicker(float vol, Transform flickerOrigin)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ThunderType, flickerOrigin, flickerOrigin.GetComponent<Rigidbody>());
        FlickerType.setVolume(vol);
        FlickerType.setParameterByID(pID_F, Random.Range(1, 4));
    }

    public void RandomSqueak(int type, float vol, Transform squeakOrigin)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ThunderType, squeakOrigin, squeakOrigin.GetComponent<Rigidbody>());
        SqueakType.setVolume(vol);
        SqueakType.setParameterByID(pID_S, type);
    }
}
