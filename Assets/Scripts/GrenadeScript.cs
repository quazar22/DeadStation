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
    
    float explosion_radius = 20f;

    private Light grenade_light;
    bool exploded;

    Stopwatch st;
    // Start is called before the first frame update
    void Start()
    {
        st = new Stopwatch();
        grenade_light = GetComponent<Light>();
        player = GameObject.Find(Character.PLAYER);
        cc = player.GetComponent<CharacterController>();
        player_mesh = GameObject.Find(Character.PLAYER + "/SpaceMan@Idle");
        wm = player.GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce((player.transform.position + player.transform.forward * 2000f) + (cc.velocity * 200));
        UnityEngine.Debug.Log(cc.velocity * 100);

        exploded = false;
        st.Start();

        grenade_light.intensity = 0f;

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
        while(!exploded)
        {
            //grenade_light.intensity = 10f * Mathf.Sin((2 * Mathf.PI) * ((st.ElapsedMilliseconds / 1000f) + 0.25f)) + 10f; //old
            if (st.ElapsedMilliseconds >= 2500)
            {
                grenade_light.intensity = 40f;
            }
            else
            {
                float s_input = 20 * Mathf.Sin(10 * Mathf.Pow(st.ElapsedMilliseconds / 1000f, 2)) + 20;//Mathf.Pow(st.ElapsedMilliseconds / 600f, 3) + 0.5f; //current
                grenade_light.intensity = s_input;
            }
            if (st.ElapsedMilliseconds >= 3000f)
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
                z.cc.enabled = true;
                z.cc.detectCollisions = false;
                z.nma.enabled = false;
                z.anim.enabled = false;

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
