using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    GameObject player;
    List<Collider> ColliderList;
    // Start is called before the first frame update
    void Start()
    {
        ColliderList = new List<Collider>();
        player = GameObject.Find(Character.char_names[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ColliderList.Add(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Collider ClosestCollider = ColliderList[0];

        foreach (Collider c in ColliderList) //find closest collider
        {

            if (Vector3.Distance(player.transform.position, c.transform.position) < Vector3.Distance(player.transform.position, ClosestCollider.transform.position))
            {
                ClosestCollider = c;
            }
        }

        Debug.DrawLine(player.transform.position, ClosestCollider.transform.position, Color.red);
        Character cdc = ClosestCollider.GetComponent<CharacterDataController>().character;
        cdc.DamageCharacter(1);

        if (cdc.GetHealth() <= 0)
        {
            ColliderList.Remove(ClosestCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderList.Remove(other);
    }

}
