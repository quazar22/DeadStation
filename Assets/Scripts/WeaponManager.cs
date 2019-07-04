using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    Weapon currentWeapon;
    GameObject aim_angle;
    GameObject aim_cone;
    GameObject player;
    public List<Weapon> weapon_list;
    Transform fireposition;
    bool CanFire;

    void Start()
    {
        CanFire = true;
        aim_angle = GameObject.Find("player/aim_angle");
        try
        {
            aim_cone = GameObject.Find("player/aim_cone_blue");
        } catch(System.NullReferenceException e)
        {
            aim_cone = GameObject.Find("player/aim_cone_blue_dotted");
        }
        player = GameObject.Find(Character.PLAYER);
        fireposition = GameObject.Find("player/fire_position").transform;
        weapon_list = new List<Weapon>(new Weapon[] { new AutoRifle(), new Shotgun(), new LaserCannon(), new GrenadeLauncher() });
        SwitchWeapon(weapon_list[Weapon.AUTORIFLE]);
    }

    void Update()
    {
        
    }

    public void FireWeapon()
    {
        if(currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 1000f)
        {
            if (CanFire)
            {
                currentWeapon.ShootWeapon(fireposition.position);
                currentWeapon.timer.Restart();
            }
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
        Transform aim = aim_angle.gameObject.transform;
        Transform player_aim_cone = aim_cone.gameObject.transform;
        aim.localScale = player_aim_cone.localScale = weapon.aim_angle_size;
        aim.localPosition = player_aim_cone.localPosition = weapon.aim_angle_location;
        weapon.p.projectile_object.transform.localScale = weapon.p.projectile_scale;
    }

    public void SetCanFire(bool canFire)
    {
        CanFire = canFire;
    }

}

abstract public class Weapon
{
    public static string[] weapons = { "shotgun", "autorifle", "lasercannon", "grenadelauncher" };
    static public Vector3 default_aim_angle_size = new Vector3(300f, 900f, 15f);
    static public Vector3 default_aim_angle_location = new Vector3(16.56f, 0.28f, 8.79f);
    static public Vector3 default_fire_pos = new Vector3(16.56f, 3.2f, -0.6f);

    public Vector3 aim_angle_size;
    public Vector3 aim_angle_location;
    public Projectile p = new Projectile();
    public Stopwatch timer;

    public string weapon_name;
    public int damage_per_shot;
    public float rate_of_fire;
    public float weapon_lock_time;
    public float weapon_spread;

    public static int AUTORIFLE = 0;
    public static int SHOTGUN = 1;
    public static int LASERCANNON = 2;
    public static int GRENADELAUNCHER = 3;

    abstract public void ShootWeapon(Vector3 at);
}

public class Shotgun : Weapon
{
    public Shotgun()
    {
        aim_angle_size = new Vector3(400f, 450f, default_aim_angle_size.z);
        aim_angle_location = new Vector3(16.56f, 0.28f, 4f);
        weapon_name = "shotgun";
        damage_per_shot = 25;
        rate_of_fire = 1f;
        weapon_lock_time = 0.1f;
        weapon_spread = 15f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(5f, 5f, 5f);
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
    }
}

public class AutoRifle : Weapon
{
    public AutoRifle()
    {
        aim_angle_size = default_aim_angle_size;
        aim_angle_location = default_aim_angle_location;
        weapon_name = "autorifle";
        damage_per_shot = 10;
        rate_of_fire = 0.2f;
        weapon_lock_time = 1f;
        weapon_spread = 3f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(5f, 5f, 5f);
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
        damage_per_shot = 100;
        rate_of_fire = 1.5f;
        weapon_lock_time = 1f;
        weapon_spread = 0.5f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(8f, 8f, 8f);
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
        aim_angle_location = new Vector3(16.56f, default_aim_angle_location.y, 14.88f);
        weapon_name = "grenadelauncher";
        damage_per_shot = 2000;
        rate_of_fire = 10f;
        weapon_lock_time = 4f;
        weapon_spread = 0f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Rocket");
        p.projectile_scale = new Vector3(1f, 1f, 1f);
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 90, 0));
    }

}



