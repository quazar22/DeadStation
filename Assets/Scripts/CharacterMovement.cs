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
    private float speed = 5.0f;
    private float interpspeed = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        interpspeed = cdc.character.GetInterpSpeed();
        speed = cdc.character.GetMoveSpeed();
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
        Vector3 movement;
        if (x2 != 0f && y2 != 0f)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x, Mathf.Atan2(x2, y2) * Mathf.Rad2Deg, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newvec), interpspeed);
        }

        movement = new Vector3(x1, 0, y1) * distance * speed;
        //movement.y = rb.velocity.y;
        //rb.velocity = movement;
        //pc.Move(movement);
        pc.SimpleMove(movement);
    }

    //private void LateUpdate()
    //{
    //    speed = cdc.character.GetMoveSpeed();
    //}
    //maybe use later in case I want to change move speed?
    //should probably just change it once instead of every frame though
}
