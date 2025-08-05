using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Dagger = 0,
    RustySword = 1,
    Hatchet = 2,
    Rapier = 3
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
    public Sprite hatchetSprite;
    public Sprite rapierSprite;

    public Dictionary<WeaponType, GameObject> weaponsGameobjects = new Dictionary<WeaponType, GameObject>();
    public GameObject daggerGameobject;
    public GameObject stoneSwordGameobject;
    public GameObject hatchetGameobject;
    public GameObject rapierGameobject;

    public Dictionary<WeaponType, BoxCollider> weaponsColliders = new Dictionary<WeaponType, BoxCollider>();

    public Dictionary<DamageType, AudioClip[]> EnemyDamageAudioClips = new Dictionary<DamageType, AudioClip[]>();
    public AudioClip[] EnemyDamageAudioClips_Blunt;
    public AudioClip[] EnemyDamageAudioClips_Slash;
    public AudioClip[] EnemyDamageAudioClips_Magic;

    private void Awake()
    {
        //Sprite list init
        weaponSprites.Add(WeaponType.Dagger, daggerSprite);
        weaponSprites.Add(WeaponType.RustySword, stoneSwordSprite);
        weaponSprites.Add(WeaponType.Hatchet, hatchetSprite);
        weaponSprites.Add(WeaponType.Rapier, rapierSprite);

        //Weapon gamobjects list init
        weaponsGameobjects.Add(WeaponType.Dagger, daggerGameobject);
        weaponsGameobjects.Add(WeaponType.RustySword, stoneSwordGameobject);
        weaponsGameobjects.Add(WeaponType.Hatchet, hatchetGameobject);
        weaponsGameobjects.Add(WeaponType.Rapier, rapierGameobject);

        //Colliders list init
        weaponsColliders.Add(WeaponType.Dagger, daggerGameobject.GetComponentInChildren<BoxCollider>());
        weaponsColliders.Add(WeaponType.RustySword, stoneSwordGameobject.GetComponentInChildren<BoxCollider>());
        weaponsColliders.Add(WeaponType.Hatchet, hatchetGameobject.GetComponentInChildren<BoxCollider>());
        weaponsColliders.Add(WeaponType.Rapier, rapierGameobject.GetComponentInChildren<BoxCollider>());
        foreach (var weapon in weaponsColliders)
        {
            weapon.Value.enabled = false;
        }

        //Enemy attack sounds list init
        EnemyDamageAudioClips.Add(DamageType.Blunt, EnemyDamageAudioClips_Blunt);
        EnemyDamageAudioClips.Add(DamageType.Slash, EnemyDamageAudioClips_Slash);
        EnemyDamageAudioClips.Add(DamageType.Magic, EnemyDamageAudioClips_Magic);
    }
}

public static class WeaponsBible
{
    public static Dictionary<WeaponType, string> WeaponsNames = new Dictionary<WeaponType, string>() 
    {
        { WeaponType.Dagger, "Dagger" },
        { WeaponType.RustySword, "Rusty sword" },
        { WeaponType.Hatchet, "Hatchet" },
        { WeaponType.Rapier, "Rapier" }
    };
    public static Dictionary<WeaponType, string> WeaponsDescriptions = new Dictionary<WeaponType, string>()
    {
        { WeaponType.Dagger, "Quick but has a short range." },
        { WeaponType.RustySword, "Very heavy and not really sharp." },
        { WeaponType.Hatchet, "Chops bones as wood." },
        { WeaponType.Rapier, "Great to pierce but ineffective versus armors." }
    };
    public static Dictionary<WeaponType, int> WeaponsDamages = new Dictionary<WeaponType, int>()
    {
        { WeaponType.Dagger, 1 },
        { WeaponType.RustySword, 1 },
        { WeaponType.Hatchet, 2 },
        { WeaponType.Rapier, 2 }
    };
    public static Dictionary<WeaponType, float> WeaponsCooldowns = new Dictionary<WeaponType, float>()
    {
        { WeaponType.Dagger, 1f },
        { WeaponType.RustySword, 1.2f },
        { WeaponType.Hatchet, 1f },
        { WeaponType.Rapier, 0.9f }
    };
    public static Dictionary<WeaponType, float> WeaponsColliderEnableTimes = new Dictionary<WeaponType, float>()
    {
        { WeaponType.Dagger, 0.15f },
        { WeaponType.RustySword, 0.2f },
        { WeaponType.Hatchet, 0.3f },
        { WeaponType.Rapier, 0.15f }
    };

    public static Dictionary<WeaponType, WhooshType> WeaponsWhoosh = new Dictionary<WeaponType, WhooshType>()
    {
        { WeaponType.Dagger, WhooshType.Small },
        { WeaponType.RustySword, WhooshType.Medium },
        { WeaponType.Hatchet, WhooshType.Small },
        { WeaponType.Rapier, WhooshType.Small }
    };
    public static Dictionary<WeaponType, int> weaponsStamina = new Dictionary<WeaponType, int>()
    {
        { WeaponType.Dagger, 1 },
        { WeaponType.RustySword, 3 },
        { WeaponType.Hatchet, 2 },
        { WeaponType.Rapier, 2 }
    };
}