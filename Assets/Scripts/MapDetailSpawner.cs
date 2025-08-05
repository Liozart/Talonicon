using UnityEngine;

public class MapDetailSpawner : MonoBehaviour
{
    public GameObject map;
    public GameObject baseDetailGameObject;
    public Sprite[] details_Grass;
    public Sprite[] details_Flowers;
    public Sprite[] details_Rocks;

    public PhysicsMaterial pmaterial_Grass;
    public PhysicsMaterial pmaterial_Rocks;


    void Start()
    {
        MeshCollider currentCube;
        GameObject spawn;
        float res;
        float res2;
        int turns;
        int turns2;
        RaycastHit hit;

        for (int i = 0; i < map.transform.childCount; i++)
        {
            res = Random.value;

            if (res <= 0.7f)
                turns = 0;
            else
            if (res <= 0.8f)
                turns = 1;
            else
            if (res <= 0.9f)
                turns = 2;
            else
                turns = 3;

            //Spawn grass
            if (turns == 0)
            {
                if (map.transform.GetChild(i).GetComponent<MeshCollider>().sharedMaterial == pmaterial_Grass)
                {
                    res2 = Random.value;

                    if (res2 <= 0.5f)
                        turns2 = 4;
                    else
                    if (res2 <= 0.8f)
                        turns2 = 6;
                    else
                    if (res2 <= 0.9f)
                        turns2 = 9;
                    else
                        turns2 = 12;

                    for (int j = 0; j < turns2; j++)
                    {
                        currentCube = map.transform.GetChild(i).GetComponent<MeshCollider>();
                        Vector3 basePos = new Vector3(currentCube.transform.position.x + Random.Range(-(currentCube.bounds.size.x / 2), (currentCube.bounds.size.x / 2)),
                            currentCube.transform.position.y + currentCube.bounds.size.y,
                            currentCube.transform.position.z + Random.Range(-(currentCube.bounds.size.z / 2), (currentCube.bounds.size.z / 2)));

                        Physics.Raycast(basePos, Vector3.down, out hit);
                        basePos.y = hit.point.y;

                        spawn = Instantiate(baseDetailGameObject, basePos, Quaternion.identity);

                        spawn.GetComponent<SpriteRenderer>().sprite = details_Grass[Random.Range(0, details_Grass.Length)];
                        if (Random.value > 0.5f)
                            spawn.GetComponent<SpriteRenderer>().flipX = true;
                        spawn.transform.position = new Vector3(spawn.transform.position.x,
                            spawn.transform.position.y + (spawn.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2),
                            spawn.transform.position.z);
                        spawn.transform.SetParent(currentCube.transform);
                    }
                }
            }
            else
            {
                for (int j = 0; j < turns; j++)
                {
                    currentCube = map.transform.GetChild(i).GetComponent<MeshCollider>();
                    Vector3 basePos = new Vector3(currentCube.transform.position.x + Random.Range(-(currentCube.bounds.size.x / 2), (currentCube.bounds.size.x / 2)),
                            currentCube.transform.position.y + currentCube.bounds.size.y,
                            currentCube.transform.position.z + Random.Range(-(currentCube.bounds.size.z / 2), (currentCube.bounds.size.z / 2)));

                    Physics.Raycast(basePos, Vector3.down, out hit);
                    basePos.y = hit.point.y;

                    spawn = Instantiate(baseDetailGameObject, basePos, Quaternion.identity);
                    if (map.transform.GetChild(i).GetComponent<MeshCollider>().sharedMaterial == pmaterial_Grass)
                    {
                        spawn.GetComponent<SpriteRenderer>().sprite = details_Flowers[Random.Range(0, details_Flowers.Length)];
                    }
                    else
                    if (map.transform.GetChild(i).GetComponent<MeshCollider>().sharedMaterial == pmaterial_Rocks)
                    {
                        spawn.GetComponent<SpriteRenderer>().sprite = details_Rocks[Random.Range(0, details_Rocks.Length)];
                    }
                    if (Random.value > 0.5f)
                        spawn.GetComponent<SpriteRenderer>().flipX = true;
                    spawn.transform.position = new Vector3(spawn.transform.position.x,
                        spawn.transform.position.y + (spawn.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2),
                        spawn.transform.position.z);
                    spawn.transform.SetParent(currentCube.transform);
                }
            }
        }
    }
}
