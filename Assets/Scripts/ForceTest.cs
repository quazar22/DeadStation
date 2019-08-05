﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceTest : MonoBehaviour
{
    private GameObject character;
    Vector3 center = Vector3.zero;

    private GameObject[] zombies;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("player/SpaceMan@Idle");
        //zombie = GameObject.Find("zombie0");
        zombies = GameObject.FindGameObjectsWithTag("zombie");
        foreach(GameObject z in zombies)
        {
            //foreach(Rigidbody rbb in z.GetComponentsInChildren<Rigidbody>())
            //{
            //    rbb.detectCollisions = false;
            //}
            foreach(Collider c in z.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(GameObject z in zombies)
            {
                z.GetComponent<CharacterDataController>().character.cc.enabled = false;
                Rigidbody[] rbb = z.GetComponentsInChildren<Rigidbody>();
                //foreach (Rigidbody rb in rbb)
                //{
                //    rb.detectCollisions = true;
                //}
                foreach (Collider c in z.GetComponentsInChildren<Collider>())
                {
                    c.enabled = true;
                }
                z.GetComponent<CharacterDataController>().character.nma.enabled = false;
                z.GetComponent<CharacterDataController>().character.anim.enabled = false;
                //rbb[0].AddExplosionForce(300f, zombies[6].transform.position, 100f, 1f, ForceMode.Impulse);
            }
        }
    }
}
