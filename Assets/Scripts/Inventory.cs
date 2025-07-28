using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryPage
{
    Items, Weapons, Armor
}

public class Inventory : MonoBehaviour
{
    public ItemsData itemData;
    public WeaponsData weaponsData;
    public Canvas canvasInventory;
    public Texture2D cursorTexture;
    public bool isOpen;
    public InventoryPage currentPage;

    public TMP_Text text_Coins;
    public TMP_Text text_ItemTitle;
    public TMP_Text text_ItemDescription;
    public Button button_use;
    public TMP_Text text_button_use;

    public Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();
    public List<WeaponType> weapons = new List<WeaponType>();
    public int coins;

    public TMP_Text inventoryContentTitle;
    public GameObject inventoryContent;

    public WeaponType equippedWeapon;
    Animator animator;
    AudioSource audioSource;
    PlayerManager playerManager;
    public WeaponManager weaponManager;

    [Header("UI Sounds")]
    public AudioClip audio_openBag;
    public AudioClip audio_closeag;
    public AudioClip audio_getCoins;
    public AudioClip audio_getCoinsBag;
    public AudioClip audio_pickupItem;
    public AudioClip audio_pickWeapon;

    public AudioClip audio_equipWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPage = InventoryPage.Items;
        coins = 0;
        text_Coins.text = coins.ToString();
        isOpen = false;
        canvasInventory.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();

        equippedWeapon = WeaponType.Dagger;
        AddWeapon(equippedWeapon);
        EquipWeapon(equippedWeapon);
        weaponManager.SetWeaponCollider(weaponsData.weaponsColliders[equippedWeapon]);

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
        }
        //Item effect
    }

    public void EquipWeapon(WeaponType wep)
    {
        //Update weaponmanager vars
        weaponManager.equippedWeapon = wep;
        weaponManager.equippedWeaponWhoosh = WeaponsBible.WeaponsWhoosh[wep];
        //Change handled gameobject
        weaponsData.weaponsGameobjects[equippedWeapon].SetActive(false);
        equippedWeapon = wep;
        weaponsData.weaponsGameobjects[equippedWeapon].SetActive(true);
        //Change attack cooldown
        weaponManager.damageCooldownMax = WeaponsBible.WeaponsCooldowns[equippedWeapon];
        //Set correct animation
        animator.SetInteger("Weapon", (int)equippedWeapon);
        //Play audio
        audioSource.clip = audio_equipWeapon;
        audioSource.Play();
        //Change active weapon collider 
        weaponManager.SetWeaponCollider(weaponsData.weaponsColliders[equippedWeapon]);
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

    public void AddItem(ItemType item)
    {
        if (!items.ContainsKey(item))
            items.Add(item, 1);
        else
            items[item] += 1;

        audioSource.clip = audio_pickupItem;
        audioSource.Play();

        if (currentPage == InventoryPage.Items)
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
        }
    }

    public void ButtonClick_Items()
    {
        currentPage = InventoryPage.Items;
        DrawInventoryPage(currentPage);
    }
    public void ButtonClick_Weapon()
    {
        currentPage = InventoryPage.Weapons;
        DrawInventoryPage(currentPage);
    }

    public void ItemClick(ItemType item)
    {
        text_ItemTitle.text = ItemBible.ItemNames[item];
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
}
