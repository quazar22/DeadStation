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

    public bool shouldAttack;
    public bool shouldMove;
    private bool shouldFollow;

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
        shouldMove = true;
        shouldFollow = true;

        zam.BeginIdleAnimation();
        agent_speed = zam.SetRandomMovementAnim();
    }

    void Update()
    {
        if(shouldFollow)
            FollowPlayer();
        //else if(Vector3.Distance(gameObject.transform.position, player.transform.position) < 20f && cm.GetMovementMagnitude() > 6.75f/2f)
        //{
        //    AlertZombie();
        //}
    }

    void AlertZombie()
    {
        shouldAttack = true;
        ContinueMoving();
    }

    void SleepZombie()
    {
        shouldAttack = false;
        StandStill();
    }

    public void FollowPlayer()
    {
        if (agent.enabled && player_char.isAlive)
        {
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(agent.transform.position, player.transform.position);

            if (distance < 4f)
            {
                shouldAttack = true;
                if(distance < 3f)
                {
                    shouldMove = false;
                    StandStill();
                }
            }
            else
            {
                ContinueMoving();
            }
        } else if (!player_char.isAlive)
        {
            shouldAttack = false;
            shouldMove = false;
            agent.enabled = false;
        }
    }

    public void StandStill()
    {
        shouldMove = false;
        agent.speed = 0f;
        agent.acceleration = 60f;
    }

    public void ContinueMoving()
    {
        agent.acceleration = 8f;
        agent.speed = agent_speed;
        shouldAttack = false;
        shouldMove = true;
    }

    public GameObject GetZombieTarget()
    {
        return player;
    }

}
