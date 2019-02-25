using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    Weapon currentWeapon; //manage dis 

    GameObject aim_angle;
    GameObject player;
    List<Weapon> weapon_list;
    GameObject laser;
    GameObject rocket;
    Transform fireposition;

    void Start()
    {
        aim_angle = GameObject.Find("aim_angle");
        player = GameObject.Find(Character.char_names[1]);
        laser = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        rocket = Resources.Load<GameObject>("Prefabs/Projectiles/Rocket");
        fireposition = GameObject.Find("fire_position").transform;
        weapon_list = new List<Weapon>(new Weapon[] { new AutoRifle(), new Shotgun(), new LaserCannon(), new GrenadeLauncher() });
        weapon_list[0].p.projectile_object = weapon_list[1].p.projectile_object = weapon_list[2].p.projectile_object = laser;
        weapon_list[3].p.projectile_object = rocket;
        currentWeapon = weapon_list[0];
    }

    void Update()
    {
        
    }

    public void FireWeapon()
    {
        if(currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 1000f)
        {
            currentWeapon.ShootWeapon(fireposition.position);
            currentWeapon.timer.Restart();
        }
    }

    public Weapon GetWeapon(string weapon)
    {
        foreach(Weapon w in weapon_list)
        {
            if(w.weapon_name == weapon)
            {
                return w;
            }
        }
        return weapon_list[0];
    }

    public Weapon GetCurrentWeapon() { return currentWeapon; }

    public void SwitchWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }
}

abstract public class Weapon
{
    public static string[] weapons = { "shotgun", "autorifle", "lasercannon", "grenadelauncher" };
    static public Vector3 default_aim_angle_size = new Vector3(300f, 900f, 50f);
    static public Vector3 default_aim_angle_location = new Vector3(0f, 0f, 10f);
    public Vector3 aim_angle_size;
    public Vector3 aim_angle_location;
    public Projectile p = new Projectile();
    public Stopwatch timer;
    public GameObject bullet;

    public string weapon_name;
    public float damage_per_shot;
    public float rate_of_fire;
    public float weapon_lock_time;

    public abstract void ShootWeapon(Vector3 at);
}

public class Shotgun : Weapon
{
    public Shotgun()
    {
        aim_angle_size = new Vector3(800f, 450f, default_aim_angle_size.z);
        aim_angle_location = new Vector3(0f, 0f, 5.5f);
        weapon_name = "shotgun";
        damage_per_shot = 20f;
        rate_of_fire = 1f;
        weapon_lock_time = 0.1f;
        p.projectile_scale = 1f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));

        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));

        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));

        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
        //Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));

    }
}

public class AutoRifle : Weapon
{
    public AutoRifle()
    {
        aim_angle_size = default_aim_angle_size;
        aim_angle_location = default_aim_angle_location;
        weapon_name = "autorifle";
        damage_per_shot = 25f;
        rate_of_fire = 0.2f;
        weapon_lock_time = 1f;
        p.projectile_scale = 1f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
    }
}

public class LaserCannon : Weapon
{
    public LaserCannon()
    {
        aim_angle_size = new Vector3(250f, 900f, default_aim_angle_size.z);
        aim_angle_location = default_aim_angle_location;
        weapon_name = "lasercannon";
        damage_per_shot = 25f;
        rate_of_fire = 0.25f;
        weapon_lock_time = 1f;
        p.projectile_scale = 1f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
    }
}

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher()
    {
        aim_angle_size = new Vector3(150f, 1500f, default_aim_angle_size.z);
        aim_angle_location = new Vector3(0f, 0f, 16f);
        weapon_name = "grenadelauncher";
        damage_per_shot = 2000f;
        //rate_of_fire = 10f;
        rate_of_fire = 5f;
        weapon_lock_time = 4f;
        p.projectile_scale = 1f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
    }
}



