﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    //static GameObject[] zombie_prefab_list = null;
    internal static List<ZombieTextureData> zombie_texture_data = null;
    float SpawnSpeed = 5f;
    static int zombieCount = 0;

    public bool shouldSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        //for each zombie, make possible skin array for that zombie, then make possible clothing array for it too
        if (zombie_texture_data == null)
        {
            zombie_texture_data = new List<ZombieTextureData>();
            GameObject[] zombie_prefabs = Resources.LoadAll<GameObject>("Prefabs/Zombies");
            Material[] mats = Resources.LoadAll<Material>("Materials/ZombieMaterials");

            for(int i = 0; i < zombie_prefabs.Length; i++)
            {
                zombie_texture_data.Add(new ZombieTextureData(zombie_prefabs[i]));
            }

            for(int i = 0; i < mats.Length; i++)
            {
                string s = mats[i].name;
                if (s.StartsWith("skin"))
                {
                    ZombieTextureData.AddSkin(mats[i]); //all skins work for every model, so just make a static variable for it
                }
                else if (s.StartsWith("clothing"))
                {
                    int index = int.Parse(s.Substring(s.Length - 1));
                    zombie_texture_data[index].AddClothing(mats[i]);
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
        while(shouldSpawn && !(Time.fixedTime > 10f))
        {
            SpawnZombie();
            zombieCount++;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        //if (Time.time > 10f && Time.time < 11f)
        //    UnityEngine.Debug.Log(zombieCount);
    }

    void SpawnZombie()
    {
        ZombieTextureData zombie = zombie_texture_data[Random.Range(0, zombie_texture_data.Count)];
        GameObject zombie_obj = Instantiate(zombie.GetZombieObject(), gameObject.transform.position, Quaternion.identity);

        Renderer[] r = zombie_obj.GetComponentsInChildren<Renderer>();
        r[0].sharedMaterial = ZombieTextureData.skins[Random.Range(0, ZombieTextureData.skins.Count)];
        r[1].sharedMaterial = zombie.clothes[Random.Range(0, zombie.clothes.Count)];

        Zombie z = (Zombie)zombie_obj.GetComponent<CharacterDataController>().character;

        //Material m = new Material(zombie.clothes[Random.Range(0, zombie.clothes.Count)])
        //{
        //    color = new Color() { r = Random.Range(0.5f, 1f), a = Random.Range(0.5f, 1f), b = Random.Range(0.5f, 1f), g = Random.Range(0.5f, 1f) }
        //};

        z.rigidbodies = zombie_obj.GetComponentsInChildren<Rigidbody>();
        z.colliders = zombie_obj.GetComponentsInChildren<Collider>();

        foreach (Rigidbody rb in z.rigidbodies)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        foreach (Collider c in z.colliders)
        {
            if(!(c is CharacterController))
                c.enabled = false;
        }

    }

    public class ZombieTextureData
    {

        public ZombieTextureData(GameObject zombie)
        {
            this.zombie = zombie;
            clothes = new List<Material>();
            if (skins == null)
                skins = new List<Material>();
        }

        public static void AddSkin(Material skin)
        {
            skins.Add(skin);
        }

        public void AddClothing(Material clothing)
        {
            clothes.Add(clothing);
        }

        public GameObject GetZombieObject()
        {
            return zombie;
        }

        public GameObject zombie;
        public List<Material> clothes;
        public static List<Material> skins = null;
    }

}
