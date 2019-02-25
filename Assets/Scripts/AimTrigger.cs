using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    GameObject player;
    static List<Collider> ColliderList;
    static Collider ClosestCollider = null;
    WeaponManager wm;

    void Start()
    {
        ColliderList = new List<Collider>();
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

    public List<Collider> GetColliderList()
    {
        return ColliderList;
    }

    private void OnTriggerStay(Collider other)
    {
        bool broken = false;
        Collider tmpCollider = null;

        if (ColliderList.Count > 0)
            ClosestCollider = ColliderList[0];
        else
            return;

        foreach (Collider c in ColliderList) //find closest collider
        {
            //if(c == null) { ColliderList.Remove(c); }
            if(c == null)
            {
                tmpCollider = c;
                broken = true;
                break;
            }
            if (Vector3.Distance(player.transform.position, c.transform.position) < Vector3.Distance(player.transform.position, ClosestCollider.transform.position))
            {
                ClosestCollider = c;
            }
            if(c.gameObject.GetComponent<CharacterDataController>().character.health <= 0 && ColliderList.Contains(c))
            {
                tmpCollider = c;
                broken = true;
                break;
            }
        }

        if(broken)
        {
            ColliderList.Remove(tmpCollider);
            return;
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
