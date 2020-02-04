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
    }
}