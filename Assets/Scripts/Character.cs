using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public void DamageCharacter(int damage)     { health -= damage; }
    public void HealCharacter(int heal)         { health += heal; }
    public float GetInterpSpeed()               { return interpspeed; }
    public float GetMoveSpeed()                 { return movespeed; }
    public int GetHealth()                      { return health; }

    public static Character CreateCharacter(string name, GameObject go)
    {
        Character return_character;

        if(name.StartsWith(ZOMBIE))
        {
            return_character = new Zombie();
        } else if(name.StartsWith(PLAYER))
        {
            return_character = new Player();
        } else
        {
            return_character = new Boss();
        }

        return_character.anim = go.GetComponentInChildren<Animator>();
        return_character.nma = go.GetComponent<NavMeshAgent>();
        return_character.cc = go.GetComponent<CharacterController>();

        return return_character;
    }

    //if a character falls down
    public void CycleFall()
    {

    }

    public void Die()
    {
        if (!(this is Player))
        {
            anim.SetInteger("AnimState", Random.Range(0, 2) * -1);
            nma.speed = 0;
            nma.enabled = false;
            cc.enabled = false;
        }
        anim.SetBool("Alive", false);
    }

}

public class Zombie : Character
{

    public Zombie()
    {
        char_name = "zombie";
        health = 100;
        interpspeed = 0.05f;
        movespeed = 2.5f;
        isStanding = true;
    }

}

public class Player : Character
{
    public Player()
    {
        char_name = "player";
        health = 1000;
        interpspeed = 0.02f;
        movespeed = 6.0f;
        isStanding = true;
    }

}

public class Boss : Character
{
    public Boss()
    {
        char_name = "boss";
        health = 5000;
        interpspeed = 0.2f;
        movespeed = 3.5f;
        isStanding = true;
    }

}