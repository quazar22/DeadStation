using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceTest : MonoBehaviour
{
    private GameObject character;
    private GameObject zombie;
    private CharacterController cc;
    private NavMeshAgent nma;
    private Animator anim;
    private Rigidbody[] rb;
    Vector3 center = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("player/SpaceMan@Idle");
        zombie = GameObject.Find("zombie0");
        nma = zombie.GetComponent<NavMeshAgent>();
        cc = zombie.GetComponent<CharacterController>();
        anim = zombie.GetComponentInChildren<Animator>();
        rb = zombie.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody r in rb)
        {
            r.detectCollisions = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cc.enabled = false;
            foreach (Rigidbody rbb in rb)
            {
                rbb.detectCollisions = true;
            }
            nma.enabled = false;
            anim.enabled = false;
            rb[0].AddExplosionForce(300f, zombie.transform.position, 100f, 10f, ForceMode.Impulse);
        }
    }
}
