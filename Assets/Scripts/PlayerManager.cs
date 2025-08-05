using StarterAssets;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int healthMax;
    public int stamina;
    public int staminaMax;
    public float staminaCooldown;
    public float staminaCooldownMax;
    public int mana;
    public int manaMax;

    public int XP;
    public int level;
    public int nextLevel;

    public int minArmorValue;
    public int maxArmorValue;

    public bool isDead;
    public bool hasGrabbedBreakable;
    public bool canAttack;
    public GameObject grabbedBreakable;
    public Rigidbody grabbedBreakableRB;
    bool canGrab;
    public AudioClip audio_throwBreakable;

    Camera playerCamera;

    public GameObject Handle;
    public GameObject HandleMagic;
    public GameObject GrabSpot;

    public Inventory inventory;
    public MoodManager moodManager;
    public WeaponManager WeaponManager;
    public MagicManager MagicManager;
    public WeaponsData weaponsData;

    [Header("UI")]
    public TMP_Text Text_HP;
    public RectTransform Image_HP;
    public RectTransform Image_Stamina;
    public RectTransform Image_Mana;
    public TMP_Text Text_Death;
    int Image_HPMaxWidth = 255;

    [Header("NPC UI")]
    public Canvas Canvas_NPCMerchant;

    FirstPersonController firstPersonController;
    #if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
    #endif
    StarterAssetsInputs _input;
    CharacterController characterController;
    AudioSource audioSource;

    int layerInteractable = 1 << 12;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthMax = 3;
        health = healthMax;
        staminaMax = 10;
        stamina = staminaMax;
        staminaCooldownMax = 0.5f;
        manaMax = 10;
        mana = manaMax;

        XP = 0;
        nextLevel = 10;

        minArmorValue = 0;
        maxArmorValue = 0;

        isDead = false;
        hasGrabbedBreakable = false;
        grabbedBreakable = null;
        grabbedBreakableRB = null;
        canGrab = true;
        canAttack = true;

        playerCamera = Camera.main;

        inventory = GetComponent<Inventory>();
        moodManager = GetComponent<MoodManager>();

        Image_HPMaxWidth = (int)Image_HP.sizeDelta.x;
        UpdateHealthText();
        Text_Death.enabled = false;

        firstPersonController = GetComponent<FirstPersonController>();
        characterController = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        #if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#       endif

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stamina management
        if (_input.sprint)
        {
            if (_input.move != Vector2.zero)
            {
                if (stamina >= 0)
                {
                    firstPersonController.canSprint = true;
                    staminaCooldown -= Time.deltaTime;
                    if (staminaCooldown <= 0)
                    {
                        staminaCooldown = staminaCooldownMax;
                        stamina -= 1;
                        UpdateStaminBar();
                    }
                }
                else
                {
                    firstPersonController.canSprint = false;
                }
            }
        }
        else
        {
            //Stamina regen
            if (stamina < staminaMax)
            {
                staminaCooldown -= Time.deltaTime;
                if (staminaCooldown <= 0)
                {
                    staminaCooldown = staminaCooldownMax / 2;
                    stamina += 1;
                    UpdateStaminBar();
                }
            }
        }

        if (isDead || inventory.isShopOpen) 
            return;

        //Inventory key
        if (_input.inventory)
        {
            inventory.ToggleInventory();
            _input.inventory = false;
        }

        //Stop actions if inventory is open
        if (inventory.isOpen)
            return;

        if (_input.alt)
        {
            if (inventory.hasEquippedMagic)
                MagicManager.Magic();
        }

        //Attack key
        RaycastHit interactHit;
        if (_input.attack)
        {
            if (stamina > 0)
                if (!hasGrabbedBreakable)
                {
                    if (inventory.hasEquippedWeapon)
                    {
                        if (canAttack)
                            WeaponManager.Attack();
                    }
                }
                else
                {
                    hasGrabbedBreakable = false;
                    grabbedBreakable.transform.SetParent(null);
                    grabbedBreakableRB.isKinematic = false;
                    grabbedBreakableRB.linearVelocity = playerCamera.transform.forward * 15f;
                    grabbedBreakable.GetComponent<Breakable>().isThrown = true;
                    grabbedBreakable = null;
                    grabbedBreakableRB = null;
                    stamina -= 2;
                    StartCoroutine(ThrowBreakableCooldown());
                    Handle.SetActive(true);
                    audioSource.clip = audio_throwBreakable;
                    audioSource.Play();
                }
        }

        //Interaction
        if (_input.interact)
        {
            if (!canGrab) return;

            StartCoroutine(DisableInteract());
            if (!hasGrabbedBreakable)
            {
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out interactHit, 3f, layerInteractable))
                {
                    if (interactHit.transform.CompareTag("NPC"))
                    {
                        NPC NPCChecked = interactHit.transform.gameObject.GetComponent<NPC>();
                        switch (NPCChecked.NPCType)
                        {
                            case NPCType.Merchant:
                                NPCChecked.TriggerNextDialogue(inventory);
                                break;
                        }
                    }
                    else
                    if (interactHit.transform.CompareTag("Breakable"))
                    {
                        hasGrabbedBreakable = true;
                        grabbedBreakable = interactHit.transform.gameObject;
                        interactHit.transform.SetParent(GrabSpot.transform);
                        interactHit.transform.position = GrabSpot.transform.position;
                        grabbedBreakableRB = grabbedBreakable.GetComponent<Rigidbody>();
                        grabbedBreakableRB.isKinematic = true;
                        Handle.SetActive(false);
                    }
                    else
                    if (interactHit.transform.CompareTag("Chest"))
                    {
                        interactHit.transform.GetComponent<Chest>().OpenChest();
                    }
                }
            }
            else
            {
                hasGrabbedBreakable = false;
                grabbedBreakable.transform.SetParent(null);
                grabbedBreakableRB.isKinematic = false;
                grabbedBreakable.GetComponent<Breakable>().isThrown = true;
                grabbedBreakable = null;
                grabbedBreakableRB = null;
                Handle.SetActive(true);
            }
        }
    }

    IEnumerator DisableInteract()
    {
        canGrab = false;
        yield return new WaitForSeconds(0.2f);
        canGrab = true;
    }

    IEnumerator ThrowBreakableCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    bool isCollisionProcessed = false;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isCollisionProcessed)
            return;

        isCollisionProcessed = true;

        if (hit.transform.CompareTag("Coin"))
        {
            inventory.UpdateCoins(hit.gameObject.GetComponent<Coin>().amount);
            Destroy(hit.gameObject);
        }
        else
        if (hit.transform.CompareTag("Item"))
        {
            Item w = hit.transform.GetComponent<Item>();
            inventory.AddItem(w.itemType, false);
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
            else
            {
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.GetComponent<Collider>().enabled = false;
            }
        }
        else
        if (hit.transform.CompareTag("Armor"))
        {
            Armor a = hit.transform.GetComponent<Armor>();
            if (!inventory.armors.Contains(a.armorTye))
            {
                inventory.AddArmor(a.armorTye);
                Destroy(hit.gameObject);
            }
            else
            {
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.GetComponent<Collider>().enabled = false;
            }
        }
        else
        if (hit.transform.CompareTag("Magic"))
        {
            Magic a = hit.transform.GetComponent<Magic>();
            if (!inventory.magics.Contains(a.magicType))
            {
                inventory.AddMagic(a.magicType);
                Destroy(hit.gameObject);
            }
            else
            {
                hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                hit.transform.GetComponent<Collider>().enabled = false;
            }
        }
        StartCoroutine(IsCollisionProcessed());
    }

    public void WeaponDamage(Enemy enemy, int dam)
    {
        moodManager.ChangeMoodForTime(Mood.Angry, 4.0f);
        enemy.Damage(dam, false);
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

    public void TakeDamage(int dam, DamageType type)
    {
        audioSource.clip = weaponsData.EnemyDamageAudioClips[type][UnityEngine.Random.Range(0, weaponsData.EnemyDamageAudioClips[type].Length)];
        audioSource.Play();

        //Test armor pen
        int res = UnityEngine.Random.Range(minArmorValue, maxArmorValue + 1);
        dam -= res;
        if (dam <= 0)
            return;

        health -= dam;
        if (health < 0)
            health = 0;

        UpdateHealthText();

        if (health == 0)
        {
            isDead = true;
            Text_Death.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            firstPersonController.enabled = false;
            moodManager.ChangeMood(Mood.Dead, true);
        }
        else
        {
            firstPersonController.Shake(0.2f);
            if (health == 1)
            {
                moodManager.ChangeMood(Mood.Sad, true);
            }
        }
    }

    public void Heal(int v)
    {
        health += v;
        moodManager.ChangeMood(Mood.Normal, false);
        if (health > healthMax)
        {
            health = healthMax;
            moodManager.ChangeMoodForTime(Mood.Happy, 3f);
        }
        UpdateHealthText();
    }

    public void AddMana(int v)
    {
        mana += v;
        if (mana > manaMax)
        {
            mana = manaMax;
            moodManager.ChangeMoodForTime(Mood.Happy, 3f);
        }
        UpdateManaBar();
    }

    public void UpdateHealthText()
    {
        Text_HP.text = $"{health}/{healthMax}";
        Image_HP.sizeDelta = new Vector2(Image_HPMaxWidth / healthMax * health, Image_HP.sizeDelta.y);
    }

    public void UpdateStaminBar()
    {
        Image_Stamina.sizeDelta = new Vector2(Image_HPMaxWidth / staminaMax * stamina, Image_Stamina.sizeDelta.y);
    }

    public void UpdateManaBar()
    {
        Image_Mana.sizeDelta = new Vector2(Image_HPMaxWidth / manaMax * mana, Image_Mana.sizeDelta.y);
    }
    IEnumerator IsCollisionProcessed()
    {
        yield return new WaitForEndOfFrame();
        isCollisionProcessed = false;
    }
}
