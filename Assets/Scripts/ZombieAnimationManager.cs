using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationManager : MonoBehaviour
{
    private Animator m_anim;
    private GameObject m_player_target = null;
    private CharacterDataController m_cdc;

    private CharacterMovement CharacterMovement = null;
    private Character target_character = null;

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

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_WeightScalar = 0f;
        m_cdc = GetComponentInParent<CharacterDataController>();
        idle_anim = Random.Range(0, 2);
        idle_speed = Random.Range(0.75f, 1.5f);
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
            target_character.DamageCharacter(z.GetDamage());
        }
    }

    //random death animation
    public void BeginDeathAnimation()
    {
        m_anim.SetBool("Alive", false);
        m_anim.SetInteger("AnimState", Random.Range(0, 2) * -1);
    }

    //assigns random idle animation and begins animation
    public void BeginIdleAnimation()
    {
        m_move_state = idle_anim;
        m_anim.SetInteger("AnimState", m_move_state);
        m_anim.speed = idle_speed;
        m_anim.SetLayerWeight(1, 0f);
    }

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

    //player is the mesh object of the player GameObject, so related scripts are in the parent
    public void Attack(GameObject player)
    {
        if (m_player_target != player || CharacterMovement == null || target_character == null)
        {
            m_player_target = player;
            CharacterMovement = player.GetComponentInParent<CharacterMovement>();
            target_character = player.GetComponentInParent<CharacterDataController>().character;
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
        }

        if (distance < 3f)
        {
            if (!(CharacterMovement.GetMovementMagnitude() > 0f))
                StandStill();
        }
    }

    public float SetRandomMovementAnim()
    {
        float agent_speed = 1f;

        m_move_state = Random.Range(2, 6);
        //m_anim.SetInteger("AnimState", m_attack_state);

        if (m_move_state == 2)
        {
            float multiplier = Random.Range(0.75f, 1.25f);
            m_lower_anim_speed = 2f * multiplier;
            agent_speed = 1.6f * multiplier;
        }
        else if (m_move_state == 5)
        {
            float multiplier = Random.Range(.75f, 1f);
            m_lower_anim_speed = 1f * multiplier;
            agent_speed = 4.5f * multiplier;
        }
        else if (m_move_state == 4)
        {
            float multiplier = Random.Range(.75f, 1.5f);
            m_lower_anim_speed = 1f * multiplier;
            agent_speed = 1.5f * multiplier;
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
