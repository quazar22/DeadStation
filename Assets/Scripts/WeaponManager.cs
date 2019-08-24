using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon currentWeapon;
    private GameObject player;
    public List<Weapon> weapon_list;
    private GameObject grenade;
    private GameObject explosion;
    private Transform fireposition;
    private Animator anim;
    private CharacterAnimationManager cam;
    private CharacterMovement m_cm;
    //private Light laser_flash;
    private Stopwatch flash_timer;
    private AudioSource[] audio_sources;

    public bool CanFire;
    public bool isShooting = false;
    public int shot_count;

    private void Awake()
    {
        try
        {
            fireposition = GameObject.Find("player/gun/Bone001/m4a1_upper receiver/fire_position").transform;
        } catch(System.Exception e)
        {
            fireposition = GameObject.Find("player/gun/fire_position").transform;
        }
    }

    void Start()
    {
        CanFire = true;
        player = GameObject.Find(Character.PLAYER);

        //laser_flash = fireposition.GetComponent<Light>();
        //laser_flash.intensity = 0f;

        weapon_list = new List<Weapon>(new Weapon[] { new AutoRifle(), new Shotgun(), new LaserCannon()});
        grenade = Resources.Load<GameObject>("Prefabs/Weapons/grenade");
        explosion = Resources.Load<GameObject>("Prefabs/Explosions/Explosion");

        audio_sources = GetComponents<AudioSource>();

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
        //if(currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 500f)
        //{
        //    laser_flash.intensity = 0f;
        //}
        if (currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 1000f)
        {
            CanFire = true;
            anim.SetBool("CanFire", true);
        }
        else
        {
            CanFire = false;
            anim.SetBool("CanFire", false);
        }
    }

    public void FireWeapon()
    {
        if (currentWeapon.timer.ElapsedMilliseconds >= currentWeapon.rate_of_fire * 1000f)
        {
            if (CanFire)
            {
                isShooting = true;
                //laser_flash.intensity = 1f;
                if (shot_count == 0)
                {
                    anim.SetBool("CanFire", true);
                    cam.BeginShooting();
                }
                currentWeapon.ShootWeapon(fireposition.position);
                currentWeapon.timer.Restart();
                shot_count++;
                AudioSource source = GetAvailableAudioSource();
                source.volume = 0.2f;
                source.Play(0);
            }
        }
    }

    AudioSource GetAvailableAudioSource()
    {
        foreach(AudioSource source in audio_sources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return audio_sources[0]; //¯\_(ツ)_/¯
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
        foreach(AudioSource source in audio_sources)
        {
            source.clip = weapon.weapon_sound_clip;
        }
        currentWeapon = weapon;
        weapon.p.projectile_object.transform.localScale = weapon.p.projectile_scale;
        shot_count = 0;
    }

    public void SetCanFire(bool canFire)
    {
        CanFire = canFire;
    }

}

abstract public class Weapon
{
    public static string[] weapons = { "shotgun", "autorifle", "lasercannon", "grenadelauncher" };
    static public Vector3 default_fire_pos = new Vector3(0, 3.2f, -0.6f);

    public Projectile p = new Projectile();
    public Stopwatch timer;
    public AudioClip weapon_sound_clip;

    public int recoilCount;
    public string weapon_name;
    public int damage_per_shot;
    public float rate_of_fire;
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
        weapon_spread = 15f;
        weapon_sound_clip = Resources.Load<AudioClip>("Audio/shotgun_shot");
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.025f, 0.025f, 0.025f);
        recoilCount = 1;
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
        weapon_spread = 3f;
        weapon_sound_clip = Resources.Load<AudioClip>("Audio/autorifle_shot_1");
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.05f, 0.05f, 0.05f);
        recoilCount = 2;
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
        weapon_spread = 0.5f;
        weapon_sound_clip = Resources.Load<AudioClip>("Audio/lasercannon_shot");
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Laser");
        p.projectile_scale = new Vector3(0.1f, 0.1f, 0.1f);
        recoilCount = 1;
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
        weapon_spread = 0f;
        p.projectile_object = Resources.Load<GameObject>("Prefabs/Projectiles/Rocket");
        p.projectile_scale = new Vector3(0.1f, 0.2f, 0.1f);
        recoilCount = 1;
        timer = new Stopwatch();
        timer.Start();
    }

    public override void ShootWeapon(Vector3 at)
    {
        Object.Instantiate(p.projectile_object, at, Quaternion.Euler(0, 0, 0));
    }

}



