using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] loot;

    public void OpenChest()
    {
        GetComponent<Animator>().SetTrigger("Open");
        GetComponent<AudioSource>().Play();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(SpawnLoot());
    }

    IEnumerator SpawnLoot()
    {
        yield return new WaitForSeconds(0.4f);
        foreach (GameObject item in loot)
        {
            float dirX = Random.Range(-1f, -2f);
            float dirZ = Random.Range(-2f, 2f);
            Instantiate(item, transform.position, Quaternion.identity).GetComponent<Rigidbody>().linearVelocity =
                Vector3.up * 3 + (Vector3.right * dirX) + (Vector3.forward * dirZ);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
