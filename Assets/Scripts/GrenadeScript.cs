using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    GameObject player_mesh;
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
        player_mesh = GameObject.Find(Character.PLAYER + "/SpaceMan@Idle");
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(rb.velocity + player.transform.position + player.transform.forward * 2000f);

        exploded = false;
        st.Start();

        grenade_light.intensity = 0f;

        StartCoroutine("GrenadeTick");
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    
    IEnumerator GrenadeTick()
    {
        while(!exploded)
        {
            grenade_light.intensity = 10f * Mathf.Sin((2 * Mathf.PI) * ((st.ElapsedMilliseconds / 1000f) + 0.25f)) + 10f;
            if(st.ElapsedMilliseconds >= 3000f)
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
            float damage = -80f * Mathf.Pow(distance, 2f) + 2000f;

            Zombie z = (Zombie)colliders[i].GetComponent<CharacterDataController>().character;
            z.DamageCharacter((int)damage);
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

            z.rigidbodies[0].AddExplosionForce(300f, gameObject.transform.position, 15f, 1f, ForceMode.Impulse);

        }

        Destroy(gameObject);
    }
}
