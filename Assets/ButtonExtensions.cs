using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtensions : MonoBehaviour
{
    [Serializable]
    struct cassette
    {
        public Rigidbody instance;
        public RenderTexture displayedContent;
    }

    [SerializeField]
    List<cassette> cassettes;

    [SerializeField]
    RawImage TVSource;

    [SerializeField]
    Image TVChannel;

    [SerializeField]
    SnapTargetToObject console;

    [SerializeField]
    Animator TVCanvas;

    [Serializable]
    struct channel
    {
        public int chNumber;
        public Sprite chImage;
    }
    [SerializeField]
    List<channel> channels;

    [SerializeField]
    int currentChannel;

    [SerializeField]
    TMPro.TextMeshProUGUI channelText;

    [SerializeField]
    GameObject blackScreen;

    bool TVStatus;
    Sprite x;

    public void OnButtonPressed(string buttonType)
    {
        x = TVChannel.sprite;

        switch (buttonType)
        {
            case "PowerTV":
                //print("test");
                TVCanvas.SetTrigger("TurnOn");
                TVStatus = true;
                
                break;
            case "VolumeUP":
                print("logic to raise tv audio");
                break;
            case "VolumeDOWN":
                print("logic to lower tv audio");
                break;
            case "ProgramUP":
                if (currentChannel == 99 || !TVStatus)
                {
                    return;
                }
                //print("logic to switch renderTexture forward");
                print("rising ch " + currentChannel);
                currentChannel += 1;
                TVCanvas.SetTrigger("ChangeSource");
                Invoke("CheckChannels", 1.3f);
                Invoke("ChangeChannel", 2);
                break;
            case "ProgramDOWN":
                if (currentChannel == -1 || !TVStatus)
                {
                    return;
                }
                print("lowering ch " + currentChannel);
                currentChannel -= 1;
                TVCanvas.SetTrigger("ChangeSource");
                Invoke("CheckChannels", 0.3f);
                Invoke("ChangeChannel", 2);
                //print("logic to switch renderTexture backward");
                break;
            case "PowerConsole":
                if(console.possibleTarget != null)
                {
                    foreach (cassette tape in cassettes)
                    {
                        //print("looking for texture " + tape.instance);
                        if (tape.instance == console.possibleTarget)
                        {
                            //print("setting texture " + console.possibleTarget.name);
                            TVSource.texture = tape.displayedContent;
                        }
                    }
                }
                break;
        }
    }

    void CheckChannel()
    {
        print(currentChannel);
        foreach (channel ch in channels)
        {
            if (ch.chNumber == currentChannel)
            {
                TVChannel.sprite = ch.chImage;
                TVChannel.color = Color.white;
            }
        }
        if (x == TVChannel.sprite)
        {
            TVChannel.color = Color.clear;
        }
    }

    void ChangeChannel()
    {
        channelText.enabled = true;
        channelText.text = "P " + currentChannel.ToString();

        if (currentChannel == -1)
        {
            if (console.possibleTarget == null)
            {
                TVSource.color = Color.black;
            }
            else
            {
                TVSource.color = Color.white;
            }

            TVChannel.enabled = false;
            TVSource.enabled = true;
        }
        if (currentChannel == 0)
        {
            TVChannel.enabled = true;
            TVSource.enabled = false;
        }

        Invoke("RemoveCH", 2);
    }

    void RemoveCH()
    {
        channelText.enabled = false;
    }
}
