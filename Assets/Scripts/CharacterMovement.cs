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

        float distance = Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2));
        Vector3 movement = new Vector3(x1, 0, y1);
        if (x2 != 0f && y2 != 0f)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x, Mathf.Atan2(x2, y2) * Mathf.Rad2Deg, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newvec), interpspeed);
        }

        if (distance != 0f)
        {
            float y_component_radians = gameObject.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float cos = Mathf.Cos(y_component_radians);
            float sin = Mathf.Sin(y_component_radians);
            float direction_y = y1 * cos + x1 * sin; //positive means forward movement, negative means backward
            float direction_x = x1 * cos - y1 * sin; //negative means strafe left, positive means strafe right
            anim.SetFloat("direction_y", direction_y);
            anim.SetFloat("direction_x", direction_x);
        } else
        {
            anim.SetFloat("direction_y", 0);
            anim.SetFloat("direction_x", 0);
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
