using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementLEFTJOYSTICK : MonoBehaviour
{
    public Joystick leftstick;
    public Joystick rightstick;
    private Rigidbody rb;
    private CharacterController pc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float x = leftstick.Horizontal() * -1;
        float y = leftstick.Vertical();
        float distance = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        Vector3 movement;
        if (x != 0f && y != 0f)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x,
                                                Mathf.Atan2(x, y) * Mathf.Rad2Deg,
                                                transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newvec), .8f);
        }

        movement = transform.forward * distance * 5;
        //movement.y = rb.velocity.y;
        //rb.velocity = movement;

        pc.SimpleMove(movement);
    }
}
