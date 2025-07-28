using System.Collections;
using UnityEngine;

public enum WeaponWhoosh
{
    Small
}

public class WeaponManager : MonoBehaviour
{
    public float damageCooldownMax;
    public float damageCooldown;

    PlayerManager playerManager;
    Animator animator;

    public GameObject sliceImpact;

    AudioSource audioSource;
    public AudioClip[] audio_whooshesSmall;
    public AudioClip[] audio_whooshesMedium;

    public WeaponType equippedWeapon;
    public WhooshType equippedWeaponWhoosh;
    
    public Collider weaponCollider;

    private void Start()
    {
        damageCooldown = 0f;
        damageCooldownMax = WeaponsBible.WeaponsCooldowns[equippedWeapon];

        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Damage recieved cooldown
        if (damageCooldown >= 0)
        {
            damageCooldown -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (damageCooldown <= 0)
        {
            animator.SetTrigger("Attack");
            damageCooldown = damageCooldownMax;
            weaponCollider.enabled = true;
            StartCoroutine(DisableWeaponCollider());
            PlayWhoosh();
        }
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

    IEnumerator DisableWeaponCollider()
    {
        yield return new WaitForSeconds(0.2f);
        weaponCollider.enabled = false;
    }

    public void SetWeaponCollider(Collider col)
    {
        weaponCollider = col;
    }
}
