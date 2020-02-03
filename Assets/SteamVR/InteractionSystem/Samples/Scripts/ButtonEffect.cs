//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ButtonEffect : MonoBehaviour
    {
        //Edit to main
        [SerializeField]
        GameObject renderTexture;

        [SerializeField]
        Animator TVCanvas;

        [SerializeField]
        List<Color> tempColor;

        public void OnButtonDown(Hand fromHand)
        {
            ColorSelf(Color.white);
            fromHand.TriggerHapticPulse(1000);
        }

        public void OnButtonUp(Hand fromHand)
        {
            ColorSelf(Color.clear);
        }

        private void ColorSelf(Color newColor)
        {
            Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
            //print(renderers.Length);
            for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
            {

                if (newColor == Color.clear)
                {
                    //print("test");
                    renderers[rendererIndex].material.color = tempColor[rendererIndex];

                }
                else
                {
                    //print("sto colorando il tasto");
                    tempColor.Add(renderers[rendererIndex].material.color);
                    renderers[rendererIndex].material.color = newColor;
                }
            }
        }

        // edit to main
        public void OnButtonPressed(string buttonType)
        {
            switch (buttonType)
            {
                case "PowerTV":
                    //print("test");
                    var status = !TVCanvas.GetBool("isPowered");
                    TVCanvas.SetBool("isPowered", status);
                    break;
                case "VolumeUP":
                    print("logic to raise tv audio");
                    break;
                case "VolumeDOWN":
                    print("logic to lower tv audio");
                    break;
                case "ProgramUP":
                    print("logic to switch renderTexture forward");
                    break;
                case "ProgramDOWN":
                    print("logic to switch renderTexture backward");
                    break;
                case "PowerConsole":
                    renderTexture.SetActive(!renderTexture.activeSelf);
                    break;
            }
        }
    }
}