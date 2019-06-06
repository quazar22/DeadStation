using System;
using System.Collections;
using System.Collections.Generic;
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
    GameObject explosion;
    WeaponManager wm;

    void Start()
    {
        time = 0;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find(Character.char_names[1]);
        closestCollider = player.GetComponentInChildren<AimTrigger>().GetClosestCollider();
        wm = player.GetComponent<WeaponManager>();
        weapon = wm.GetCurrentWeapon();
        explosion = wm.GetExplosionRadius();
        rb.transform.LookAt(closestCollider.transform);
        rb.transform.rotation *= Quaternion.Euler(0, -90f, 0);

        if (weapon.weapon_name == Weapon.weapons[3]) //grenadelauncher
        {
            rb.velocity = (closestCollider.transform.position - rb.position).normalized;
            speed = 2000;
            rb.useGravity = false;
            Destroy(gameObject, 5f);
        } else
        {
            rb.velocity = ((closestCollider.transform.position - rb.position).normalized + AddNoiseOnAngle(-weapon.weapon_spread, weapon.weapon_spread)) * speed;
            Destroy(gameObject, 1f);
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (weapon.weapon_name == Weapon.weapons[3]) //increase projectile speed
        {
            float currentSpeed = Mathf.SmoothStep(minSpeed, speed, time / accelerationTime);
            rb.velocity = (closestCollider.transform.position - rb.position).normalized * currentSpeed;
            time += Time.deltaTime;
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right, Color.black);
            if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
            {
                if (hit.collider)
                {
                    if (hit.collider.gameObject.name.StartsWith("aim_angle"))
                        return;
                    try
                    {
                        //hit.collider.gameObject.GetComponent<CharacterDataController>().character.DamageCharacter(weapon.damage_per_shot);
                        //Instantiate(explosion, hit.collider.transform.position, Quaternion.identity);
                        ExplosionDamage(hit.point, 5f);
                    }
                    catch (MissingReferenceException e)
                    {
                        return;
                    }
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right, Color.black);
            if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
            {
                if (hit.collider)
                {
                    if (hit.collider.gameObject.name.StartsWith("aim_angle"))
                        return;
                    if (hit.collider.tag.StartsWith("wall"))
                    {
                        Destroy(gameObject);
                        return;
                    }
                    try
                    {
                        hit.collider.gameObject.GetComponent<CharacterDataController>().character.DamageCharacter(weapon.damage_per_shot);
                    }
                    catch (MissingReferenceException e)
                    {
                        return;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    Vector3 AddNoiseOnAngle(float min, float max)
    {
        //return new Vector3(
        //  Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360),
        //  Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360),
        //  Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360)
        //);
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
            if(!colliders[i].tag.StartsWith(Character.char_names[0])) { continue; }
            float distance = Vector3.Distance(center, colliders[i].transform.position);
            float damage = -380 * distance + 2000;
            colliders[i].GetComponent<CharacterDataController>().character.DamageCharacter(2000);
        }
    }

}

 public class Projectile
{
    public float speed;
    public Vector3 projectile_scale;
    public GameObject projectile_object;
}