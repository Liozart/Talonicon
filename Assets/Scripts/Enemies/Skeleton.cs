using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Skeleton : Enemy
{
    NavMeshAgent agent;
    GameObject player;
    PlayerManager playerManager;
    Animator animator;
    AudioSource audioSource;

    bool isWalking = false;

    int playerMask = 1 << 6;

    public AudioClip[] audio_roars;
    public AudioClip[] audio_impacts;
    bool raorCoroutineStarted;

    void Start()
    { 
        health = 3;
        power = 1;
        xp = 1; 

        chaseRadius = 8f;
        attackCooldown = 0;
        attackCooldownMax = 1f;
        onAttackRange = false;

        hitCooldown = 0f;
        hitCooldownMax = 0.5f;

        checkForPlayerCooldown = 0;
        checkForPlayerCooldownMax = 0.5f;

        raorCoroutineStarted = false;

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.forward = player.transform.forward;

        if (health <= 0)
            return;

        if (isWalking)
        {
            agent.SetDestination(player.transform.position);
        }

        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            if (onAttackRange)
            {
                animator.SetTrigger("Attack");
                attackCooldown = attackCooldownMax;
                agent.isStopped = true;
                isWalking = false;
                playerManager.TakeDamage(power);
            }
            else
            {
                if (checkForPlayerCooldown >= 0)
                {
                    checkForPlayerCooldown -= Time.deltaTime;
                }
                else
                {
                    checkForPlayerCooldown = checkForPlayerCooldownMax;
                    if (Physics.CheckSphere(transform.position, chaseRadius, playerMask))
                    {
                        isWalking = true;
                        agent.isStopped = false;

                        if (!raorCoroutineStarted)
                            StartCoroutine(RandomRoad());
                    }
                    else
                    {
                        isWalking = false;
                        agent.isStopped = true;
                    }
                }
            }
        }

        if (hitCooldown >= 0)
        {
            hitCooldown -= Time.deltaTime;
        }

        animator.SetBool("Walk", isWalking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onAttackRange = false;
        }
    }

    public override void Damage(int dam)
    {
        if (hitCooldown <= 0)
        {
            health -= dam;
            hitCooldown = hitCooldownMax; 
            damageNumber.Spawn(new Vector3(transform.position.x, transform.position.y, transform.position.z), dam);
            audioSource.clip = audio_impacts[Random.Range(0, audio_impacts.Length)];
            audioSource.Play();

            if (health <= 0)
            {
                playerManager.WinXP(xp);
                animator.SetTrigger("Death");
                isWalking = false;
                onAttackRange = false;
                agent.isStopped = true;
                Destroy(gameObject, 0.5f);
            }
            animator.SetTrigger("Hit");
        }
    }

    IEnumerator RandomRoad()
    {
        raorCoroutineStarted = true;
        if (isWalking)
        {
            audioSource.clip = audio_roars[Random.Range(0, audio_roars.Length)];
            audioSource.Play();
        }
        yield return new WaitForSeconds(Random.Range(3, 10));
        if (isWalking)
        {
            StartCoroutine(RandomRoad());
        }
        else
            raorCoroutineStarted = false;
    }
}
