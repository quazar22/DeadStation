using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    GameObject player;
    Character character;

    List<Collider> ColliderList;
    Collider ClosestCollider = null;
    WeaponManager wm;
    GameObject top;
    Renderer aim_cone;
    Color blue;
    Color red;

    void Start()
    {
        ColliderList = new List<Collider>();
        player = GameObject.Find(Character.PLAYER);
        character = player.GetComponent<CharacterDataController>().character;

        wm = GetComponentInParent<WeaponManager>();
        top = GameObject.Find("player/top");
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


        RaycastHit hit;
        if (Physics.Linecast(top.transform.position, ClosestCollider.transform.position, out hit))
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
            if (ClosestCollider == null)
                return;
            RaycastHit hit;
            if(Physics.Linecast(top.transform.position, ClosestCollider.transform.position, out hit))
            {
                if(!hit.collider.tag.StartsWith("wall"))
                {
                    SetAimConeToRed();
                    //character.anim.SetInteger("UpperBodyAnimState", wm.GetCurrentWeapon().recoilCount);
                    BeginShooting();
                } else
                {
                    SetAimConeToBlue();
                    //character.anim.SetInteger("UpperBodyAnimState", 0);
                    ResetToIdleAnim();
                }
            }
        }
        else
        {
            SetAimConeToBlue();
            //character.anim.SetInteger("UpperBodyAnimState", 0);
            ResetToIdleAnim();
        }
    }

    public void BeginShooting()
    {
        if(character.anim.GetInteger("UpperBodyAnimState") == 3 && character.AnimPlayTime.ElapsedMilliseconds > 0.15f * 1000f)
            character.anim.SetInteger("UpperBodyAnimState", wm.GetCurrentWeapon().recoilCount);
        else if(character.anim.GetInteger("UpperBodyAnimState") == 0)
            character.anim.SetInteger("UpperBodyAnimState", wm.GetCurrentWeapon().recoilCount);

    }

    public void ResetToIdleAnim()
    {
        character.anim.SetInteger("UpperBodyAnimState", 0);
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
        if (!other.tag.StartsWith(Character.ZOMBIE)) { return; }

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
