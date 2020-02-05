using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    bool TVStatus;
    bool consoleStatus;

    public void OnButtonPressed(string buttonType)
    {
        switch (buttonType)
        {
            case "PowerTV":
                //print("test");
                TVCanvas.SetTrigger("TurnOn");
                TVStatus = !TVStatus;
                TVCanvas.SetTrigger("ChangeSource");
                StartCoroutine("ChangeChannel");
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
                StartCoroutine("ChangeChannel");
                break;
            case "ProgramDOWN":
                if (currentChannel == -1 || !TVStatus)
                {
                    return;
                }
                print("lowering ch " + currentChannel);
                currentChannel -= 1;
                TVCanvas.SetTrigger("ChangeSource");
                StartCoroutine("ChangeChannel");
                //print("logic to switch renderTexture backward");
                break;
            case "PowerConsole":

                consoleStatus = !consoleStatus;
                break;
        }
    }

    IEnumerator ChangeChannel()
    {
        print("changing current channel: " + currentChannel);
        yield return new WaitForSeconds(0.4f);

        if (channels.Any(f => f.chNumber == currentChannel))
        {
            foreach (channel ch in channels)
            {
                if (ch.chNumber == currentChannel)
                {
                    if (ch.chImage != null)
                    {
                        print("i found a image, now using it");
                        TVChannel.sprite = ch.chImage;
                        TVChannel.color = Color.white;
                    }
                }
            }

        }
        else
        {
            print("i found NO image");
            TVChannel.sprite = null;
            TVChannel.color = Color.clear;
        }
        yield return new WaitForSeconds(0.3f);

        if (currentChannel == -1)
        {

            channelText.text = "AV";
            StartCoroutine("CheckForGame");
        }

        if (currentChannel >= 0)
        {
            StopCoroutine("CheckForGame");
            channelText.text = "P " + currentChannel.ToString();
        }
    }
    IEnumerator CheckForGame()
    {
        print("im checking");

        if (!console.isFilled || !consoleStatus)
        {
            print("no game in console");
            TVSource.color = Color.clear;
            TVSource.texture = null;
            yield return new WaitForSeconds(1);
            StartCoroutine("CheckForGame");
        }

        if (console.isFilled && consoleStatus)
        {
            print("found something in console");
            foreach (cassette tape in cassettes)
            {
                //print("looking for texture " + tape.instance);
                if (tape.instance == console.possibleTarget)
                {
                    print("setting texture " + console.possibleTarget.name);
                    TVSource.texture = tape.displayedContent;
                }
            }
            TVSource.color = Color.white;
            TVChannel.color = Color.clear;
            yield return new WaitForSeconds(1);
            StartCoroutine("CheckForGame");
        }


    }
}
