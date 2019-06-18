using System;
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
    Renderer aim_cone;
    Color blue;
    Color red;

    void Start()
    {
        ColliderList = new List<Collider>();
        player = GameObject.Find(Character.PLAYER);
        wm = GetComponentInParent<WeaponManager>();
        fireposition = GameObject.Find("player/fire_position");
        try
        {
            aim_cone = GameObject.Find("player/aim_cone_blue").GetComponent<Renderer>();
        } catch(NullReferenceException e)
        {
            aim_cone = GameObject.Find("player/aim_cone_blue_dotted").GetComponent<Renderer>();
        }
        blue = new Color
        {
            r = 0f,
            g = 0.1176f,
            b = 1f,
            a = 0.4705f
        };
        red = new Color
        {
            r = 1f,
            g = 0.1176f,
            b = 0f,
            a = 0.4705f
        };
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
                if (hit.collider.tag.StartsWith("wall"))
                {
                    return;
                }
            }
        }

        wm.FireWeapon();
    }

    private void SetAimConeToBlue()
    {
        aim_cone.material.color = blue;
    }

    private void SetAimConeToRed()
    {
        aim_cone.material.color = red;

    }

    private void FixedUpdate()
    {
        if (ColliderList.Count > 0)
        {
            SetAimConeToRed();
        }
        else
        {
            SetAimConeToBlue();
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

    public void RemoveFromList(Collider c)
    {
        ColliderList.Remove(c);
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
