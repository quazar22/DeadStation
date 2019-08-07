﻿using System.Collections;
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

    public int health;
    public float movespeed;
    public float interpspeed;
    public string char_name;
    public bool isStanding;
    public GameObject playerobject;
    public Animator anim;
    public NavMeshAgent nma;
    public CharacterController cc;
    public bool isAlive;

    //public CharacterAnimationManager cam = null;
    //public ZombieAnimationManager zam = null;

    public void HealCharacter(int heal)         { health += heal; }
    public float GetInterpSpeed()               { return interpspeed; }
    public float GetMoveSpeed()                 { return movespeed; }
    public int GetHealth()                      { return health; }

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

        return_character.anim = go.GetComponentInChildren<Animator>();
        return_character.nma = go.GetComponent<NavMeshAgent>();
        return_character.cc = go.GetComponent<CharacterController>();
        return_character.isAlive = true;

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
        playerobject = character_object;
        zam = playerobject.GetComponentInChildren<ZombieAnimationManager>();
        em = playerobject.GetComponent<EnemyMovement>();
        damage = Random.Range(8, 14);
        char_name = "zombie";
        health = 100;
        interpspeed = 0.05f;
        movespeed = 2.5f;
        isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        health -= Mathf.Abs(damage);
        zam.TakeHitFromBullet();
    }
    
    public override void Die()
    {
        zam.BeginDeathAnimation();
        em.shouldMove = false;
        em.shouldAttack = false;
        nma.speed = 0;
        nma.enabled = false;
        cc.enabled = false;
        isAlive = false;
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
        playerobject = character_object;
        cam = playerobject.GetComponentInChildren<CharacterAnimationManager>();
        wm = playerobject.GetComponent<WeaponManager>();
        cm = playerobject.GetComponent<CharacterMovement>();
        char_name = "player";
        health = 100;
        interpspeed = 0.02f;
        movespeed = 6.0f;
        isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        health -= 0;
        cam.TakeDamage();
    }

    public override void Die()
    {
        cm.canMove = false;
        cam.BeginDeathAnimation();
        wm.CanFire = false;
        isAlive = false;
    }
}

public class Boss : Character
{

    public Boss(GameObject character_object)
    {
        playerobject = character_object;
        char_name = "boss";
        health = 5000;
        interpspeed = 0.2f;
        movespeed = 3.5f;
        isStanding = true;
    }

    public override void DamageCharacter(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        isAlive = false;
        throw new System.NotImplementedException();
    }
}