using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //[Device]
    [Header("Device Settings")]
    public static bool isVREnable;
    public static string VRDeviceName;

    //[Player]
    public GameObject player; // Reference of the player GameObject

    //[Game]
    public WaveManager waveManager;

    //[Scene]
    private enum SceneIndex { MAIN, VR, ARCADE };
    private static SceneIndex currentScene;
    //[UI]
    // TO DO


    // Instantiate the GameManger right after the scene is loaded
    [RuntimeInitializeOnLoadMethod]
    private static void InitGameManager()
    {
        Init();

        currentScene = GetCurrentSceneIndex();

        isVREnable = XRSettings.isDeviceActive;
        VRDeviceName = XRSettings.loadedDeviceName;

        // Debug Mode: if the scene is not the main menu will be loaded the current opened scene
        if (currentScene == SceneIndex.MAIN)
        {
            if (isVREnable)
            {
                // Load the VR Scene and the Arcade Scene as additive for the projecting the arcade game on TV.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Single);
            }
        }
    }

    // This Method is called when the scene change
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Delegate a custom method when the scene change
    }

    // Delegate that try to find the player GameObject when new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private static SceneIndex GetCurrentSceneIndex()
    {
        return (SceneIndex)SceneManager.GetActiveScene().buildIndex;
    }

    private void Awake()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>();
        }
    }
}
