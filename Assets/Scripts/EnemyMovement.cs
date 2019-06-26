using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController cc;
    private CharacterDataController cdc;
    private GameObject player;
    private float speed = 2.5f;

    private Transform target;
    private Quaternion lookRotation;
    private Vector3 direction;

    NavMeshAgent agent;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"
        //target = player.transform;

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //direction = (target.position - transform.position).normalized;
        //lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
        //cc.SimpleMove(transform.forward);

        agent.destination = player.transform.position;
    }

    private void LateUpdate()
    {
        //speed = cdc.character.GetMoveSpeed();
    }

}
