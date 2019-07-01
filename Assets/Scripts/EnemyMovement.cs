using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController cc;
    private CharacterDataController cdc;
    private GameObject player;

    private Vector3 direction;
    private Animator anim;

    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //cc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"
        agent = GetComponent<NavMeshAgent>();

        int anim_state = Random.Range(2,6);
        anim.SetInteger("AnimState", anim_state);

        if (anim_state == 2)
        {
            float multiplier = Random.Range(0.75f, 1f);
            anim.speed = 2f * multiplier;
            agent.speed = 1.6f * multiplier;
        } else if(anim_state == 5)
        {
            anim.speed = 1f;
            agent.speed = 4.5f;
        } else if(anim_state == 4)
        {
            float multiplier = Random.Range(1f, 1.5f);
            anim.speed = 1f * multiplier;
            agent.speed = 1.5f * multiplier;
        }
        else
        {
            float multiplier = Random.Range(1f, 1.5f);
            anim.speed = 0.75f * multiplier;
            agent.speed = 4.5f * multiplier;
        }
    }

    void Update()
    {
        if(agent.enabled)
            agent.destination = player.transform.position;
    }

    private void LateUpdate()
    {
        //speed = cdc.character.GetMoveSpeed();
    }

}
