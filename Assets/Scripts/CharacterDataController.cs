using System.Collections;
using UnityEngine;

//I hate this class
public class CharacterDataController : MonoBehaviour
{
    public Character character;

    private void Awake()
    {
        character = Character.CreateCharacter(name, gameObject);
    }

}
