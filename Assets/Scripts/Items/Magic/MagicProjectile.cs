using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public MagicType magicType;
    int damage;

    private void Awake()
    {
        damage = MagicBible.MagicProjectilesDamages[magicType];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage, true);
            Destructe();
        }
        else
        if (collision.transform.CompareTag("Breakable"))
        {
            collision.gameObject.GetComponent<Breakable>().Break();
            Destructe();
        }
        else
        if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("MagicProjectile"))
        {
            Destructe();
        }
    }

    public void Destructe()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject, 0.5f);
    }
}
