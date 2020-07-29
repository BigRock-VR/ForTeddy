using UnityEngine;

public class Music_manager : MonoBehaviour
{
    public GameObject Shop;
    public GameObject MainMenu;
    [SerializeField] public PlayerManager m_PlayerManager;
    //FMOD.Studio.EventInstance MusicShop;
    FMOD.Studio.EventInstance MusicGameplay;
    // Start is called before the first frame update
    void Start()
    {
        //MusicShop = FMODUnity.RuntimeManager.CreateInstance("event:/Music ARCADE/Music Shop");
        MusicGameplay = FMODUnity.RuntimeManager.CreateInstance("event:/Music ARCADE/Music Gameplay");
        MusicGameplay.start();
    }

    private void OnEnable()
    {
        m_PlayerManager.onPlayerDeath += onPlayerDeath;
    }
    private void OnDisable()
    {
        m_PlayerManager.onPlayerDeath -= onPlayerDeath;
    }
    private void onPlayerDeath()
    {
        Debug.Log("Call Death Sound effect");
        MusicGameplay.setParameterByName("MainMenu", 1);
        MusicGameplay.setParameterByName("GameStatus", 2);
    }

    void Update()
    {
        if (MainMenu.activeSelf)
        {
            MusicGameplay.setParameterByName("MainMenu", 1);
            MusicGameplay.setParameterByName("GameStatus", 1);
           

        }

        if (Shop.activeSelf)
        {
            MusicGameplay.setParameterByName("MainMenu", 0);
            MusicGameplay.setParameterByName("GameStatus", 1);
        }
        
        if (!MainMenu.activeSelf && !Shop.activeSelf)
        {
            if(m_PlayerManager != null)
            {
                if (m_PlayerManager.isDead) { return; }
            }
            MusicGameplay.setParameterByName("MainMenu", 1);
            MusicGameplay.setParameterByName("GameStatus", 0);
        }
    }
}
