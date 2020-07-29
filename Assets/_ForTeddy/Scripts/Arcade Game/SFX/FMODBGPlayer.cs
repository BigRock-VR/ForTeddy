using UnityEngine;
using FMOD;
using FMODUnity;
public static class FMODBGPlayer
{
    public static FMOD.Studio.EventInstance MusicGameplay;
    public static void Init()
    {
        MusicGameplay = RuntimeManager.CreateInstance("event:/Music ARCADE/Music Gameplay");
        MusicGameplay.start();
    }

    public static void SetDeathMusic()
    {
        MusicGameplay.setParameterByName("MainMenu", 1);
        MusicGameplay.setParameterByName("GameStatus", 2);
    }

    public static void SetMainMenuMusic()
    {
        MusicGameplay.setParameterByName("MainMenu", 1);
        MusicGameplay.setParameterByName("GameStatus", 1);
    }

    public static void SetGamePlayMusic()
    {
        MusicGameplay.setParameterByName("MainMenu", 0);
        MusicGameplay.setParameterByName("GameStatus", 0);
    }
}
