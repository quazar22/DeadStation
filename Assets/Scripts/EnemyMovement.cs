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

    private Vector3 direction;
    private Animator anim;

    private int idle_anim;
    private int attack_anim;

    private float WeightScalar = 0f;
    private int anim_state;
    private int attack_state;
    private float anim_speed;
    private float agent_speed;
    private float inside_range_time = 0f;
    private float damage_cooldown = 0f;

    private float RL_attack_time = 1.3165f;
    private float both_attack_time = 2.3165f;

    private int fps;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>(); //wary of deleting
        cm = GameObject.Find(Character.PLAYER).GetComponent<CharacterMovement>();
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"
        agent = GetComponent<NavMeshAgent>();
        fps = Application.targetFrameRate;
        player_char = GameObject.Find(Character.PLAYER).GetComponent<CharacterDataController>().character;

        BeginIdleAnimation();
        //StartWithRandomAnim();
    }

    void Update()
    {
        //FollowPlayerAndAttack();
    }

    public void FollowPlayerAndAttack()
    {
        if (agent.enabled)
        {
            agent.destination = player.transform.position;
            float distance = Vector3.Distance(agent.transform.position, player.transform.position);

            if (distance < 4f)
            {
                inside_range_time += Time.deltaTime;
                if (WeightScalar < 1f)
                {
                    if (WeightScalar < 0.1f)
                    {
                        attack_state = Random.Range(0, 3);
                        anim.SetInteger("AttackState", attack_state); //attack_left/right = 79 frames, attack both = 139 frames
                        damage_cooldown = attack_state == 0 || attack_state == 1 ? (32f / 79f) * RL_attack_time : (44f / 139f) * both_attack_time;
                    }
                    WeightScalar += Time.deltaTime * 2f;
                    anim.SetLayerWeight(1, WeightScalar);
                }
                if (distance < 3f)
                {
                    if(!(cm.GetMovementMagnitude() > 0f))
                        StandStill();
                    if (inside_range_time > damage_cooldown)
                    {
                        //Damage character
                        player_char.DamageCharacter(0);
                        inside_range_time -= attack_state == 1 || attack_state == 0 ? RL_attack_time : both_attack_time;
                    }
                }
            }
            else
            {
                inside_range_time = 0f;
                if (WeightScalar > 0f)
                {
                    WeightScalar -= Time.deltaTime * 2f;
                    anim.SetLayerWeight(1, WeightScalar);
                }
                ContinueMoving();
            }
        }
    }

    public void StandStill()
    {
        agent.speed = 0f;
        agent.acceleration = 60f;
        anim.SetInteger("AnimState", Random.Range(0, 2));
        anim.speed = 1f;
        anim.SetFloat("AttackSpeed", 2f);
    }

    public void ContinueMoving()
    {
        anim.speed = anim_speed;
        agent.acceleration = 8f;
        agent.speed = agent_speed;
        anim.SetInteger("AnimState", anim_state);
        anim.SetFloat("AttackSpeed", 2f / anim_speed);
    }

    private void LateUpdate()
    {
        //speed = cdc.character.GetMoveSpeed();
    }

    public void BeginIdleAnimation()
    {
        idle_anim = Random.Range(0, 2);
        anim.SetInteger("AnimState", idle_anim);
        anim.speed = Random.Range(0.75f, 1.5f);
    }

    public void RandomAttackAnim()
    {
        attack_anim = anim_state = Random.Range(2, 6);
        //anim.SetInteger("AnimState", anim_state);

        if (anim_state == 2)
        {
            float multiplier = Random.Range(0.75f, 1.25f);
            anim_speed = anim.speed = 2f * multiplier;
            agent_speed = agent.speed = 1.6f * multiplier;
        }
        else if (anim_state == 5)
        {
            float multiplier = Random.Range(.75f, 1f);
            anim_speed = anim.speed = 1f * multiplier;
            agent_speed = agent.speed = 4.5f * multiplier;
        }
        else if (anim_state == 4)
        {
            float multiplier = Random.Range(.75f, 1.5f);
            anim_speed = anim.speed = 1f * multiplier;
            agent_speed = agent.speed = 1.5f * multiplier;
        }
        else
        {
            float multiplier = Random.Range(.8f, 1.25f);
            anim_speed = anim.speed = 0.75f * multiplier;
            agent_speed = agent.speed = 4.5f * multiplier;
        }
        //anim.SetFloat("AttackSpeed", 2f / anim_speed);
    }

}
