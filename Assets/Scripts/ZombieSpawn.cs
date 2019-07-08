using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    static GameObject[] zombie_prefab_list = null;
    static List<Material> skin_material_list = null;
    static List<GameObject> alive_zombies;
    float SpawnSpeed = 5f;

    public bool shouldSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        //get list of all acceptable skin materials to choose from
        //randomly choose one of those at runtime
        if(zombie_prefab_list == null && skin_material_list == null)
        {
            alive_zombies = new List<GameObject>();
            skin_material_list = new List<Material>();
            zombie_prefab_list = Resources.LoadAll<GameObject>("Prefabs/Zombies");
            for(int i = 0; i < zombie_prefab_list.Length; i++)
            {
                Renderer[] rendererList = zombie_prefab_list[i].GetComponentsInChildren<Renderer>();
                foreach(Renderer r in rendererList)
                {
                    if(r.name.Contains("white") || r.name.Contains("black"))
                    {
                        skin_material_list.Add(r.sharedMaterial);
                    }
                }
            }
        }
        StartCoroutine("BeginSpawnZombie");
    }

    public void SetSpawnSpeed(float speed)
    {
        SpawnSpeed = speed;
    }

    public void StopSpawning()
    {
        StopCoroutine("BeginSpawnZombie");
    }

    IEnumerator BeginSpawnZombie()
    {
        while(shouldSpawn && !(Time.fixedTime > 5f))
        {
            SpawnZombie();
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        
    }

    void SpawnZombie()
    {
        GameObject zombie = zombie_prefab_list[Random.Range(0, zombie_prefab_list.Length)];
        zombie = Instantiate(zombie, gameObject.transform.position, Quaternion.identity);

        Renderer[] r = zombie.GetComponentsInChildren<Renderer>();

        Material m = new Material(r[1].sharedMaterial);
        m.color = new Color() {r = Random.Range(0f,0.5f), a = Random.Range(0.5f, 1f), b = Random.Range(0f, 0.5f), g = Random.Range(0f, 0.5f) };

        //skin material is located in first [0] component under the character (in the zombie prefab)
        //clothing material is located in second [1] component 
        r[0].sharedMaterial = skin_material_list[Random.Range(0, skin_material_list.Count)];
        r[1].sharedMaterial = m;
        //alive_zombies.Add(zombie);
    }

}
