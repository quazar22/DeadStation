using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    GameObject[] zombie_list;
    
    // Start is called before the first frame update
    void Start()
    {
        zombie_list = Resources.LoadAll<GameObject>("Prefabs/Zombies");
        StartCoroutine(BeginSpawnZombie());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BeginSpawnZombie()
    {
        while(true)
        {
            SpawnZombie();
            yield return new WaitForSeconds(5f);
        }
    }

    void SpawnZombie()
    {
        GameObject zombie = zombie_list[Random.Range(0, zombie_list.Length)];
        Instantiate(zombie, gameObject.transform.position, Quaternion.identity);
    }

}
