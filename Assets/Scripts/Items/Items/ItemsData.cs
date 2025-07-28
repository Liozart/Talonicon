using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HealthPotion
}

public class ItemsData : MonoBehaviour
{
    public Dictionary<ItemType, Sprite> itemSprites = new Dictionary<ItemType, Sprite>();
    public Sprite healthPotionSprite;

    private void Awake()
    {
        //Sprite list init
        itemSprites.Add(ItemType.HealthPotion, healthPotionSprite);
    }
}

public static class ItemBible
{
    public static Dictionary<ItemType, string> ItemNames = new Dictionary<ItemType, string>()
    {
        { ItemType.HealthPotion, "Health potion" },
    };
    public static Dictionary<ItemType, string> ItemDescriptions = new Dictionary<ItemType, string>()
    {
        { ItemType.HealthPotion, "A boring health potion. Resore a few health." },
    };
}