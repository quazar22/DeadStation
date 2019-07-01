using System.Collections;
using UnityEngine;

//this class should be used for controlling what a character should do given changes to their Character() object
//an example would be in the Update() function where the character dies if their health gets below 1
public class CharacterDataController : MonoBehaviour
{
    public Character character;
    private float HealthCheckTime = 0.2f;
    private AimTrigger at;
    private Collider c;
    private Animator anim;
    private Coroutine healthCheck;

    void Start()
    {
        c = gameObject.GetComponent<Collider>();
        anim = GetComponentInChildren<Animator>();
        at = GameObject.Find("aim_angle").GetComponent<AimTrigger>();
        character = Character.CreateCharacter(name, gameObject);
        healthCheck = StartCoroutine("CheckHealth");
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
            //if (character.health <= 0 && character is Zombie)
            if (character.health <= 0)
            {
                at.RemoveFromList(c);
                character.Die();
                StopCoroutine("CheckHealth");
                Destroy(gameObject, 10f);
            }
            yield return new WaitForSeconds(HealthCheckTime);
        }
    }

}
