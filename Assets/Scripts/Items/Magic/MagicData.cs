using System.Collections.Generic;
using UnityEngine;

public enum MagicType
{
    SparkleScepter = 0,
    RubyScepter = 1
}

public class MagicData : MonoBehaviour
{
    public Dictionary<MagicType, Sprite> magicSprites = new Dictionary<MagicType, Sprite>();
    public Sprite sparkleScepterSprite;
    public Sprite rubyScepterSprite;

    public Dictionary<MagicType, GameObject> magicGameobjects = new Dictionary<MagicType, GameObject>();
    public GameObject sparkleScepterGameobject;
    public GameObject rubyScepterGameobject;

    public Dictionary<MagicType, GameObject> magicProjectiles = new Dictionary<MagicType, GameObject>();
    public GameObject sparkleScepterProjectile;
    public GameObject rubyScepterProjectile;

    public Dictionary<MagicType, AudioClip> magicAudio = new Dictionary<MagicType, AudioClip>();
    public AudioClip sparkleScepterAudio;
    public AudioClip rubyScepterAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        magicSprites.Add(MagicType.SparkleScepter, sparkleScepterSprite);
        magicSprites.Add(MagicType.RubyScepter, rubyScepterSprite);

        magicGameobjects.Add(MagicType.SparkleScepter, sparkleScepterGameobject);
        magicGameobjects.Add(MagicType.RubyScepter, rubyScepterGameobject);

        magicProjectiles.Add(MagicType.SparkleScepter, sparkleScepterProjectile);
        magicProjectiles.Add(MagicType.RubyScepter, rubyScepterProjectile);

        magicAudio.Add(MagicType.SparkleScepter, sparkleScepterAudio);
        magicAudio.Add(MagicType.RubyScepter, rubyScepterAudio);
    }
}

public static class MagicBible
{
    public static Dictionary<MagicType, string> MagicNames = new Dictionary<MagicType, string>()
    {
        { MagicType.SparkleScepter, "Sparkle scepter" },
        { MagicType.RubyScepter, "Ruby scepter" }
    };
    public static Dictionary<MagicType, string> MagicDescriptions = new Dictionary<MagicType, string>()
    {
        { MagicType.SparkleScepter, "So old it's barely imbued anymore. Shoots faints sparkles." },
        { MagicType.RubyScepter, "Used by battlemages apprentices to support front lines." }
    };
    public static Dictionary<MagicType, float> MagicCooldowns = new Dictionary<MagicType, float>()
    {
        { MagicType.SparkleScepter, 3f },
        { MagicType.RubyScepter, 5f }
    };
    public static Dictionary<MagicType, int> MagicProjectilesDamages = new Dictionary<MagicType, int>()
    {
        { MagicType.SparkleScepter, 2 },
        { MagicType.RubyScepter, 1 }
    };
    public static Dictionary<MagicType, int> MagicManaCost = new Dictionary<MagicType, int>()
    {
        { MagicType.SparkleScepter, 1},
        { MagicType.RubyScepter, 2 }
    };
}

