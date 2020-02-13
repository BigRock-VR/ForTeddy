using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopManager : MonoBehaviour
{
    [SerializeField] public GameObject shopMenu;
    public Transform[] soldiers = new Transform[MAX_SOLDIERS]; //SOLDIERS IN GAME
    public Transform player;
    [SerializeField] public GameObject ammoBarCateogry; // Ammo slider 
    public Weapon[] weapons = new Weapon[MAX_WEAPONS];

    [Header("Button:")]
    [SerializeField] public Button[] tabButtons = new Button[MAX_TAB_BUTTONS]; //  TOP MENU SHOP BUTTONS
    [SerializeField] public Button[] categoryButtons = new Button[MAX_CATEGORY_BUTTONS]; // HP - WEAPON - ARMOR - PLAY GAME
    [SerializeField] public GameObject leftArrowBtn, rightArrowBtn;


    [Header("Image:")]
    [SerializeField] public Image currBannerImage;
    [SerializeField] public Image[] soldierIMG = new Image[MAX_SOLDIERS]; // SOLDIER IMG
    [SerializeField] public Sprite[] bannerImages = new Sprite[MAX_TAB_BUTTONS];
    [SerializeField] public Sprite[] soldierImgBlocked = new Sprite[MAX_SOLDIERS];
    [SerializeField] public Image armorBar,hpBar,ammoBar, weaponImg,infinityAmmoImg, productImg;
    [SerializeField] public Sprite hpImg, armorImg;

    [Header("Text:")]
    [SerializeField] public Text t_Score;
    [SerializeField] public Text t_Cost;
    [SerializeField] public Text t_Desc;

    public enum eCategory { HP, WEAPON, ARMOR, PLAYGAME }
    public eCategory categoryType = eCategory.HP;

    public int hpCost = 100;
    public int armorCost = 200;
    public int soldierCost = 500;

    public int currSelectedTab = 0;
    public int currSelectedCategory = 0;
    private enum eWeapons { DEFAULT, DAKKAGUN, IMPALLINATOR, ATOMIZER, REKTIFIER };
    private eWeapons currWeaponCategory = eWeapons.DEFAULT;

    public bool isPlayer;
    private const int MAX_TAB_BUTTONS = 5;
    private const int MAX_CATEGORY_BUTTONS = 4;
    private const int MAX_SOLDIERS = 4;
    private const int MAX_WEAPONS = 5;
    private Color clr_SoldierDisable = new Color(1, 1, 1, 0.5f);
    private PlayerManager p_PlayerManager;
    private WeaponSystem p_WeaponSystem;
    private SoldierManager[] s_SoldierManager = new SoldierManager[MAX_SOLDIERS];
    private WeaponSystem[] s_WeaponSystem = new WeaponSystem[MAX_SOLDIERS];
    private string hpDesc = "Refill Health to 100%";
    private string armorDesc = "Refill Armor to 100%";


    public void Start()
    {
        p_PlayerManager = player.GetComponent<PlayerManager>();
        p_WeaponSystem = player.GetComponent<WeaponSystem>();

        for (int i = 0; i < soldiers.Length; i++)
        {
            s_SoldierManager[i] = soldiers[i].GetComponent<SoldierManager>();
            s_WeaponSystem[i] = soldiers[i].GetComponent<WeaponSystem>();
        }
        InitShop();
        //Close();
    }

    private void LateUpdate()
    {
        CheckTabButton();
        CheckCategoryButton();
        UpdateProduct();
        if (isPlayer)
        {
            UpdatePlayerStats();
        }
        else
        {
            UpdateSoldierStats();
        }
    }

    public void CheckTabButton()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (tabButtons[i].GetComponent<ButtonInteract>().isSelected)
            {
                currSelectedTab = i;
                isPlayer = i == 0;
                ChangeBannerImage();
                break;
            }
        }
    }

    public void CheckCategoryButton()
    {
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            if (categoryButtons[i].GetComponent<ButtonInteract>().isSelected)
            {
                currSelectedCategory = i;
                categoryType = (eCategory)i;
                break;
            }
        }
    }

    public void InitShop()
    {
        
        if (GetAllSoldiersAlive() == 0)
        {
            DisableAllSoldier();
        }
        else
        {
            EnableSoldierImage();
        }

        InitSelection();
    }
    private void InitSelection()
    {
        tabButtons[0].Select();
    }

    public void ChangeBannerImage()
    {
        if (currSelectedTab > 0 && !soldiers[currSelectedTab-1].gameObject.activeInHierarchy)
        {
            currBannerImage.sprite = soldierImgBlocked[currSelectedTab-1];
        }
        else
        {
            currBannerImage.sprite = bannerImages[currSelectedTab];
        }

    }

    public void Close()
    {
        shopMenu.SetActive(false);
    }

    public void Open()
    {
        shopMenu.SetActive(true);
    }

    private void UpdatePlayerStats()
    {
        t_Score.text = p_PlayerManager?.score.ToString();
        armorBar.fillAmount = Map(p_PlayerManager.armor, 0, p_PlayerManager.PLAYER_MAX_ARMOR, 0, 1);
        hpBar.fillAmount = Map(p_PlayerManager.hp, 0, p_PlayerManager.PLAYER_MAX_HP, 0, 1);
        Weapon currWeapon = p_WeaponSystem.weapons[p_WeaponSystem.GetCurrSelectedWeapon()];
        weaponImg.sprite = currWeapon.weaponImage;
        if (currWeapon.isDefaultWeapon)
        {
            ammoBarCateogry.SetActive(false);
            infinityAmmoImg.enabled = true;
        }
        else
        {
            ammoBarCateogry.SetActive(true);
            ammoBar.fillAmount = Map(p_WeaponSystem.currAmmo, 0, currWeapon.maxAmmoCount, 0, 1);
            infinityAmmoImg.enabled = false;
        }
    }

    private void UpdateSoldierStats()
    {
        currSelectedTab = Mathf.Clamp(currSelectedTab - 1, 0, MAX_SOLDIERS);
        t_Score.text = p_PlayerManager?.score.ToString();
        armorBar.fillAmount = Map(s_SoldierManager[currSelectedTab].armor, 0, s_SoldierManager[currSelectedTab].SOLDIER_MAX_ARMOR, 0, 1);
        hpBar.fillAmount = Map(s_SoldierManager[currSelectedTab].hp, 0, s_SoldierManager[currSelectedTab].SOLDIER_MAX_HP, 0, 1);
        weaponImg.sprite = s_WeaponSystem[currSelectedTab].weapons[s_WeaponSystem[currSelectedTab].GetCurrSelectedWeapon()].weaponImage;
        Weapon currWeapon = s_WeaponSystem[currSelectedTab].weapons[s_WeaponSystem[currSelectedTab].GetCurrSelectedWeapon()];
        weaponImg.sprite = currWeapon.weaponImage;
        if (currWeapon.isDefaultWeapon)
        {
            ammoBarCateogry.SetActive(false);
            infinityAmmoImg.enabled = true;
        }
        else
        {
            ammoBarCateogry.SetActive(true);
            ammoBar.fillAmount = Map(s_WeaponSystem[currSelectedTab].currAmmo, 0, currWeapon.maxAmmoCount, 0, 1);
            infinityAmmoImg.enabled = false;
        }
    }

    private void UpdateProduct()
    {
        switch (categoryType)
        {
            case eCategory.HP:
                productImg.sprite = hpImg;
                t_Cost.text = hpCost.ToString();
                t_Desc.text = hpDesc;
                leftArrowBtn.SetActive(false);
                rightArrowBtn.SetActive(false);
                break;
            case eCategory.WEAPON:
                productImg.sprite = weapons[(int)currWeaponCategory].weaponImage;
                t_Cost.text = weapons[(int)currWeaponCategory].cost.ToString();
                t_Desc.text = weapons[(int)currWeaponCategory].name;
                leftArrowBtn.SetActive(true);
                rightArrowBtn.SetActive(true);
                break;
            case eCategory.ARMOR:
                productImg.sprite = armorImg;
                t_Cost.text = armorCost.ToString();
                t_Desc.text = armorDesc;
                leftArrowBtn.SetActive(false);
                rightArrowBtn.SetActive(false);
                break;
            case eCategory.PLAYGAME:
                break;
            default:
                break;
        }
    }

    public void SwitchLeft()
    {
        float currWeapon = (float)--currWeaponCategory;
        if (currWeapon < 0 )
        {
            currWeaponCategory = (eWeapons)MAX_WEAPONS-1;
        }
        else
        {
            currWeaponCategory = (eWeapons)currWeapon;
        }
    }
    public void SwitchRight()
    {
        float currWeapon = (float)++currWeaponCategory;
        if (currWeapon > MAX_WEAPONS-1)
        {
            currWeaponCategory = eWeapons.DEFAULT;
        }
        else
        {
            currWeaponCategory = (eWeapons)currWeapon;
        }
    }

    public void PlayNextWave()
    {
        GameManager.Instance.waveManager.GetComponent<WaveManager>().StartGame();
        Close();
    }

    private void DisableAllSoldier()
    {
        for (int i = 0; i < soldierIMG.Length; i++)
        {
            soldierIMG[i].color = clr_SoldierDisable;
        }
    }

    public int GetAllSoldiersAlive()
    {
        int result = 0;

        for (int i = 0; i < soldiers.Length; i++)
        {
            if (soldiers[i].gameObject.activeInHierarchy)
            {
                result++;
            }
        }

        return result;
    }

    public void EnableSoldierImage()
    {
        for (int i = 0; i < MAX_SOLDIERS; i++)
        {
            if (soldiers[i].gameObject.activeInHierarchy)
            {
                soldierIMG[i].color = Color.white;
            }
        }
    }

    public void Buy()
    {
        switch (categoryType)
        {
            case eCategory.HP:
                if (!hasEnoughtCoins(p_PlayerManager.score, hpCost))
                {
                    return;
                }
                if (isPlayer)
                {
                    p_PlayerManager.score -= hpCost;
                    p_PlayerManager.ReloadPlayerHP();
                }
                else
                {
                    p_PlayerManager.score -= hpCost;
                    s_SoldierManager[currSelectedTab].ReloadSoldierHP();
                }
                break;
            case eCategory.WEAPON:
                /*TO DO WEAPON SWITCH */
                break;
            case eCategory.ARMOR:
                if (!hasEnoughtCoins(p_PlayerManager.score, armorCost))
                {
                    return;
                }
                if (isPlayer)
                {
                    p_PlayerManager.score -= hpCost;
                    p_PlayerManager.ReloadPlayerArmor();
                }
                else
                {
                    p_PlayerManager.score -= armorCost;
                    s_SoldierManager[currSelectedTab].ReloadSoldierArmor();
                }
                break;
            case eCategory.PLAYGAME:
                break;
            default:
                break;
        }
    }

    public bool hasEnoughtCoins(int playerScore, int cost)
    {
        return playerScore >= cost;
    }
    private float Map(float input, float inputMin, float inputMax, float min, float max)
    {
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }
}
