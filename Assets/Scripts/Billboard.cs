using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        transform.forward = player.forward;
    }
}
