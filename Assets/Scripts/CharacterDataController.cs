using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class should be used for controlling what a character should do given changes to their Character() object
//an example would be in the Update() function where the character dies if their health gets below 1
public class CharacterDataController : MonoBehaviour
{
    public Character character;
    private float HealthCheckTime = 0.2f;
    private AimTrigger at;
    private Collider c;

    void Start()
    {
        c = gameObject.GetComponent<Collider>();
        at = GameObject.Find("aim_angle").GetComponent<AimTrigger>();
        character = Character.CreateCharacter(name);
        StartCoroutine(CheckHealth());
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    //never called on mobile, probably doesn't matter
    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    IEnumerator CheckHealth()
    {
        while(true)
        {
            if (character.health <= 0 && character.char_name.StartsWith("zombie"))
            {
                at.RemoveFromList(c);
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(HealthCheckTime);
        }
    }

}
