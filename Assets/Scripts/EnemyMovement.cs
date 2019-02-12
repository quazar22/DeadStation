using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController cc;
    private GameObject player;
    private float speed = 2.5f;

    private Transform target;
    private Quaternion lookRotation;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        player = GameObject.Find("Player");
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (target.position - transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        //Vector3 playerposition = player.transform.position;
        //transform.forward = Vector3.Lerp(transform.forward, playerposition, 1f);
        cc.SimpleMove(transform.forward);
    }
}
