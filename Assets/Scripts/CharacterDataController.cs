using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class should be used for controlling what a character should do given changes to their Character() object
//an example would be in the Update() function where the character dies if their health gets below 1
public class CharacterDataController : MonoBehaviour
{
    public Character character;
    private float HealthCheckTime = 0.2f;

    void Start()
    {
        character = Character.CreateCharacter(name);
    }

    void Update()
    {
        StartCoroutine(CheckHealth());
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator CheckHealth()
    {
        while(true)
        {
            if (character.health <= 0)
            {
                GameObject.Find("aim_angle").GetComponent<AimTrigger>().GetColliderList().Remove(gameObject.GetComponent<Collider>());
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(HealthCheckTime);
        }
    }

}
