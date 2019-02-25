using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    GameObject player;
    static List<Collider> ColliderList;
    static Collider ClosestCollider = null;
    static List<Collider> ignoreList = null;
    WeaponManager wm;

    void Start()
    {
        ColliderList = new List<Collider>();
        ignoreList = new List<Collider>();
        //weapon_list = new List<Weapon>(new Weapon[] { new AutoRifle(), new Shotgun(), new LaserCannon(), new GrenadeLauncher() });
        player = GameObject.Find(Character.char_names[1]);
        wm = GetComponentInParent<WeaponManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ColliderList.Add(other);
    }

    public Collider GetClosestCollider()
    {
        return ClosestCollider;
    }

    private void OnTriggerStay(Collider other)
    {
        ClosestCollider = ColliderList[0];

        foreach (Collider c in ColliderList) //find closest collider
        {

            if (Vector3.Distance(player.transform.position, c.transform.position) < Vector3.Distance(player.transform.position, ClosestCollider.transform.position))
            {
                ClosestCollider = c;
            }
        }

        Debug.DrawLine(player.transform.position, ClosestCollider.transform.position, Color.red);
        Character cdc = ClosestCollider.GetComponent<CharacterDataController>().character; ;
        wm.FireWeapon();
        //change weapon firing mechanism :(

        //if (cdc.GetHealth() <= 0)
        //{
        //    ColliderList.Remove(ClosestCollider);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderList.Remove(other);
        if(ColliderList.Count == 0)
        {
            ClosestCollider = null;
        }
    }
}
