using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    float explosion_radius = 10f;

    private Light grenade_light;
    bool exploded;

    Stopwatch st;
    // Start is called before the first frame update
    void Start()
    {
        st = new Stopwatch();
        grenade_light = GetComponent<Light>();
        player = GameObject.Find(Character.PLAYER);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(rb.velocity + player.transform.position + player.transform.forward * 2500f);

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
            UnityEngine.Debug.Log(grenade_light.intensity);
            if(st.ElapsedMilliseconds > 4000f)
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
            //float damage = -380 * distance + 2000;
            float damage = -80f * Mathf.Pow(distance, 2f) + 2000f;
            colliders[i].GetComponent<CharacterDataController>().character.DamageCharacter((int)damage);
        }

        Destroy(gameObject, 2f);
    }
}
