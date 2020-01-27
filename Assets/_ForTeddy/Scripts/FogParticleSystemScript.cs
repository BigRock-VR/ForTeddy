using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FogParticleSystemScript : MonoBehaviour
{
    public ParticleSystem all, middle, boss;
    public Color colorAll,colorMiddle,colorBoss;
    void Update()
    {
        ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[all.particleCount];
        int num1 = all.GetParticles(allParticles);

        for (int i = 0; i < num1; i++)
        {
            allParticles[i].startColor = colorAll;
        }

        all.SetParticles(allParticles, num1);

        ParticleSystem.Particle[] middleParticles = new ParticleSystem.Particle[middle.particleCount];
        int num2 = middle.GetParticles(middleParticles);

        for (int i = 0; i < num2; i++)
        {
            middleParticles[i].startColor = colorMiddle;
        }

        middle.SetParticles(middleParticles, num2);

        ParticleSystem.Particle[] bossParticles = new ParticleSystem.Particle[boss.particleCount];
        int num3 = boss.GetParticles(bossParticles);

        for (int i = 0; i < num3; i++)
        {
            bossParticles[i].startColor = colorBoss;
        }

        boss.SetParticles(bossParticles, num3);



    }

    //public void ChangingColor()
    //{
        
    //    ParticleSystem.MainModule allMain = all.main;
    //    ParticleSystem.MainModule middleMain = middle.main;
    //    ParticleSystem.MainModule bossMain = boss.main;
    //    allMain.startColor = startColor;
    //    middleMain.startColor = startColor;
    //    bossMain.startColor = startColor;
    //}




}
