using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    Weapon currentWeapon;
    private GameObject player;
    public List<Weapon> weapon_list;
    private GameObject grenade;
    private GameObject explosion;
    private Transform fireposition;
    private Animator anim;
    private CharacterAnimationManager cam;
    private CharacterMovement m_cm;
    private Light laser_flash;
    private Stopwatch flash_timer;

    public bool CanFire;
    public bool isShooting = false;


    private void Awake()
    {
        fireposition = GameObject.Find("player/gun/Bone001/m4a1_upper receiver/fire_position").transform;
    }

    void Start()
    {
        CanFire = true;
        player = GameObject.Find(Character.PLAYER);

        laser_flash = fireposition.GetComponent<Light>();
        laser_flash.intensity = 0f;

        weapon_list = new List<Weapon>(new Weapon[] { new AutoRifle(), new Shotgun(), new LaserCannon()});
        grenade = Resources.Load<GameObject>("Prefabs/Weapons/grenade");
        explosion = Resources.Load<GameObject>("Prefabs/Explosions/Explosion");

        anim = GameObject.Find(Character.PLAYER).GetComponentInChildren<Animator>();
        cam = GameObject.Find(Character.PLAYER).GetComponentInChildren<CharacterAnimationManager>();
        m_cm = GameObject.Find(Character.PLAYER).GetComponentInChildren<CharacterMovement>();

        SwitchWeapon(weapon_list[Weapon.AUTORIFLE]);
    }

    public GameObject GetExplosionPrefab()
    {
        return explosion;
    }

    void Update()
    {
        if(currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 500f)
        {
            laser_flash.intensity = 0f;
        }
    }

    public void FireWeapon()
    {
        if(currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 1000f)
        {
            if (CanFire)
            {
                isShooting = true;
                anim.SetInteger("UpperBodyAnimState", currentWeapon.recoilCount);
                laser_flash.intensity = 1f;
                currentWeapon.ShootWeapon(fireposition.position);
                currentWeapon.timer.Restart();
            }
        }
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void ThrowGrenade()
    {
        cam.ThrowGrenade();
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
    static public Vector3 default_aim_angle_location = new Vector3(0, 0.28f, 7.93f);
    static public Vector3 default_fire_pos = new Vector3(0, 3.2f, -0.6f);

    public Projectile p = new Projectile();
    public Stopwatch timer;

    public int recoilCount;
    public float animPlayTime;
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
        weapon_name = "shotgun";
        damage_per_shot = 25;
        rate_of_fire = 1f;
        weapon_lock_time = 0.1f;
        weapon_spread = 15f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.025f, 0.025f, 0.025f);
        recoilCount = 1;
        animPlayTime = 0.933f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));

        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
    }
}

public class AutoRifle : Weapon
{
    public AutoRifle()
    {
        weapon_name = "autorifle";
        damage_per_shot = 10;
        rate_of_fire = 0.2f;
        weapon_lock_time = 1f;
        weapon_spread = 3f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.05f, 0.05f, 0.05f);
        recoilCount = 2;
        animPlayTime = 0.267f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
    }
}

public class LaserCannon : Weapon
{
    public LaserCannon()
    {
        weapon_name = "lasercannon";
        damage_per_shot = 100;
        rate_of_fire = 1.5f;
        weapon_lock_time = 1f;
        weapon_spread = 0.5f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.1f, 0.1f, 0.1f);
        recoilCount = 1;
        animPlayTime = 0.933f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.identity);
    }
}

public class GrenadeLauncher : Weapon
{
    public GrenadeLauncher()
    {
        weapon_name = "grenadelauncher";
        damage_per_shot = 2000;
        rate_of_fire = 4f;
        weapon_lock_time = 4f;
        weapon_spread = 0f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Rocket");
        p.projectile_scale = new Vector3(0.1f, 0.2f, 0.1f);
        recoilCount = 1;
        animPlayTime = 0.933f;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
    }

}



