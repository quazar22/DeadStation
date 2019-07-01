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
    private Animator anim;

    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //cc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"

        agent = GetComponent<NavMeshAgent>();
        //int animState = (int)Mathf.Round(Random.Range(1f, 2f));
        //float animSpeed = Random.Range(0.5f, 1.5f);
        //anim.SetInteger("AnimState", animState);
        anim.speed = 1;
        float anim_multiplier = Random.Range(0.5f, 1f);
        int anim_state = Random.Range(2,6);
        anim.SetInteger("AnimState", anim_state);
        if (anim_state == 2)
        {
            anim.speed = 2f * anim_multiplier;
            agent.speed = 1.6f * anim_multiplier;
        } else if(anim_state == 5)
        {
            anim.speed = 1f;
            agent.speed = 4.5f;
        } else if(anim_state == 4)
        {
            anim.speed = 1f * anim_multiplier;
            agent.speed = 1.5f * anim_multiplier;
        }
        else
        {
            anim.speed = 0.75f;
            agent.speed = 4.5f;
        }
    }

    void Run()
    {
        anim.SetInteger("AnimState", 4);
        agent.speed = 6f;
    }

    void Walk()
    {
        anim.SetInteger("AnimState", 3);
        agent.speed = 0.8f;
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
