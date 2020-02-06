using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class IdleController : MonoBehaviour
{
    public float velX;
    public float velZ;

    public Animator Anim;
    public float counter;

    // SET THE ANIMATOR COMPONENT ///

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // GET MOVEMENT INFORMATION AND SET ANIMATION PARAMETERS ///

    void Update()
    {
        Anim.SetFloat("VelZ", velZ);
        Anim.SetFloat("VelX", velX);

        if(velZ == 0 && velX == 0)
        {
            Conteggio();
        }
        else
        {
            Anim.SetFloat("IdleSet", 0);
            counter = 0;
        }
    }

    //  IDLE CHANGE BY RANDOMIZER ///

    private void Conteggio()
    {
        counter += Time.deltaTime;
        if(counter >= 10)
        {
            int random = Random.Range(0, 2);
            Anim.SetFloat("IdleSet", random);
            counter = 0;
        }
    }
}
