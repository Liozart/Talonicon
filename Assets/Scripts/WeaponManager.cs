using UnityEngine;

public enum WeaponWhoosh
{
    Small
}

public class WeaponManager : MonoBehaviour
{
    PlayerManager playerManager;

    public GameObject sliceImpact;

    AudioSource audioSource;
    public AudioClip[] audio_whooshesSmall;
    public AudioClip[] audio_whooshesMedium;

    public WeaponType equippedWeapon;
    public WhooshType equippedWeaponWhoosh;

    private void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWhoosh()
    {
        if (equippedWeaponWhoosh == WhooshType.Small)
            audioSource.clip = audio_whooshesSmall[Random.Range(0, audio_whooshesSmall.Length)];
        else
        if (equippedWeaponWhoosh == WhooshType.Medium)
            audioSource.clip = audio_whooshesMedium[Random.Range(0, audio_whooshesMedium.Length)];

        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerManager.WeaponDamage(other.GetComponent<Enemy>());
            Destroy(Instantiate(sliceImpact, other.ClosestPoint(transform.position), Quaternion.identity), 0.5f);
        }
    }
}
