using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    [Header("Player Reference:")]
    public PlayerManager p_Manager;
    public WeaponSystem p_WeaponSystem;
    public WaveManager waveManager;

    public GameObject playerUI;

    [Header("UI Elements:")]
    public Sprite[] weaponsBackground = new Sprite[MAX_WEAPONS];
    public Image weaponImg,playerArmorBar, playerHPBar, weaponAmmoBar;
    public Text t_timerMinutes,t_timerSeconds, t_playerScore;
    private const int MAX_WEAPONS = 5;
    private bool isOpen;
    private float minutes, seconds;

    void Start()
    {
        Init();
        waveManager.onStartGame += Open;
        waveManager.onEndWave += Close;
        Close();
    }

    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        UpdateTimer();
        UpdatePlayerStats();
    }

    private void Init()
    {
        UpdateTimer();
        UpdatePlayerStats();
    }

    private void UpdateTimer()
    {
        t_timerMinutes.text = GetMinutes(waveManager.waveEndTimer);
        t_timerSeconds.text = GetSeconds(waveManager.waveEndTimer);
    }

    private void Close()
    {
        isOpen = false;
        playerUI.SetActive(false);
    }

    private void Open()
    {
        isOpen = true;
        Init();
        playerUI.SetActive(true);
    }

    private void UpdatePlayerStats()
    {
        playerArmorBar.fillAmount = Map01(p_Manager.armor, 0, p_Manager.PLAYER_MAX_ARMOR);
        playerHPBar.fillAmount = Map01(p_Manager.hp, 0, p_Manager.PLAYER_MAX_HP);
        t_playerScore.text = p_Manager.score.ToString();
        // UPDATE WEAPON SPRITE AND AMMO
        weaponImg.sprite = weaponsBackground[p_WeaponSystem.GetCurrSelectedWeapon()];
        weaponAmmoBar.fillAmount = Map01(p_WeaponSystem.currAmmo, 0, p_WeaponSystem.weapons[p_WeaponSystem.GetCurrSelectedWeapon()].maxAmmoCount);
    }

    private string GetMinutes(float timer)
    {
        minutes = (timer / 60);
        return String.Format("{0:0}", Mathf.FloorToInt(minutes));
    }
    private string GetSeconds(float timer)
    {
        seconds = (timer % 60);
        
        if (seconds <= 0)
        {
            return "0";
        }

        return String.Format("{00:00}", seconds);
    }
    private float Map01(float input, float inputMin, float inputMax)
    {
        return 0 + (input - inputMin) * (1 - 0) / (inputMax - inputMin);
    }
}
