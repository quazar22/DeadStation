using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        time = 0;
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find(Character.char_names[1]);
        closestCollider = player.GetComponentInChildren<AimTrigger>().GetClosestCollider();
        weapon = player.GetComponent<WeaponManager>().GetCurrentWeapon();
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
                    Debug.Log("ROCKET HIT");
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
                    Debug.Log("BULLET HIT");
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

}

 public class Projectile
{
    public float speed;
    public Vector3 projectile_scale;
    public GameObject projectile_object;
}