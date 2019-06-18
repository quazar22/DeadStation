﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Character
{
    static public string ZOMBIE = "zombie";
    static public string PLAYER = "player";
    static public string BOSS = "boss";

    public int health;
    public float movespeed;
    public float interpspeed;
    public List<Animation> animations;
    public string char_name;
    public GameObject playerobject;

    public void DamageCharacter(int damage)     { health -= damage; }
    public void HealCharacter(int heal)         { health += heal; }
    public List<Animation> GetAnimationStates() { return animations; }
    public float GetInterpSpeed()               { return interpspeed; }
    public float GetMoveSpeed()                 { return movespeed; }
    public int GetHealth()                      { return health; }

    public static Character CreateCharacter(string name)
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

        return return_character;
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
    }

}

public class Player : Character
{
    public Player()
    {
        char_name = "player";
        health = 1000;
        interpspeed = 0.025f;
        movespeed = 6.0f;
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
    }
}