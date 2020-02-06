using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRepos : MonoBehaviour
{
    [Header("Weapon Positions")]
    public Transform peashooterPos;

    [Header("Weapon Prefabs")]
    public GameObject peashooter;
    //public GameObject dakkaGun;
    //public GameObject atomizer;
    //public GameObject impallinator;
    //public GameObject rektifier;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(peashooter,peashooterPos);
    }
}
