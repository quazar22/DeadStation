using System;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    GameObject player;
    List<Collider> ColliderList;
    Collider ClosestCollider = null;
    WeaponManager wm;
    GameObject fireposition;


    void Start()
    {
        ColliderList = new List<Collider>();
        player = GameObject.Find(Character.char_names[1]);
        wm = GetComponentInParent<WeaponManager>();
        fireposition = GameObject.Find("fire_position");

        //StartCoroutine(FindClosestCollider());
    }

    void Update()
    {
        bool broken = false;
        Collider tmpCollider = null;

        if (ColliderList.Count > 0)
            ClosestCollider = ColliderList[0];
        else
            return;

        foreach (Collider c in ColliderList) //find closest collider
        {
            //if (c == null)
            //{
            //    tmpCollider = c;
            //    broken = true;
            //    break;
            //}
            if (c == null || c.gameObject.GetComponent<CharacterDataController>().character.health <= 0 && ColliderList.Contains(c))
            {
                tmpCollider = c;
                broken = true;
                break;
            }
            if (Vector3.Distance(player.transform.position, c.transform.position) < Vector3.Distance(player.transform.position, ClosestCollider.transform.position))
            {
                ClosestCollider = c;
            }
            
        }

        if (broken)
        {
            ColliderList.Remove(tmpCollider);
            return;
        }

        Debug.DrawLine(fireposition.transform.position, ClosestCollider.transform.position, Color.red);

        RaycastHit hit;
        if (Physics.Linecast(fireposition.transform.position, ClosestCollider.transform.position, out hit))
        {
            if (hit.collider)
            {
                //Debug.Log("hit: " + hit.collider.tag);
                if (hit.collider.tag.StartsWith("wall"))
                {
                    return;
                }
            }
        }

        wm.FireWeapon();
    }

    private void FixedUpdate()
    {
        
    }

   void RaycastEnemies()
    {
        Debug.DrawLine(fireposition.transform.position, ClosestCollider.transform.position, Color.red);

        RaycastHit hit;
        if (Physics.Linecast(fireposition.transform.position, ClosestCollider.transform.position, out hit))
        {
            if (hit.collider)
            {
                Debug.Log("hit: " + hit.collider.tag);
                if (hit.collider.tag.StartsWith("wall"))
                {
                    return;
                }
            }
        }
    }

    public Collider GetClosestCollider()
    {
        return ClosestCollider;
    }

    public List<Collider> GetColliderList()
    {
        return ColliderList;
    }

    void FindClosestCollider()
    {
        bool broken = false;
        Collider tmpCollider = null;

        if (ColliderList.Count > 0)
            ClosestCollider = ColliderList[0];
        else
            return;

        if (!ClosestCollider) { return; }

        foreach (Collider c in ColliderList) //find closest collider
        {
            if (c == null)
            {
                tmpCollider = c;
                broken = true;
                break;
            }
            if (Vector3.Distance(player.transform.position, c.transform.position) < Vector3.Distance(player.transform.position, ClosestCollider.transform.position))
            {
                ClosestCollider = c;
            }
            if (c.gameObject.GetComponent<CharacterDataController>().character.health <= 0 && ColliderList.Contains(c))
            {
                tmpCollider = c;
                broken = true;
                break;
            }
        }

        if (broken)
        {
            ColliderList.Remove(tmpCollider);
            return;
        }

        Debug.DrawLine(fireposition.transform.position, ClosestCollider.transform.position, Color.red);

        RaycastHit hit;
        if (Physics.Linecast(fireposition.transform.position, ClosestCollider.transform.position, out hit))
        {
            if (hit.collider)
            {
                Debug.Log("hit: " + hit.collider.tag);
                if (hit.collider.tag.StartsWith("wall"))
                {
                    return;
                }
            }
        }

        wm.FireWeapon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("wall")) { return; }
        ColliderList.Add(other);

        if (ColliderList.Count == 1)
        {
            ClosestCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ColliderList.Remove(other);
    }
}
