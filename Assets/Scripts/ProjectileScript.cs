using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 150;
    private float time;
    private float minSpeed = 20;
    private float accelerationTime = 20;
    Rigidbody rb;
    GameObject player;
    Weapon weapon;
    Collider closestCollider;
    Vector3 colliderPos;
    GameObject explosion;
    WeaponManager wm;
    private int penetrationDepth = 1;

    void Start()
    {
        time = 0;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find(Character.PLAYER);

        closestCollider = player.GetComponentInChildren<AimTrigger>().GetClosestCollider();
        if (closestCollider == null)
        {
            colliderPos = player.transform.GetChild(0).position + player.transform.GetChild(0).forward * 10f + new Vector3(0f, 3.2f, 0f);
        } else
        {
            colliderPos = closestCollider.transform.position;
        }

        wm = player.GetComponent<WeaponManager>();
        weapon = wm.GetCurrentWeapon();

        if (weapon is GrenadeLauncher) //grenadelauncher
        {
            rb.velocity = (colliderPos - rb.position).normalized;
            speed = 2000;
            Destroy(gameObject, 5f);
        } else
        {
            rb.velocity = ((colliderPos - rb.position).normalized + AddNoiseOnAngle(-weapon.weapon_spread, weapon.weapon_spread)) * speed;
            Destroy(gameObject, 1f);
        }
        rb.transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (weapon is GrenadeLauncher) //increase projectile speed
        {
            float currentSpeed = Mathf.SmoothStep(minSpeed, speed, time / accelerationTime);
            rb.velocity = (colliderPos - rb.position).normalized * currentSpeed;
            time += Time.deltaTime;
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward, Color.black);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider.tag.StartsWith(Character.ZOMBIE))
                {
                    try
                    {
                        ExplosionDamage(hit.point, 5f);
                    }
                    catch (MissingReferenceException e)
                    {
                        return;
                    }
                    Destroy(gameObject);
                } else if(hit.collider.tag == "wall")
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            RaycastHit hit;
            Debug.DrawRay(gameObject.transform.position, transform.forward, Color.black);
            if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, 4f))
            {
                if (hit.collider.tag.StartsWith(Character.ZOMBIE))
                {
                    try
                    {
                        hit.collider.gameObject.GetComponent<CharacterDataController>().character.DamageCharacter(weapon.damage_per_shot / penetrationDepth++);
                    }
                    catch (MissingReferenceException e)
                    {
                        return;
                    }
                    if(!(weapon is LaserCannon))
                    {
                        Destroy(gameObject);
                    }
                    //Destroy(gameObject);
                } else if (hit.collider.tag.StartsWith("wall"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    Vector3 AddNoiseOnAngle(float min, float max)
    {
        return new Vector3(
          Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360),
          0,
          Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360)
        );
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        for(int i = 0;  i < colliders.Length; i++)
        {
            if(!colliders[i].tag.StartsWith(Character.ZOMBIE)) { continue; }
            float distance = Vector3.Distance(center, colliders[i].transform.position);
            //float damage = -380 * distance + 2000;
            float damage = -80f * Mathf.Pow(distance, 2f) + 2000f;
            colliders[i].GetComponent<CharacterDataController>().character.DamageCharacter(wm.GetCurrentWeapon().damage_per_shot);
        }
    }

}

 public class Projectile
{
    //public float speed;
    public Vector3 projectile_scale;
    public GameObject projectile_object;
}