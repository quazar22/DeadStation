using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataController : MonoBehaviour
{
    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = Character.CreateCharacter(name);
    }

    // Update is called once per frame
    void Update()
    {
        if(character.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
