using System.Collections;
using UnityEngine;

public enum BreakableTypes
{
    Pot
}

public class Breakable : MonoBehaviour
{
    public BreakableTypes breakableType;
    public AudioClip potBreak;
    AudioSource audioSource;

    bool hasBroken;
    public bool isThrown;

    [Header("Loot")]
    public GameObject coins;


    Enemy n;
    Breakable b;

    private void Awake()
    {
        hasBroken = false;
        isThrown = false;
        audioSource = GetComponent<AudioSource>();
        switch (breakableType)
        {
            case BreakableTypes.Pot:
                audioSource.clip = potBreak;
                break;
        }
    }

    public void Break()
    {
        if (!hasBroken)
        {
            hasBroken = true;
            GetComponent<SphereCollider>().center = new Vector3(0, 0.1f, 0);
            GetComponent<Animator>().SetTrigger("Break");
            audioSource.Play();
            StartCoroutine(DisablePhysics());
            gameObject.tag = "Untagged";

            float rand = Random.value;
            if (rand >= 0.9f)
            {
                Instantiate(coins, transform.position, transform.rotation).GetComponent<Coin>().SetAmount(Random.Range(11, 21));
            }
            else
            if (rand >= 0.5f)
            {
                Instantiate(coins, transform.position, transform.rotation).GetComponent<Coin>().SetAmount(Random.Range(1, 11));
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!hasBroken && isThrown)
        {
            Break();
            if (collision.gameObject.TryGetComponent<Enemy>(out n))
            {
                n.Damage(3, true);
            }
            else
            if (collision.gameObject.TryGetComponent<Breakable>(out b))
            {
                b.Break();
            }
        }
    }

    IEnumerator DisablePhysics()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
