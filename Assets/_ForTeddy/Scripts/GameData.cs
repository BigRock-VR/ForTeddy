using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static GameData current = new GameData();

    // base items
    public string nickname;
    public int score; // is total earned money - it is used just in vr to display vr experience score
    public bool[] hasSoldier = new bool[3];
    public bool[] hasEnemy = new bool[2];
    public bool hasPeashooter;
    public bool hasDakkaGun;
    public bool hasImpallinator;
    public bool hasAtomizer;
    public bool hasRektifier;
    public bool hasTeddy;
    public bool hasAlan;

    // extras
    public bool hasTorch;
    public bool hasHand;
    public bool hasFinishedGame;
    public bool[] hasArts = new bool[999]; // to define
    public bool[] hasMovie = new bool[999]; // to define
}