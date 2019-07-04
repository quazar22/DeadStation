using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private CharacterController cc;
    private CharacterDataController cdc;
    private GameObject player;
    NavMeshAgent agent;

    private Vector3 direction;
    private Animator anim;

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
        //cc = GetComponent<CharacterController>(); //wary of deleting
        cdc = GetComponent<CharacterDataController>();
        player = GameObject.Find(Character.PLAYER).transform.GetChild(0).gameObject; //"player"
        agent = GetComponent<NavMeshAgent>();
        fps = Application.targetFrameRate;

        StartWithRandomAnim();
    }

    void Update()
    {
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
                inside_range_time += Time.deltaTime;
                if (WeightScalar < 1f)
                {
                    if (WeightScalar < 0.1f)
                    {
                        attack_state = Random.Range(0, 3);
                        anim.SetInteger("AttackState", attack_state); //attack_left/right = 79 frames, attack both = 139 frames
                                                                      //damage_cooldown is calculated time until next attack will hit

                        //attack both = 4.633 seconds, 4.633/2 = 2.3165 seconds of playback.
                        //attack happens at frame 44/139 for the animations
                        //attack happens at 2.3165 * 44/139 seconds of playback

                        //attack_left/right = 2.633 seconds, 2.633/2 = 1.3165 seconds of playback.
                        //attack happens at frame 32/79 for the animations
                        //attack happens at 1.3165 * 32/79 seconds of playback
                        damage_cooldown = attack_state == 0 || attack_state == 1 ? (32f / 79f) * RL_attack_time : (44f / 139f) * both_attack_time;
                    }
                    WeightScalar += Time.deltaTime * 2f;
                    anim.SetLayerWeight(1, WeightScalar);
                }
                if (distance < 3f)
                {
                    StandStill();
                    if (inside_range_time > damage_cooldown)
                    {
                        Debug.Log("Damage Taken!");
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

    public void StartWithRandomAnim()
    {
        anim_state = Random.Range(2, 6);
        anim.SetInteger("AnimState", anim_state);

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
        anim.SetFloat("AttackSpeed", 2f / anim_speed);
    }

}
