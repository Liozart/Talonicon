using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HealthPotion, ManaPotion
}

public class ItemsData : MonoBehaviour
{
    public Dictionary<ItemType, Sprite> itemSprites = new Dictionary<ItemType, Sprite>();
    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;

    public AudioClip[] audio_PotionsUse;

    private void Awake()
    {
        //Sprite list init
        itemSprites.Add(ItemType.HealthPotion, healthPotionSprite);
        itemSprites.Add(ItemType.ManaPotion, manaPotionSprite);
    }
}

public static class ItemBible
{
    public static Dictionary<ItemType, string> ItemNames = new Dictionary<ItemType, string>()
    {
        { ItemType.HealthPotion, "Health potion" },
        { ItemType.ManaPotion, "Mana potion" }
    };
    public static Dictionary<ItemType, string> ItemDescriptions = new Dictionary<ItemType, string>()
    {
        { ItemType.HealthPotion, "Restores a few health." },
        { ItemType.ManaPotion, "Revigores a bit of mana." }
    };

    public static Dictionary<ItemType, int> ItemPrices = new Dictionary<ItemType, int>()
    {
        { ItemType.HealthPotion, 100 },
        { ItemType.ManaPotion, 100 }
    };
}