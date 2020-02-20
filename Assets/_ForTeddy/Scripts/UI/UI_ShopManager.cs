using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopManager : MonoBehaviour
{
    [SerializeField] public GameObject shopMenu;
    public bool isOpen;
    public Transform[] soldiers = new Transform[MAX_SOLDIERS]; //SOLDIERS IN GAME
    public Transform player;
    [SerializeField] public GameObject ammoBarCateogry; // Ammo slider 
    public Weapon[] weapons = new Weapon[MAX_WEAPONS]; // Weapon Scriptable Object
    public GameObject dialogMenu;

    [Header("Button:")]
    [SerializeField] public Button[] tabButtons = new Button[MAX_TAB_BUTTONS]; //  TOP-SIDE MENU SHOP BUTTONS
    [SerializeField] public Button[] categoryButtons = new Button[MAX_CATEGORY_BUTTONS]; // MID-SIDE MENU BUTTONS HP - WEAPON - ARMOR - PLAY GAME
    [SerializeField] public Button buyBtn;
    [SerializeField] public GameObject leftArrowBtn, rightArrowBtn, dialogConfirmBtn, dialogCancelBtn, dialogBackBtn, fireRateBtn, damageBtn, ammoCapacityBtn,buyButton;


    [Header("Image:")]
    [SerializeField] public Image currBannerImage;
    [SerializeField] public Image[] soldierIMG = new Image[MAX_SOLDIERS]; // SOLDIER IMG
    [SerializeField] public Sprite[] bannerImages = new Sprite[MAX_TAB_BUTTONS];
    [SerializeField] public Sprite[] soldierImgBlocked = new Sprite[MAX_SOLDIERS];
    [SerializeField] public Sprite[] weaponImgs = new Sprite[MAX_WEAPONS];
    [SerializeField] public Image armorBar, hpBar, ammoBar, fireRateBar, damageBar, ammoCapacityBar, weaponImg,infinityAmmoImg, productImg;
    [SerializeField] public Sprite hpImg, armorImg, soldierUnlocked, soldierLocked;

    [Header("Text:")]
    [SerializeField] public Text t_Score;
    [SerializeField] public Text t_Cost;
    [SerializeField] public Text t_Desc;
    [SerializeField] public Text dialogText;
    [SerializeField] public Text t_nextWaveCount;
    [SerializeField] public GameObject nextWaveText;
    [SerializeField] public GameObject costText;

    public enum eCategory { HP, WEAPON, ARMOR, PLAY, SOLDIER }
    public eCategory categoryType = eCategory.HP;

    public int hpCost = 100;
    public int armorCost = 200;
    public int soldierCost = 500;

    public int currSelectedTab = 0;
    public int currSelectedSoldier = 0;
    public int currSelectedCategory = 0;

    private eWeapons currWeaponCategory = eWeapons.DEFAULT;

    public bool isPlayer;
    private const int PLAYER_BUTTON_INDEX = 0;
    private const int MAX_TAB_BUTTONS = 5;
    private const int MAX_CATEGORY_BUTTONS = 4;
    private const int MAX_SOLDIERS = 4;
    private const int MAX_WEAPONS = 5;
    private PlayerManager p_PlayerManager;
    private WeaponSystem p_WeaponSystem;
    private SoldierManager[] s_SoldierManager = new SoldierManager[MAX_SOLDIERS];
    private WeaponSystem[] s_WeaponSystem = new WeaponSystem[MAX_SOLDIERS];
    private string hpDesc = "Refill Health to 100%";
    private string armorDesc = "Refill Armor to 100%";
    private string dialogWeapon = "Do you really want to buy {0} for {1} ?";
    private string dialogSoldier = "Do you really want to buy Soldier N.{0} for {1} ?";
    private string dialogHp = "Do you really want to refill your Health for {0} ?";
    private string dialogArmor = "Do you really want to refill your Armor for {0} ?";
    private string dialogHpSoldier = "Do you really want to refill Soldier N.{0} Health for {1} ?";
    private string dialogArmorSoldier = "Do you really want to refill Soldier N.{0} Armor for {1} ?";
    private string dialogNoEnoughtCoin = "You don't have enough coins!";
    private string dialogNoCoinSoldier = "You need {0} coin to buy a soldier";
    private string dialogNoSoldier = "You need to buy the Soldier N.{0} first";

    public void Start()
    {
        p_PlayerManager = player.GetComponent<PlayerManager>();
        p_WeaponSystem = player.GetComponent<WeaponSystem>();

        for (int i = 0; i < soldiers.Length; i++)
        {
            s_SoldierManager[i] = soldiers[i].GetComponent<SoldierManager>();
            s_WeaponSystem[i] = soldiers[i].GetComponent<WeaponSystem>();
        }

        GameManager.Instance.waveManager.GetComponent<WaveManager>().onOpenShop += Open;
    }

    private void LateUpdate()
    {
        if (!isOpen)
        {
            return;
        }

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
        isPlayer = currSelectedTab == PLAYER_BUTTON_INDEX;

        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (tabButtons[i].GetComponent<ButtonInteract>().isSelected)
            {
                currSelectedTab = i;
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
        isOpen = true;
        HideDialog();
        shopMenu.SetActive(true);
        if (GetAllSoldiersAlive() == 0)
        {
            DisableAllSoldier();
        }
        else
        {
            EnableSoldierImage();
        }
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;
        InitSelection();
    }

    public void InitSelection()
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
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        isOpen = false;
        currSelectedTab = 0;
        currSelectedCategory = 0;
        categoryType = eCategory.HP;
        InitSelection();
        shopMenu.SetActive(false);
    }

    public void Open()
    {
        Invoke("InitShop", 3.0f);
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
        currSelectedSoldier = Mathf.Clamp(currSelectedTab - 1, 0, MAX_SOLDIERS);
        // Update player score
        t_Score.text = p_PlayerManager?.score.ToString();
        // Update soldier armor and HP bar
        armorBar.fillAmount = Map(s_SoldierManager[currSelectedSoldier].armor, 0, s_SoldierManager[currSelectedSoldier].SOLDIER_MAX_ARMOR, 0, 1);
        hpBar.fillAmount = Map(s_SoldierManager[currSelectedSoldier].hp, 0, s_SoldierManager[currSelectedSoldier].SOLDIER_MAX_HP, 0, 1);
        weaponImg.sprite = s_WeaponSystem[currSelectedSoldier].weapons[s_WeaponSystem[currSelectedSoldier].GetCurrSelectedWeapon()].weaponImage;
        Weapon currWeapon = s_WeaponSystem[currSelectedSoldier].weapons[s_WeaponSystem[currSelectedSoldier].GetCurrSelectedWeapon()];
        weaponImg.sprite = currWeapon.weaponImage;

        if (currWeapon.isDefaultWeapon)
        {
            ammoBarCateogry.SetActive(false);
            infinityAmmoImg.enabled = true;
        }
        else
        {
            ammoBarCateogry.SetActive(true);
            ammoBar.fillAmount = Map(s_WeaponSystem[currSelectedSoldier].currAmmo, 0, currWeapon.maxAmmoCount, 0, 1);
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
                EnableProductInfo();
                DisableWeaponInfo();
                break;
            case eCategory.WEAPON:
                fireRateBtn.SetActive(true);
                damageBtn.SetActive(true);
                ammoCapacityBtn.SetActive(true);
                // SET PRODUCT INFORMATION
                productImg.sprite = weaponImgs[(int)currWeaponCategory];
                t_Cost.text = weapons[(int)currWeaponCategory].cost.ToString();
                t_Desc.text = weapons[(int)currWeaponCategory].name;
                // ACTIVE PRODUCT INFORMATION
                EnableProductInfo();
                leftArrowBtn.SetActive(true);
                rightArrowBtn.SetActive(true);
                UpdateWeaponStats(currWeaponCategory);
                break;
            case eCategory.ARMOR:
                productImg.sprite = armorImg;
                t_Cost.text = armorCost.ToString();
                t_Desc.text = armorDesc;
                EnableProductInfo();
                DisableWeaponInfo();
                break;
            case eCategory.SOLDIER:
                break;
            case eCategory.PLAY:
                costText.SetActive(false);
                buyButton.SetActive(false);
                t_Desc.enabled = false;
                nextWaveText.SetActive(true);
                t_nextWaveCount.text = GameManager.Instance.waveManager.GetComponent<WaveManager>().waveCount.ToString();
                productImg.enabled = false;
                DisableWeaponInfo();
                break;
            default:
                break;
        }
    }

    private void UpdateWeaponStats(eWeapons weapon)
    {
        switch (weapon)
        {
            case eWeapons.DEFAULT:
                fireRateBar.fillAmount = 0.4f;
                damageBar.fillAmount = 0.3f;
                ammoCapacityBar.fillAmount = 1.0f;
                buyButton.SetActive(false);
                break;
            case eWeapons.DAKKAGUN:
                fireRateBar.fillAmount = 1.0f;
                damageBar.fillAmount = 0.4f;
                ammoCapacityBar.fillAmount = 0.8f;
                buyButton.SetActive(true);
                break;
            case eWeapons.IMPALLINATOR:
                fireRateBar.fillAmount = 0.2f;
                damageBar.fillAmount = 0.6f;
                ammoCapacityBar.fillAmount = 0.3f;
                buyButton.SetActive(true);
                break;
            case eWeapons.ATOMIZER:
                fireRateBar.fillAmount = 1.0f;
                damageBar.fillAmount = 0.8f;
                ammoCapacityBar.fillAmount = 0.4f;
                buyButton.SetActive(isPlayer);
                break;
            case eWeapons.REKTIFIER:
                fireRateBar.fillAmount = 0.4f;
                damageBar.fillAmount = 1.0f;
                ammoCapacityBar.fillAmount = 0.2f;
                buyButton.SetActive(isPlayer);
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


    private void EnableProductInfo()
    {
        t_Desc.enabled = true;
        costText.SetActive(true);
        productImg.enabled = true;
        nextWaveText.SetActive(false);
        buyButton.SetActive(true);
    }
    private void DisableWeaponInfo()
    {
        leftArrowBtn.SetActive(false);
        rightArrowBtn.SetActive(false);
        fireRateBtn.SetActive(false);
        damageBtn.SetActive(false);
        ammoCapacityBtn.SetActive(false);
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
            soldierIMG[i].sprite = soldierLocked;
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
                soldierIMG[i].sprite = soldierUnlocked;
            }
        }
    }


    public void BuySoldier(int soldierIndex)
    {
        if (soldiers[soldierIndex].gameObject.activeInHierarchy)
        {
            return;
        }

        categoryType = eCategory.SOLDIER;
        Buy();
    }

    public void Buy()
    {
        switch (categoryType)
        {
            case eCategory.HP:
                if (!hasEnoughtCoins(p_PlayerManager.score, hpCost))
                {
                    ShowDialog(dialogNoEnoughtCoin, true);
                    return;
                }
                // TRY TO BUY SOLDIER THINGS WITHOUT SOLDIER
                if (!isPlayer && !soldiers[currSelectedSoldier].gameObject.activeInHierarchy)
                {
                    ShowDialog(String.Format(dialogNoSoldier, currSelectedSoldier + 1), true);
                    return;
                }
                ShowDialog(String.Format(dialogHp, hpCost));
                break;
            case eCategory.WEAPON:
                if (!hasEnoughtCoins(p_PlayerManager.score, weapons[(int)currWeaponCategory].cost))
                {
                    ShowDialog(dialogNoEnoughtCoin, true);
                    return;
                }
                // TRY TO BUY SOLDIER THINGS WITHOUT SOLDIER
                if (!isPlayer && !soldiers[currSelectedSoldier].gameObject.activeInHierarchy)
                {
                    ShowDialog(String.Format(dialogNoSoldier, currSelectedSoldier + 1), true);
                    return;
                }
                ShowDialog(String.Format(dialogWeapon, weapons[(int)currWeaponCategory].name, weapons[(int)currWeaponCategory].cost));
                break;
            case eCategory.ARMOR:
                if (!hasEnoughtCoins(p_PlayerManager.score, armorCost))
                {
                    ShowDialog(dialogNoEnoughtCoin, true);
                    return;
                }
                // TRY TO BUY SOLDIER THINGS WITHOUT SOLDIER
                if (!isPlayer && !soldiers[currSelectedSoldier].gameObject.activeInHierarchy)
                {
                    ShowDialog(String.Format(dialogNoSoldier,currSelectedSoldier+1), true);
                    return;
                }
                ShowDialog(String.Format(dialogHp, armorCost));
                break;
            case eCategory.PLAY:
                break;
            case eCategory.SOLDIER:
                if (!hasEnoughtCoins(p_PlayerManager.score, soldierCost))
                {
                    ShowDialog(String.Format(dialogNoCoinSoldier, soldierCost), true);
                    return;
                }
                ShowDialog(String.Format(dialogSoldier,currSelectedSoldier+1, soldierCost));
                break;
            default:
                break;
        }
    }
    public void ConfirmBuy()
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
                    Debug.Log("Bought player hp");
                }
                else
                {
                    p_PlayerManager.score -= hpCost;
                    s_SoldierManager[currSelectedSoldier].ReloadSoldierHP();
                    Debug.Log($"Bought {s_SoldierManager[currSelectedSoldier].transform.name} hp");
                }
                break;
            case eCategory.WEAPON:

                if (!hasEnoughtCoins(p_PlayerManager.score, weapons[(int)currWeaponCategory].cost))
                {
                    return;
                }
                if (isPlayer)
                {
                    p_PlayerManager.score -= weapons[(int)currWeaponCategory].cost;
                    p_WeaponSystem.SwitchWeapons(currWeaponCategory);
                    Debug.Log("Bought player weapon");
                }
                else
                {
                    p_PlayerManager.score -= weapons[(int)currWeaponCategory].cost;
                    s_WeaponSystem[currSelectedSoldier].SwitchWeapons(currWeaponCategory);
                    Debug.Log($"Bought {s_WeaponSystem[currSelectedSoldier].transform.name} weapon");
                }
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
                    Debug.Log("Bought player armor");
                }
                else
                {
                    p_PlayerManager.score -= armorCost;
                    s_SoldierManager[currSelectedSoldier].ReloadSoldierArmor();
                    Debug.Log($"Bought {s_SoldierManager[currSelectedSoldier].transform.name} armor");
                }
                break;
            case eCategory.SOLDIER:
                if (!hasEnoughtCoins(p_PlayerManager.score, soldierCost))
                {
                    return;
                }
                else
                {
                    p_PlayerManager.score -= soldierCost;
                    soldiers[currSelectedSoldier].gameObject.SetActive(true);
                    s_SoldierManager[currSelectedSoldier].ReloadSoldierStats();
                    EnableSoldierImage();
                }
                break;
            default:
                break;
        }
        HideDialog();
    }

    private void ShowDialog(string text, bool notEnoughCoin = false)
    {
        if (notEnoughCoin)
        {
            dialogMenu.SetActive(true);
            dialogText.text = text;
            dialogConfirmBtn.SetActive(false);
            dialogCancelBtn.SetActive(false);
            dialogBackBtn.SetActive(true);
            dialogBackBtn.GetComponent<Button>().Select();
        }
        else
        {
            dialogMenu.SetActive(true);
            dialogText.text = text;
            dialogConfirmBtn.SetActive(true);
            dialogCancelBtn.SetActive(true);
            dialogBackBtn.SetActive(false);
            dialogConfirmBtn.GetComponent<Button>().Select();
        }

    }
    public void HideDialog()
    {
        dialogText.text = "";
        dialogMenu.SetActive(false);
    }

    private bool hasEnoughtCoins(int playerScore, int cost)
    {
        return playerScore >= cost;
    }
    private float Map(float input, float inputMin, float inputMax, float min, float max)
    {
        return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
    }
}
