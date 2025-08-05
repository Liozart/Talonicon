using System.Collections.Generic;
using UnityEngine;
public enum ArmorType
{
    Cloak, WookenArmor, TinArmor
}

public class ArmorData : MonoBehaviour
{
    public Dictionary<ArmorType, Sprite> armorSprites = new Dictionary<ArmorType, Sprite>();
    public Sprite cloakSprite;
    public Sprite WoodenArmorSprite;
    public Sprite TinArmorSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        armorSprites.Add(ArmorType.Cloak, cloakSprite);
        armorSprites.Add(ArmorType.WookenArmor, WoodenArmorSprite);
        armorSprites.Add(ArmorType.TinArmor, TinArmorSprite);
    }
}

public static class ArmorBible
{
    public static Dictionary<ArmorType, string> ArmorNames = new Dictionary<ArmorType, string>()
    {
        { ArmorType.Cloak, "Cloak" },
        { ArmorType.WookenArmor, "Wooden armor" },
        { ArmorType.TinArmor, "Tin armor" }
    };
    public static Dictionary<ArmorType, string> ArmorDescriptions = new Dictionary<ArmorType, string>()
    {
        { ArmorType.Cloak, "Better than nothing." },
        { ArmorType.WookenArmor, "Made of old sturdy planks." },
        { ArmorType.TinArmor, "A low quality duel armor." }
    };
    public static Dictionary<ArmorType, int> ArmorMinValue = new Dictionary<ArmorType, int>()
    {
        { ArmorType.Cloak, 0 },
        { ArmorType.WookenArmor, 0 },
        { ArmorType.TinArmor, 1 }
    };
    public static Dictionary<ArmorType, int> ArmorMaxValue = new Dictionary<ArmorType, int>()
    {
        { ArmorType.Cloak, 1 },
        { ArmorType.WookenArmor, 2 },
        { ArmorType.TinArmor, 1 }
    };
}
