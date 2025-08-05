using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagicManager : MonoBehaviour
{
    public float damageCooldown;
    public float damageCooldownMax;

    public MagicData magicData;
    PlayerManager playerManager;
    Animator animator;

    AudioSource audioSource;

    public MagicType equippedMagic;
    ParticleSystem currentMagicIdleParticles;
    Light currentMagicLight;
    float baseIntLightIntesity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageCooldown = 0f;
        damageCooldownMax = MagicBible.MagicCooldowns[equippedMagic];

        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damageCooldown > 0f)
        {
            damageCooldown -= Time.deltaTime;
        }
    }
    
    public void Magic()
    {
        if (damageCooldown <= 0f)
        {
            if (playerManager.mana >= MagicBible.MagicManaCost[equippedMagic])
            {
                damageCooldown = damageCooldownMax;
                animator.SetTrigger("Magic");
                StartCoroutine(DisableIdleParticles(damageCooldownMax));
                audioSource.Play();
                switch (equippedMagic)
                {
                    case MagicType.SparkleScepter:
                        Instantiate(magicData.magicProjectiles[equippedMagic], transform.position, Quaternion.identity).GetComponent<Rigidbody>().linearVelocity = transform.forward * 8;
                        break;
                    case MagicType.RubyScepter:
                        for (int i = -1; i <= 1; i++)
                        {
                            Vector3 direction = Quaternion.Euler(0, i * 1f, 0) * transform.forward;

                            Instantiate(magicData.magicProjectiles[equippedMagic], transform.position, Quaternion.identity).
                                GetComponent<Rigidbody>().linearVelocity = direction.normalized * 10f;
                        }
                        break;
                }
                playerManager.mana -= MagicBible.MagicManaCost[equippedMagic];
                playerManager.UpdateManaBar();
            }
        }
    }

    public void SetMagicsVars()
    {
        currentMagicIdleParticles = magicData.magicGameobjects[equippedMagic].GetComponent<ParticleSystem>();
        currentMagicLight = magicData.magicGameobjects[equippedMagic].GetComponentInChildren<Light>();
        baseIntLightIntesity = currentMagicLight.intensity;
        audioSource.clip = magicData.magicAudio[equippedMagic];
    }

    IEnumerator DisableIdleParticles(float t)
    {
        currentMagicIdleParticles.Stop();
        currentMagicLight.enabled = false;
        yield return new WaitForSeconds(t);
        currentMagicIdleParticles.Play();
        currentMagicLight.enabled = true;
        StartCoroutine(FadeInLights());

    }

    IEnumerator FadeInLights()
    {
        currentMagicLight.intensity = 0;
        for (float i = 1; i <= 5; i++)
        {
            currentMagicLight.intensity = (baseIntLightIntesity / 5) * i;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
