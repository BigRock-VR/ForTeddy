using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp", menuName = "PowerUp")]

public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public Sprite powerUpImage;

    public int cost;
}
