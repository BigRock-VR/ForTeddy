using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/SavedGame" + GameData.current.nickname + ".dat"))
        {

            FileStream file = File.Open(Application.persistentDataPath + "/SavedGame" + GameData.current.nickname + ".dat", FileMode.Open);
            bf.Serialize(file, GameData.current);
            file.Close();
        }
        else
        {

            FileStream file = File.Create(Application.persistentDataPath + "/SavedGame" + GameData.current.nickname + ".dat" + GameData.current.nickname);
            bf.Serialize(file, GameData.current);
        }


    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame" + GameData.current.nickname + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SavedGame" + GameData.current.nickname + ".dat", FileMode.Open);
            GameData game = (GameData)bf.Deserialize(file);
            file.Close();
            GameData.current.nickname = game.nickname;
            GameData.current.score = game.score;
            GameData.current.hasSoldier = game.hasSoldier;
            GameData.current.hasEnemy = game.hasEnemy;
            GameData.current.hasPeashooter = game.hasPeashooter;
            GameData.current.hasDakkaGun = game.hasDakkaGun;
            GameData.current.hasImpallinator = game.hasImpallinator;
            GameData.current.hasAtomizer = game.hasAtomizer;
            GameData.current.hasRektifier = game.hasRektifier;
            GameData.current.hasTeddy = game.hasTeddy;
            GameData.current.hasAlan = game.hasAlan;
            GameData.current.hasTorch = game.hasTorch;
            GameData.current.hasHand = game.hasHand;
            GameData.current.hasFinishedGame = game.hasFinishedGame;
            GameData.current.hasArts = game.hasArts;
            GameData.current.hasMovie = game.hasMovie;


            Debug.Log("Loaded The File");
        }
        else
        {
            Debug.Log("File Does Not Exist");
        }
    }
}