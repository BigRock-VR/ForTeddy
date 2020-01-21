using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters")]

public class Characters : ScriptableObject
{
    public string chrName;
    public Weapons baseWeapon;
    public Weapons altWeapon;

    public int health;
    public bool armor;

    public Sprite ChrImg;
}
