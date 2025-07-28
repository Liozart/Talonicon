using StarterAssets;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

public class PlayerManager : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int maxHealth;

    public int XP;
    public int level;
    public int nextLevel;

    public GameObject Handle;

    public Inventory inventory;
    public MoodManager moodManager;
    public WeaponManager WeaponManager;

    [Header("UI")]
    public TMP_Text Text_HP;
    public RectTransform Image_HP;
    public TMP_Text Text_Death;
    int Image_HPMaxWidth = 255;

    FirstPersonController firstPersonController;
    #if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
    #endif
    StarterAssetsInputs _input;
    CharacterController _controller;
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 3;
        health = maxHealth;

        XP = 0;
        nextLevel = 10;

        inventory = GetComponent<Inventory>();
        moodManager = GetComponent<MoodManager>();

        Image_HPMaxWidth = (int)Image_HP.sizeDelta.x;
        UpdateHealthText();
        Text_Death.enabled = false;

        firstPersonController = GetComponent<FirstPersonController>();
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        #if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#       endif

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Inventory key
        if (_input.inventory)
        {
            inventory.ToggleInventory();
            _input.inventory = false;
        }

        //Stop actions if invenory is open
        if (inventory.isOpen)
            return;

        //Attack key
        if (_input.attack)
        {
            WeaponManager.Attack();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Coin"))
        {
            inventory.UpdateCoins(hit.gameObject.GetComponent<Coin>().amount);
            Destroy(hit.gameObject);
        }
        else
        if (hit.transform.CompareTag("Item"))
        {
            Item w = hit.transform.GetComponent<Item>();
            inventory.AddItem(w.itemType);
            Destroy(hit.gameObject);
        }
        else
        if (hit.transform.CompareTag("Weapon"))
        {
            Weapon w = hit.transform.GetComponent<Weapon>();
            if (!inventory.weapons.Contains(w.weaponType))
            {
                inventory.AddWeapon(w.weaponType);
                Destroy(hit.gameObject);
            }
        }
    }

    public void WeaponDamage(Enemy enemy)
    {
        moodManager.ChangeMood(Mood.Angry, false);
        enemy.Damage(1);
    }

    public void WinXP(int cb)
    {
        XP += cb;

        if (XP >= nextLevel)
        {
            level++;
            nextLevel *= 2;
        }
    }

    public void TakeDamage(int dam)
    {
        health -= dam;
        UpdateHealthText();
        if (health <= 0)
        {
            Text_Death.enabled = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (health == 1)
            {
                moodManager.ChangeMood(Mood.Sad, true);
            }
        }

    }

    public void UpdateHealthText()
    {
        Text_HP.text = $"{health}/{maxHealth}";
        Image_HP.sizeDelta = new Vector2(Image_HPMaxWidth / maxHealth * health, Image_HP.sizeDelta.y);
    }
}
