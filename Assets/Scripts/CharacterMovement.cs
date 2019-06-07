using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Joystick leftstick;
    public Joystick rightstick;
    private Rigidbody rb;
    private CharacterController pc;
    private CharacterDataController cdc;
    private float speed = 2.0f;
    private float interpspeed = 0.2f;
    private Animator anim;
    private Transform fire_position;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        interpspeed = cdc.character.GetInterpSpeed();
        speed = cdc.character.GetMoveSpeed();
        anim = GetComponent<Animator>();
        fire_position = gameObject.transform.Find("fire_position");
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float x1 = leftstick.Horizontal() * -1F;
        float y1 = leftstick.Vertical();
        float x2 = rightstick.Horizontal();
        float y2 = rightstick.Vertical();

        Debug.Log("x1 = " + x1);
        Debug.Log("y1 = " + y1);

        float distance = Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2));
        Vector3 movement = new Vector3(x1, 0, y1);
        if (x2 != 0f && y2 != 0f)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x, Mathf.Atan2(x2, y2) * Mathf.Rad2Deg, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newvec), interpspeed);
        }
        //if y1 > 0 and aim_angle_forward y > 0, run forward  -- y1 * y = +
        //if y1 < 0 and aim_angle_forward y < 0, run forward  -- y1 * y = +
        //if y1 > 0 and aim_angle_forward y < 0, run backward -- y1 * y = -
        //if y1 < 0 and aim_angle_forward y > 0, run backward -- y1 * y = - 

        //1) decide if forward or backward movement
        //2) decide if running or walking
        //3) decide which one to play

        if (distance != 0f)
        {
            float direction = fire_position.transform.forward.y * y1; //positive means forward movement, negative means backward
            int speedMultiplier = distance >= 0.5f ? 2 : 1;
            int stateNum = Mathf.RoundToInt(direction) * speedMultiplier; //doesn't work like this
            
        } else
        {
            anim.SetInteger("AnimState", 0);
        }
        movement *= distance * speed;
        pc.SimpleMove(movement);

    }

    //private void LateUpdate()
    //{
    //    speed = cdc.character.GetMoveSpeed();
    //}
    //maybe use later in case I want to change move speed?
    //should probably just change it once instead of every frame though
}
