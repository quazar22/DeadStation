using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceTest : MonoBehaviour
{
    private GameObject character;
    Vector3 center = Vector3.zero;

    private GameObject[] zombies;
    private WeaponManager wm;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("player/SpaceMan@Idle");
        wm = GameObject.Find("player").GetComponent<WeaponManager>();
        zombies = GameObject.FindGameObjectsWithTag("zombie");
        foreach(GameObject z in zombies)
        {
            foreach (Rigidbody rbb in z.GetComponentsInChildren<Rigidbody>())
            {
                rbb.isKinematic = true;
                rbb.detectCollisions = false;
            }
            foreach (Collider c in z.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            wm.ThrowGrenade();
        }
    }
}
