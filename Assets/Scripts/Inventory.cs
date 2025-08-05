using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryPage
{
    Items, Weapons, Armor, Magic
}

public class Inventory : MonoBehaviour
{
    public ItemsData itemData;
    public WeaponsData weaponsData;
    public ArmorData armorData;
    public MagicData magicData;
    public Canvas canvasInventory;
    public Texture2D cursorTexture;
    public bool isOpen;
    public bool isShopOpen;
    public InventoryPage currentPage;

    [Header("UI inventory")]
    public TMP_Text text_Coins;
    public TMP_Text text_ItemTitle;
    public TMP_Text text_ItemDescription;
    public Button button_use;
    public TMP_Text text_button_use;

    [Header("UI equipments")]
    public Image button_equipmentWeapon;
    public Image button_equipmentArmor;
    public Image button_equipmentMagic;

    [Header("UI Shops")]
    public Canvas canvasMerchant;
    public GameObject merchantShopContent;
    public TMP_Text text_ShopCoins;
    public TMP_Text text_ShopItemTitle;
    public TMP_Text text_ShopItemDescription;
    public Button button_Buy;

    public Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    public List<WeaponType> weapons = new List<WeaponType>();
    public List<ArmorType> armors = new List<ArmorType>();
    public List<MagicType> magics = new List<MagicType>();
    public int coins;

    public TMP_Text inventoryContentTitle;
    public GameObject inventoryContent;

    public WeaponType equippedWeapon;
    public bool hasEquippedWeapon;
    public ArmorType equippedArmor;
    public bool hasEquippedArmor;
    public MagicType equippedMagic;
    public bool hasEquippedMagic;
    public Animator weaponAnimator;
    AudioSource audioSource;
    PlayerManager playerManager;
    public WeaponManager weaponManager;
    public MagicManager magicManager;

    [Header("UI Sounds")]
    public AudioClip audio_openBag;
    public AudioClip audio_closeag;
    public AudioClip audio_getCoins;
    public AudioClip audio_getCoinsBag;
    public AudioClip audio_pickupItem;
    public AudioClip audio_pickWeapon;
    public AudioClip audio_pickArmor;

    public AudioClip audio_equipWeapon;
    public AudioClip audio_equipClothArmor;
    public AudioClip audio_equipMagic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPage = InventoryPage.Items;
        coins = 0;
        text_Coins.text = coins.ToString();
        isOpen = false;
        isShopOpen = false;
        canvasInventory.enabled = false;
        canvasMerchant.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;

        audioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();

        hasEquippedWeapon = false;
        hasEquippedMagic = false;
        hasEquippedArmor = false;

       /*equippedWeapon = WeaponType.Dagger;
        AddWeapon(equippedWeapon);
        EquipWeapon(equippedWeapon);*/

        DrawInventoryPage(currentPage);
    }

    public void UseItem(ItemType it)
    {
        //Manage item in inventory
        items[it] -= 1;
        if (items[it] == 0)
        {
            items.Remove(it);
            DrawInventoryPage(currentPage);
            button_use.onClick.RemoveAllListeners();
        }
        else
        {
            text_ItemTitle.text = $"{ItemBible.ItemNames[it]} x{items[it]}";
        }
        //Item effect
        switch (it)
        {
            case ItemType.HealthPotion:
                playerManager.Heal(3);
                audioSource.clip = itemData.audio_PotionsUse[Random.Range(0, itemData.audio_PotionsUse.Length)];
                audioSource.Play();
                break;
            case ItemType.ManaPotion:
                playerManager.AddMana(5);
                audioSource.clip = itemData.audio_PotionsUse[Random.Range(0, itemData.audio_PotionsUse.Length)];
                audioSource.Play();
                break;
        }
    }

    public void EquipWeapon(WeaponType wep)
    {
        //Update weaponmanager vars
        weaponManager.equippedWeapon = wep;
        weaponManager.equippedWeaponWhoosh = WeaponsBible.WeaponsWhoosh[wep];
        //Change handled gameobject
        if (hasEquippedWeapon)
            weaponsData.weaponsGameobjects[equippedWeapon].SetActive(false);
        equippedWeapon = wep;
        hasEquippedWeapon = true;
        weaponsData.weaponsGameobjects[equippedWeapon].SetActive(true);
        //Change attack cooldown
        weaponManager.damageCooldownMax = WeaponsBible.WeaponsCooldowns[equippedWeapon];
        //Set correct animation
        weaponAnimator.SetInteger("Weapon", (int)equippedWeapon);
        //Play audio
        audioSource.clip = audio_equipWeapon;
        audioSource.Play();
        //Change active weapon collider 
        weaponManager.SetWeaponCollider(weaponsData.weaponsColliders[equippedWeapon]);
        //Update equipment UI
        button_equipmentWeapon.sprite = weaponsData.weaponSprites[equippedWeapon];
    }

    public void EquipArmor(ArmorType arm)
    {
        equippedArmor = arm;
        playerManager.minArmorValue = ArmorBible.ArmorMinValue[arm];
        playerManager.maxArmorValue = ArmorBible.ArmorMaxValue[arm];
        audioSource.clip = audio_equipClothArmor;
        audioSource.Play();
        button_equipmentArmor.sprite = armorData.armorSprites[arm];
    }

    public void EquipMagic(MagicType mag)
    {
        //Update magicmanager vars
        magicManager.equippedMagic = mag;
        magicManager.SetMagicsVars();
        //Change handled gameobject
        if (hasEquippedMagic)
            magicData.magicGameobjects[equippedMagic].SetActive(false);
        hasEquippedMagic = true;
        equippedMagic = mag;
        magicData.magicGameobjects[equippedMagic].SetActive(true);
        //Changes magic cooldowns
        magicManager.damageCooldownMax = MagicBible.MagicCooldowns[equippedMagic];
        //Play audio
        audioSource.clip = audio_equipMagic;
        audioSource.Play();
        //Update equipment UI
        button_equipmentMagic.sprite = magicData.magicSprites[mag];
    }

    public void ToggleInventory()
    {
        if (isOpen)
            Close();
        else Open();

        isOpen = !isOpen;
    }

    public void Open()
    {
        canvasInventory.enabled = true;
        Cursor.SetCursor(cursorTexture, Vector2.zero, UnityEngine.CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        audioSource.clip = audio_openBag;
        audioSource.Play();
    }

    public void Close()
    {
        canvasInventory.enabled = false;
        Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        audioSource.clip = audio_closeag;
        audioSource.Play();
        text_ItemTitle.text = "";
        text_ItemDescription.text = "";
        button_use.onClick.RemoveAllListeners();
    }

    public void UpdateCoins(int amountToAdd)
    {
        coins += amountToAdd;
        text_Coins.text = coins.ToString();
        if (amountToAdd > 10)
            audioSource.clip = audio_getCoinsBag;
        else
            audioSource.clip = audio_getCoins;
        audioSource.Play();
    }

    public void AddWeapon(WeaponType weapon)
    {
        weapons.Add(weapon);
        audioSource.clip = audio_pickWeapon;
        audioSource.Play();

        if (currentPage == InventoryPage.Weapons)
            DrawInventoryPage(currentPage);
    }

    public void AddItem(ItemType item, bool isBought)
    {
        if (!items.ContainsKey(item))
        {
            items.Add(item, 1);
            if (currentPage == InventoryPage.Items)
                DrawInventoryPage(currentPage);
        }
        else
            items[item] += 1;

        if (isBought)
            audioSource.clip = audio_getCoins;
        else
            audioSource.clip = audio_pickupItem;
        audioSource.Play();
    }

    public void AddArmor(ArmorType armor)
    {
        armors.Add(armor);
        audioSource.clip = audio_pickArmor;
        audioSource.Play();

        if (currentPage == InventoryPage.Armor)
            DrawInventoryPage(currentPage);
    }

    public void AddMagic(MagicType magic)
    {
        magics.Add(magic);
        audioSource.clip = audio_pickWeapon;
        audioSource.Play();

        if (currentPage == InventoryPage.Magic)
            DrawInventoryPage(currentPage);
    }

    public void DrawInventoryPage(InventoryPage page)
    {
        text_ItemTitle.text = "";
        text_ItemDescription.text = "";

        for (int i = 0; i < inventoryContent.transform.childCount; i++)
            Destroy(inventoryContent.transform.GetChild(i).gameObject);

        switch (page)
        {
            case InventoryPage.Items:
                inventoryContentTitle.text = "Items";
                text_button_use.text = "Use";
                foreach (var item in items)
                {
                    GameObject g = new GameObject("Btn", typeof(RectTransform), typeof(Button), typeof(Image));
                    g.transform.SetParent(inventoryContent.transform);
                    g.transform.rotation = Quaternion.Euler(0, 0, 0);
                    g.GetComponent<Image>().sprite = itemData.itemSprites[item.Key];
                    g.GetComponent<Button>().onClick.AddListener(() => ItemClick(item.Key));
                }
                break;

            case InventoryPage.Weapons:
                inventoryContentTitle.text = "Weapons";
                text_button_use.text = "Equip";
                foreach (WeaponType weapon in weapons)
                {
                    GameObject g = new GameObject("Btn", typeof(RectTransform), typeof(Button), typeof(Image));
                    g.transform.SetParent(inventoryContent.transform);
                    g.GetComponent<Image>().sprite = weaponsData.weaponSprites[weapon];
                    g.GetComponent<Button>().onClick.AddListener(() => WeaponClick(weapon));
                }
                break;

            case InventoryPage.Magic:
                inventoryContentTitle.text = "Magic";
                text_button_use.text = "Equip";
                foreach (MagicType magic in magics)
                {
                    GameObject g = new GameObject("Btn", typeof(RectTransform), typeof(Button), typeof(Image));
                    g.transform.SetParent(inventoryContent.transform);
                    g.GetComponent<Image>().sprite = magicData.magicSprites[magic];
                    g.GetComponent<Button>().onClick.AddListener(() => MagicClick(magic));
                }
                break;

            case InventoryPage.Armor:
                inventoryContentTitle.text = "Armor";
                text_button_use.text = "Equip";
                foreach (ArmorType armor in armors)
                {
                    GameObject g = new GameObject("Btn", typeof(RectTransform), typeof(Button), typeof(Image));
                    g.transform.SetParent(inventoryContent.transform);
                    g.GetComponent<Image>().sprite = armorData.armorSprites[armor];
                    g.GetComponent<Button>().onClick.AddListener(() => ArmorClick(armor));
                }
                break;
        }
    }

    public void InventoryButtonClick_Items()
    {
        currentPage = InventoryPage.Items;
        DrawInventoryPage(currentPage);
    }
    public void InventoryButtonClick_Weapon()
    {
        currentPage = InventoryPage.Weapons;
        DrawInventoryPage(currentPage);
    }

    public void InventoryButtonClick_Magic()
    {
        currentPage = InventoryPage.Magic;
        DrawInventoryPage(currentPage);
    }

    public void InventoryButtonClick_Armor()
    {
        currentPage = InventoryPage.Armor;
        DrawInventoryPage(currentPage);
    }

    public void ItemClick(ItemType item)
    {
        text_ItemTitle.text = $"{ItemBible.ItemNames[item]} x{items[item]}";
        text_ItemDescription.text = ItemBible.ItemDescriptions[item];
        button_use.onClick.RemoveAllListeners();
        button_use.onClick.AddListener(() => UseItem(item));
    }

    public void WeaponClick(WeaponType weapon)
    {
        text_ItemTitle.text = WeaponsBible.WeaponsNames[weapon];
        text_ItemDescription.text = WeaponsBible.WeaponsDescriptions[weapon];
        button_use.onClick.RemoveAllListeners();
        button_use.onClick.AddListener(() => EquipWeapon(weapon));
    }

    public void ArmorClick(ArmorType armor)
    {
        text_ItemTitle.text = ArmorBible.ArmorNames[armor];
        text_ItemDescription.text = ArmorBible.ArmorDescriptions[armor];
        button_use.onClick.RemoveAllListeners();
        button_use.onClick.AddListener(() => EquipArmor(armor));
    }

    public void MagicClick(MagicType magic)
    {
        text_ItemTitle.text = MagicBible.MagicNames[magic];
        text_ItemDescription.text = MagicBible.MagicDescriptions[magic];
        button_use.onClick.RemoveAllListeners();
        button_use.onClick.AddListener(() => EquipMagic(magic));
    }

    public void OpenMerchantUI(List<ItemType> itemsList)
    {
        for (int i = 0; i < merchantShopContent.transform.childCount; i++)
            Destroy(merchantShopContent.transform.GetChild(i).gameObject);

        canvasMerchant.enabled = true;
        Cursor.SetCursor(cursorTexture, Vector2.zero, UnityEngine.CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        audioSource.clip = audio_openBag;
        audioSource.Play();
        isShopOpen = true;

        text_ShopCoins.text = text_Coins.text;

        //Draw shop inventory
        foreach (ItemType item in itemsList)
        {
            GameObject g = new GameObject("Btn", typeof(RectTransform), typeof(Button), typeof(Image));
            g.transform.SetParent(merchantShopContent.transform);
            g.GetComponent<Image>().sprite = itemData.itemSprites[item];
            g.GetComponent<Button>().onClick.AddListener(() => ShopItemClick(item, g));
        }
    }

    public void ShopItemClick(ItemType item, GameObject btn)
    {
        text_ShopItemTitle.text = $"{ItemBible.ItemNames[item]} ({ItemBible.ItemPrices[item]}g)";
        text_ShopItemDescription.text = ItemBible.ItemDescriptions[item];
        button_Buy.onClick.RemoveAllListeners();
        button_Buy.onClick.AddListener(() => BuyItem(item, btn));
    }

    public void BuyItem(ItemType item, GameObject btn)
    {
        if (coins >= ItemBible.ItemPrices[item])
        {
            text_ShopItemTitle.text = "";
            text_ShopItemDescription.text = "";
            Destroy(btn);
            AddItem(item, true);
            coins -= ItemBible.ItemPrices[item];
            text_Coins.text = text_ShopCoins.text = coins.ToString();
        }
    }

    public void CloseMerchantUI()
    {
        canvasMerchant.enabled = false;
        Cursor.SetCursor(null, Vector2.zero, UnityEngine.CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Locked;
        audioSource.clip = audio_closeag;
        audioSource.Play();
        isShopOpen = false;
        text_ShopItemTitle.text = "";
        text_ShopItemDescription.text = "";
        button_Buy.onClick.RemoveAllListeners();
    }
}
