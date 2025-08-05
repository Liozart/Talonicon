using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum NPCType
{
    Merchant, Blacksmith
}

public class NPC : MonoBehaviour
{
    public NPCType NPCType;
    public int currentTier;
    public List<ItemType> currentItemsList;
    int currentDialogue;
    public Canvas canvasDialogue;
    public TMP_Text text_dialogue;
    public AudioSource audioSource;
    public AudioClip[] audio_talk;

    private void Awake()
    {
        currentTier = 0;
        currentDialogue = 0;
        canvasDialogue.enabled = false;
        audioSource = GetComponent<AudioSource>();
        RefillShopItems(0);
    }

    public void TriggerNextDialogue(Inventory playerInv)
    {
        StopAllCoroutines();
        canvasDialogue.enabled = true;
        if (currentDialogue == NPCBible.Dialogues[NPCType].Count)
            currentDialogue = 0;
        if (currentDialogue == 0)
        {
            audioSource.clip = audio_talk[Random.Range(0, audio_talk.Length)];
            audioSource.Play();
        }
        text_dialogue.text = NPCBible.Dialogues[NPCType][currentDialogue];
        text_dialogue.alpha = 255;
        currentDialogue++;
        StartCoroutine(FadeOutText());

        switch (NPCType)
        {
            case NPCType.Merchant:
                if (currentDialogue == 3)
                    playerInv.OpenMerchantUI(currentItemsList);
                break;
        }
    }

    IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(3f);
        canvasDialogue.enabled = false;
    }

    public void RefillShopItems(int tier)
    {
        switch (NPCType)
        {
            case NPCType.Merchant:
                currentItemsList = new List<ItemType>(NPCBible.Merchant_ItemsListsPerTier[tier]);
                break;
        }
    }
}

public static class NPCBible
{
    public static Dictionary<NPCType, List<string>> Dialogues = new Dictionary<NPCType, List<string>>()
    {
        { 
            NPCType.Merchant,
            new List<string>{
                { "Hello there." },
                { "I'm selling some stuff for people like you."  },
                { "Please have a look." },
                { "Come back later, I will have a new stock." }
            }
        }
    };

    public static Dictionary<int, List<ItemType>> Merchant_ItemsListsPerTier = new Dictionary<int, List<ItemType>>()
    {
        { 
            0,
            new List<ItemType>()
            {
                { ItemType.HealthPotion },
                { ItemType.HealthPotion },
                { ItemType.ManaPotion },
                { ItemType.ManaPotion }
            }
        }
    };
}