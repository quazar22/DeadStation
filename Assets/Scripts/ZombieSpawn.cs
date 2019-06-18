using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    List<GameObject> spawns;
    GameObject zombie;
    // Start is called before the first frame update
    void Start()
    {
        spawns = new List<GameObject>();
        spawns.Add(GameObject.Find("ZombieSpawn"));
        spawns.Add(GameObject.Find("ZombieSpawn1"));
        spawns.Add(GameObject.Find("ZombieSpawn2"));
        spawns.Add(GameObject.Find("ZombieSpawn3"));
        zombie = Resources.Load<GameObject>("Prefabs/Characters/zombie");
        StartCoroutine(SpawnZombie());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnZombie()
    {
        while(true)
        {
            ///Instantiate(zombie, spawns[Random.Range(0, 3)].transform.position, Quaternion.identity);
            Instantiate(zombie, spawns[1].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }

}
