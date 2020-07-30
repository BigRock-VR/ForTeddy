using System.Collections;
using UnityEngine;
public class FMODBGPlayer : MonoBehaviour
{
    private static FMOD.Studio.EventInstance MusicGameplay;

    private void Start()
    {
        MusicGameplay = FMODUnity.RuntimeManager.CreateInstance("event:/Music ARCADE/Music Gameplay");
        MusicGameplay.start();
    }

    public static void SetDeathMusic()
    {
        MusicGameplay.setParameterByName("MainMenu", 1);
        MusicGameplay.setParameterByName("GameStatus", 2);
    }

    public static void SetGamePlayMusic()
    {
        MusicGameplay.setParameterByName("MainMenu", 0);
        MusicGameplay.setParameterByName("GameStatus", 0);
    }

    public static void SetVolume(float volume)
    {
        MusicGameplay.setVolume(volume);
    }
}
