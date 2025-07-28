using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Dagger = 0,
    StoneSword = 1
}

public enum WhooshType
{
    Small, Medium, Big
}

public class WeaponsData : MonoBehaviour
{
    public Dictionary<WeaponType, Sprite> weaponSprites = new Dictionary<WeaponType, Sprite>();
    public Sprite daggerSprite;
    public Sprite stoneSwordSprite;

    public Dictionary<WeaponType, GameObject> weaponsGameobjects = new Dictionary<WeaponType, GameObject>();
    public GameObject daggerGameobject;
    public GameObject stoneSwordGameobject;

    public Dictionary<WeaponType, BoxCollider> weaponsColliders = new Dictionary<WeaponType, BoxCollider>();

    private void Awake()
    {
        //Sprite list init
        weaponSprites.Add(WeaponType.Dagger, daggerSprite);
        weaponSprites.Add(WeaponType.StoneSword, stoneSwordSprite);

        //Weapon gamobjects list init
        weaponsGameobjects.Add(WeaponType.Dagger, daggerGameobject);
        weaponsGameobjects.Add(WeaponType.StoneSword, stoneSwordGameobject);

        //Colliders init
        weaponsColliders.Add(WeaponType.Dagger, daggerGameobject.GetComponentInChildren<BoxCollider>());
        weaponsColliders.Add(WeaponType.StoneSword, stoneSwordGameobject.GetComponentInChildren<BoxCollider>());
        foreach (var weapon in weaponsColliders)
        {
            weapon.Value.enabled = false;
        }
    }
}

public static class WeaponsBible
{
    public static Dictionary<WeaponType, string> WeaponsNames = new Dictionary<WeaponType, string>() 
    {
        { WeaponType.Dagger, "Dagger" },
        { WeaponType.StoneSword, "Stone sword" }
    };
    public static Dictionary<WeaponType, string> WeaponsDescriptions = new Dictionary<WeaponType, string>()
    {
        { WeaponType.Dagger, "A small dagger. Quick but has a short range." },
        { WeaponType.StoneSword, "A sword made of stone. Very heavy and not really sharp." }
    };
    public static Dictionary<WeaponType, int> WeaponsDamages = new Dictionary<WeaponType, int>()
    {
        { WeaponType.Dagger, 1 },
        { WeaponType.StoneSword, 1 }
    };
    public static Dictionary<WeaponType, float> WeaponsCooldowns = new Dictionary<WeaponType, float>()
    {
        { WeaponType.Dagger, 1f },
        { WeaponType.StoneSword, 1.2f }
    };
    public static Dictionary<WeaponType, WhooshType> WeaponsWhoosh = new Dictionary<WeaponType, WhooshType>()
    {
        { WeaponType.Dagger, WhooshType.Small },
        { WeaponType.StoneSword, WhooshType.Medium }
    };
}