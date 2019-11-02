using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    GameObject player_mesh;
    CharacterController cc;
    WeaponManager wm;
    AudioSource source;
    AudioClip fast_tick = null;
    AudioClip long_tick = null;
    
    float explosion_radius = 20f;

    private Light grenade_light;
    bool exploded;

    Stopwatch st;
    // Start is called before the first frame update
    void Start()
    {
        st = new Stopwatch();
        source = GetComponent<AudioSource>();
        grenade_light = GetComponent<Light>();
        player = GameObject.Find(Character.PLAYER);
        cc = player.GetComponent<CharacterController>();
        player_mesh = GameObject.Find(Character.PLAYER + "/SpaceMan@Idle");
        wm = player.GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce((player.transform.position + player.transform.forward * 2000f) + (cc.velocity * 200));

        exploded = false;
        st.Start();

        grenade_light.intensity = 0f;

        if(fast_tick == null || long_tick == null)
        {
            fast_tick = Resources.Load<AudioClip>("Audio/grenade_tick");
            long_tick = Resources.Load<AudioClip>("Audio/grenade_long_tick");
            source.clip = fast_tick;
        }

        StartCoroutine("GrenadeTick");
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    IEnumerator GrenadeTick()
    {
        bool played = false;
        bool should_continue = true;
        while (!exploded)
        {
            if (st.ElapsedMilliseconds >= 2250f && should_continue)
            {
                source.clip = long_tick;
                grenade_light.intensity = 40f;
                should_continue = false;
                source.Play(0);
            }
            else if(st.ElapsedMilliseconds < 2500f)
            {
                float s_input = 20f * Mathf.Sin( (2f * Mathf.PI) * ((st.ElapsedMilliseconds / 750f) + 1.25f) ) + 20f;//Mathf.Pow(st.ElapsedMilliseconds / 600f, 3) + 0.5f; //current
                grenade_light.intensity = s_input;
                if(s_input >= 39f && !played)
                {
                    source.Play(0);
                    played = true;
                }
                if(s_input <= 5f)
                {
                    played = false;
                }
            }
            if (st.ElapsedMilliseconds >= 2750f)
            {
                Explode();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Explode()
    {
        exploded = true;

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, explosion_radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].tag.StartsWith(Character.ZOMBIE)) { continue; }

            float distance = Vector3.Distance(gameObject.transform.position, colliders[i].transform.position);
            int damage = (int)(-200f * Mathf.Log10((distance/2) + 1f) + 200f);

            Zombie z = (Zombie)colliders[i].GetComponent<CharacterDataController>().character;
            z.DamageCharacter(damage);

            if(z.GetHealth() <= 0)
            {
                z.m_anim.enabled = false;

                foreach (Rigidbody rb in z.rigidbodies)
                {
                    rb.detectCollisions = true;
                    rb.isKinematic = false;
                }

                foreach (Collider c in z.colliders)
                {
                    c.enabled = true;
                    c.gameObject.layer = LayerMask.NameToLayer("zombie_limbs");
                }

                z.rigidbodies[0].AddExplosionForce(250f, gameObject.transform.position, 8f, 1f, ForceMode.Impulse);
            }
        }

        Destroy(Instantiate(wm.GetExplosionPrefab(), gameObject.transform.position, Quaternion.identity), 2.2f);
        Destroy(gameObject);

    }
}
