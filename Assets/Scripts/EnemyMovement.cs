using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController cc;
    private CharacterDataController cdc;
    private CharacterMovement cm;
    private GameObject player;
    private Character player_char;
    NavMeshAgent agent;
    private ZombieAnimationManager zam;

    private float agent_speed = 1f;

    private bool shouldAttack;

    void Start()
    {
        //anim = GetComponentInChildren<Animator>();
        zam = GetComponentInChildren<ZombieAnimationManager>();
        cc = GetComponent<CharacterController>(); //wary of deleting
        cm = GameObject.Find(Character.PLAYER).GetComponent<CharacterMovement>();
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"
        agent = GetComponent<NavMeshAgent>();
        player_char = GameObject.Find(Character.PLAYER).GetComponent<CharacterDataController>().character;

        shouldAttack = true;

        zam.BeginIdleAnimation();
        agent_speed = zam.SetRandomMovementAnim();
    }

    void Update()
    {
        if(shouldAttack)
            FollowPlayerAndAttack();
    }

    public void FollowPlayerAndAttack()
    {
        if (agent.enabled)
        {
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(agent.transform.position, player.transform.position);

            if (distance < 4f)
            {
                zam.Attack(player);
            }
            else
            {
                ContinueMoving();
            }
        }
    }

    public void StandStill()
    {
        agent.speed = 0f;
        agent.acceleration = 60f;
        zam.StandStill();
    }

    public void ContinueMoving()
    {
        agent.acceleration = 8f;
        agent.speed = agent_speed;
        zam.ContinueMoving();
    }

}
