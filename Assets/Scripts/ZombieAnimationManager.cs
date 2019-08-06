using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationManager : MonoBehaviour
{
    private Animator m_anim;
    private CharacterDataController m_cdc;

    private CharacterMovement CharacterMovement = null;
    private EnemyMovement m_em;

    private GameObject m_player_target = null;

    float m_WeightScalar;

    //upper body
    int m_attack_state;
    float m_upper_anim_speed;

    //lower body
    int m_move_state;
    float m_lower_anim_speed;

    //both
    int idle_anim;
    float idle_speed;

    bool shouldTakeDamage;

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_WeightScalar = 0f;
        m_cdc = GetComponentInParent<CharacterDataController>();
        m_em = GetComponentInParent<EnemyMovement>();
        idle_anim = Random.Range(0, 2);
        idle_speed = Random.Range(0.75f, 1.5f);

        shouldTakeDamage = false;

        m_attack_state = Random.Range(0, 3);
    }

    private void FixedUpdate()
    {
        if (m_cdc.character.isAlive)
        {
            if(shouldTakeDamage)
            {
                ProcessReactionHit();
            } else if(m_em.shouldAttack && m_em.shouldMove) //move into attack position
            {
                m_player_target = m_em.GetZombieTarget();
                MoveAndAttack();
            } else if(m_em.shouldAttack && !m_em.shouldMove) //stand still and attack
            {
                m_player_target = m_em.GetZombieTarget();
                Attack(m_player_target);
                StandStill();
            } else if(!m_em.shouldAttack && !m_em.shouldMove) //stand still and don't attack
            {
                BeginIdleAnimation();
            } else if(!m_em.shouldAttack && m_em.shouldMove)
            {
                ContinueMoving();
            }
        }
    }

    public void MoveAndAttack()
    {
        m_anim.SetInteger("AnimState", m_move_state);
        m_anim.SetFloat("AttackSpeed", 2f / m_lower_anim_speed);
        m_anim.speed = m_lower_anim_speed;
        Attack(m_player_target);
    }

    public void SetPlayerTarget(GameObject player)
    {
        m_player_target = player;
    }

    public void AttackLanded()
    {
        if (m_player_target == null) { return; }

        float distance = Vector3.Distance(gameObject.transform.position, m_player_target.transform.position);

        if (distance < 3f)
        {
            Zombie z = (Zombie)m_cdc.character;
            m_player_target.GetComponentInParent<CharacterDataController>().character.DamageCharacter(z.GetDamage());
        }
    }

    //random death animation
    public void BeginDeathAnimation()
    {
        m_anim.SetLayerWeight(1, 0f);
        m_anim.SetBool("Alive", false);
        m_anim.SetInteger("AnimState", Random.Range(0, 2) * -1);
    }

    //begins playing idle animation at predefined settings in Awake()
    //no attack animation can be played after calling this function
    public void BeginIdleAnimation()
    {
        m_move_state = idle_anim;
        m_anim.SetInteger("AnimState", m_move_state);
        m_anim.speed = idle_speed;
        m_anim.SetLayerWeight(1, 0f);
    }

    //stops the movement animation, attack animation can happen after this call
    public void StandStill()
    {
        m_anim.SetInteger("AnimState", idle_anim);
        m_anim.speed = 1f;
        m_anim.SetFloat("AttackSpeed", 2f);
    }

    public void ContinueMoving()
    {
        if(m_WeightScalar > 0f)
        {
            m_WeightScalar -= Time.deltaTime * 2f;
            m_anim.SetLayerWeight(1, m_WeightScalar);
        }
        m_anim.SetInteger("AnimState", m_move_state);
        m_anim.SetFloat("AttackSpeed", 2f / m_lower_anim_speed);
        m_anim.speed = m_lower_anim_speed;
    }

    public void ProcessReactionHit()
    {
        m_WeightScalar = 1f;
        m_anim.SetLayerWeight(1, m_WeightScalar);
        m_anim.SetInteger("AttackState", -1);
        shouldTakeDamage = false;
    }

    public void TakeHitFromBullet()
    {
        shouldTakeDamage = true;
    }

    //player is the mesh object of the player GameObject, so related scripts are in the parent
    public void Attack(GameObject player)
    {
        if (m_player_target != player || CharacterMovement == null)
        {
            m_player_target = player;
            CharacterMovement = player.GetComponentInParent<CharacterMovement>();
        }

        float distance = Vector3.Distance(gameObject.transform.position, m_player_target.transform.position);

        if (m_WeightScalar < 1f)
        {
            if (m_WeightScalar < 0.1f)
            {
                m_attack_state = Random.Range(0, 3);
                m_anim.SetInteger("AttackState", m_attack_state);
            }
            m_WeightScalar += Time.deltaTime * 2f;
            m_anim.SetLayerWeight(1, m_WeightScalar);
        } else
        {
            m_anim.SetInteger("AttackState", m_attack_state);
        }

        //if (distance < 3f)
        //{
        //    if (!(CharacterMovement.GetMovementMagnitude() > 0f))
        //        StandStill();
        //}
    }

    public float SetRandomMovementAnim()
    {
        float agent_speed = 1f;

        m_move_state = Random.Range(2, 8);
        //m_anim.SetInteger("AnimState", m_attack_state);

        if (m_move_state == 2)
        {
            float multiplier = Random.Range(1f, 1.5f);
            m_lower_anim_speed = 2f * multiplier;
            agent_speed = 1.6f * multiplier;
        }
        else if(m_move_state == 3)
        {
            float multiplier = Random.Range(1.5f, 2f);
            m_lower_anim_speed = .5f * multiplier;
            agent_speed = 3f * multiplier;
        }
        else if (m_move_state == 4)
        {
            float multiplier = Random.Range(1.25f, 2f);
            m_lower_anim_speed = .5f * multiplier;
            agent_speed = 2.75f * multiplier;
        }
        else if (m_move_state == 5)
        {
            float multiplier = Random.Range(1f, 1.25f);
            m_lower_anim_speed = 1f * multiplier;
            agent_speed = 3f * multiplier;
        }
        else if (m_move_state == 6)
        {
            float multiplier = Random.Range(1f, 1.5f);
            m_lower_anim_speed = 1f * multiplier;
            agent_speed = 1.25f * multiplier;
        }
        else if (m_move_state == 7)
        {
            float multiplier = Random.Range(2f, 2.5f);
            m_lower_anim_speed = 1f * multiplier;
            agent_speed = .9f * multiplier;
        }
        else
        {
            float multiplier = Random.Range(.8f, 1.25f);
            m_lower_anim_speed = 0.75f * multiplier;
            agent_speed = 4.5f * multiplier;
        }

        //m_anim.SetFloat("AttackSpeed", 2f / m_anim.speed);

        return agent_speed;
    }

}
