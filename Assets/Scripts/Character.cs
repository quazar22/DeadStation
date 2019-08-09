using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

//generic attributes and animations handling and storage
abstract public class Character
{
    static public string ZOMBIE = "zombie";
    static public string PLAYER = "player";
    static public string BOSS = "boss";

    public int m_health;
    public float m_interpspeed;
    public string m_char_name;
    public bool m_isStanding;
    public GameObject m_playerobject;
    public Animator m_anim;
    public NavMeshAgent m_nma;
    public CharacterController m_cc;
    public bool m_isAlive;

    public void HealCharacter(int heal)         { m_health += heal; }
    public float GetInterpSpeed()               { return m_interpspeed; }
    public int GetHealth()                      { return m_health; }

    public static Character CreateCharacter(string name, GameObject go)
    {
        Character return_character;

        if (name.StartsWith(ZOMBIE))
        {
            return_character = new Zombie(go);
        } else if(name.StartsWith(PLAYER))
        {
            return_character = new Player(go);
        } else
        {
            return_character = new Boss(go);
        }

        return_character.m_anim = go.GetComponentInChildren<Animator>();
        return_character.m_nma = go.GetComponent<NavMeshAgent>();
        return_character.m_cc = go.GetComponent<CharacterController>();
        return_character.m_isAlive = true;

        return return_character;
    }

    abstract public void Die();
    abstract public void DamageCharacter(int damage);

}

public class Zombie : Character
{
    private int damage;
    private ZombieAnimationManager zam;
    private EnemyMovement em;

    public Collider[] colliders;
    public Rigidbody[] rigidbodies;

    public Zombie(GameObject character_object)
    {
        m_playerobject = character_object;
        zam = m_playerobject.GetComponentInChildren<ZombieAnimationManager>();
        em = m_playerobject.GetComponent<EnemyMovement>();
        damage = Random.Range(8, 14);
        m_char_name = "zombie";
        m_health = 100;
        m_interpspeed = 0.05f;
        m_isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        m_health -= Mathf.Abs(damage);
        zam.TakeHitFromBullet();
        if(m_health <= 0)
        {
            Die();
        }
    }
    
    public override void Die()
    {
        zam.BeginDeathAnimation();
        em.shouldMove = false;
        em.shouldAttack = false;
        m_nma.speed = 0;
        m_nma.enabled = false;
        m_cc.enabled = false;
        m_cc.detectCollisions = false;
        m_isAlive = false;
        Object.Destroy(m_playerobject, 10f);
    }

    public int GetDamage()
    {
        return damage;
    }
    
}

public class Player : Character
{

    private CharacterAnimationManager cam;
    private WeaponManager wm;
    private CharacterMovement cm;

    public Player(GameObject character_object)
    {
        m_playerobject = character_object;
        cam = m_playerobject.GetComponentInChildren<CharacterAnimationManager>();
        wm = m_playerobject.GetComponent<WeaponManager>();
        cm = m_playerobject.GetComponent<CharacterMovement>();
        m_char_name = "player";
        m_health = 100;
        m_interpspeed = 0.02f;
        m_isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        m_health -= 0;
        cam.TakeDamage();
        if(m_health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        cm.canMove = false;
        cam.BeginDeathAnimation();
        wm.CanFire = false;
        m_isAlive = false;
    }
}

public class Boss : Character
{

    public Boss(GameObject character_object)
    {
        m_playerobject = character_object;
        m_char_name = "boss";
        m_health = 5000;
        m_interpspeed = 0.2f;
        m_isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        m_isAlive = false;
        throw new System.NotImplementedException();
    }
}