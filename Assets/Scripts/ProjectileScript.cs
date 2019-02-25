using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float speed = 50;
    private float time;
    private float minSpeed = 1;
    private float accelerationTime = 20;
    Rigidbody rb;
    GameObject player;
    Weapon w;
    Collider c;

    void Start()
    {
        time = 0;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find(Character.char_names[1]);
        c = player.GetComponentInChildren<AimTrigger>().GetClosestCollider();
        w = player.GetComponent<WeaponManager>().GetCurrentWeapon();
        rb.velocity = (c.transform.position - rb.position).normalized;
        rb.transform.LookAt(c.transform);
        rb.transform.rotation *= Quaternion.Euler(0, -90f, 0);

        if (w.weapon_name == Weapon.weapons[0]) //shotgun
        {
            rb.velocity = ((c.transform.position - rb.position).normalized + AddNoiseOnAngle(0, 15)) * speed;
            Destroy(gameObject, 1f);
        }
        else if (w.weapon_name == Weapon.weapons[1]) //autorifle
        {
            rb.velocity = ((c.transform.position - rb.position).normalized + AddNoiseOnAngle(0, 5)) * speed;
            Destroy(gameObject, 1f);
        }
        else if (w.weapon_name == Weapon.weapons[2]) //lasercannon
        {
            rb.velocity = ((c.transform.position - rb.position).normalized + AddNoiseOnAngle(0, 1)) * speed;
            Destroy(gameObject, 1f);
        }
        else if (w.weapon_name == Weapon.weapons[3]) //grenadelauncher
        {
            speed = 2000;
            rb.useGravity = false;
            Destroy(gameObject, 5f);
        }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (w.weapon_name == Weapon.weapons[3]) //increase projectile speed
        {
            float currentSpeed = Mathf.SmoothStep(minSpeed, speed, time / accelerationTime);
            rb.velocity = (c.transform.position - rb.position).normalized * currentSpeed;
            time += Time.deltaTime;
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right, Color.black);
            if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
            {
                if (hit.collider)
                {
                    Debug.Log("ROCKET HIT");
                    Destroy(gameObject);
                }
            }
        } else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.right, Color.black);
            if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
            {
                if (hit.collider)
                {
                    Debug.Log("BULLET HIT");
                    Destroy(gameObject);
                }
            }
        }
    }

    Vector3 AddNoiseOnAngle(float min, float max)
    {
        return new Vector3(
          Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360),
          Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360),
          Mathf.Sin(2 * Mathf.PI * Random.Range(min, max) / 360)
        );
    }

}

 public class Projectile
{
    public float speed;
    public float projectile_scale;
    public GameObject projectile_object;
}