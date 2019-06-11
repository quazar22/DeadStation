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
    private Animator anim; //potentially replace with a new class called AnimationController
    private Transform fire_position;
    private bool shouldWalk;
    private GameObject ll;
    private GameObject rl;


    private float RunForward = 2.661f;
    private float WalkForward = 2.275f;
    private float RunBackward = 1.68f;
    private float WalkBackward = 2.234f;

    private float RunStrafeRight = 1.818f;
    private float RunStrafeLeft = 1.814f;
    private float WalkStrafeLeft = 1.867f;
    private float WalkStrafeRight = 1.583f;

    private float RunForwardLeft = 2.659f;
    private float RunForwardRight = 2.533f;
    private float WalkForwardRight = 2.387f;
    private float WalkForwardLeft = 2.355f;

    private float RunBackwardLeft = 2.41f;
    private float RunBackwardRight = 2.084f;
    private float WalkBackwardRight = 2.356f;
    private float WalkBackwardLeft = 2.335f;

    void Start()
    {
        shouldWalk = false;
        //rb = GetComponent<Rigidbody>();
        pc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        interpspeed = cdc.character.GetInterpSpeed();
        speed = cdc.character.GetMoveSpeed();
        anim = GetComponentInChildren<Animator>();
        fire_position = gameObject.transform.Find("player/fire_position");
        ll = GameObject.Find("player/SpaceMan@Rifle Aiming Idle/mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot/mixamorig:LeftToeBase");
        rl = GameObject.Find("player/SpaceMan@Rifle Aiming Idle/mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot/mixamorig:RightToeBase");
      
    }

    void Update()
    {
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward Left"));
    }

    private void FixedUpdate()
    {
        float x1 = leftstick.Horizontal();
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
            if(shouldWalk)
            {
                direction_y = Mathf.Clamp(direction_y, -0.5f, 0.5f);
                direction_x = Mathf.Clamp(direction_x, -0.5f, 0.5f);
            }
            anim.SetFloat("direction_y", direction_y);
            anim.SetFloat("direction_x", direction_x);
        } else
        {
            anim.SetFloat("direction_y", 0);
            anim.SetFloat("direction_x", 0);
        }
        //Debug.Log("step distance = " + Vector3.Distance(ll.transform.position, rl.transform.position));
        //joystick_distance(float) * speed(float) * joystick_direction(vector)?
        movement *= speed * distance;
        //if(distance <= 0.5f)
        //{
        //    anim.speed = distance * 2f;
        //}
        Debug.Log("x1 = " + x1);
        Debug.Log("y1 = " + y1);
        pc.SimpleMove(movement);
    }

    float GetWalkingMagnitude()
    {
        float outFloat = 0f;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward"))
        {
            outFloat = RunForward;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Rifle Walk Forward"))
        {
            outFloat = WalkForward;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward"))
        {
            outFloat = WalkBackward;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward"))
        {
            outFloat = RunBackward;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward Left"))
        {
            outFloat = RunForwardLeft;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward Right"))
        {
            outFloat = RunForwardRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Forward Left"))
        {
            outFloat = WalkForwardLeft;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Forward Right"))
        {
            outFloat = WalkForwardRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Strafe Left"))
        {
            outFloat = WalkStrafeLeft;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Strafe Right"))
        {
            outFloat = WalkStrafeRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Strafe Right"))
        {
            outFloat = RunStrafeRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Strafe Left"))
        {
            outFloat = RunStrafeLeft;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward Left"))
        {
            outFloat = WalkBackwardLeft;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward Right"))
        {
            outFloat = WalkBackwardRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward Right"))
        {
            outFloat = RunBackwardRight;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward Left"))
        {
            outFloat = RunBackwardLeft;
        }
        return outFloat;
    }

    public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > max * max)
        {
            return v.normalized * max;
        }
        else if (sm < min * min)
        {
            return v.normalized * min;
        }
        return v;
    }

    //private void LateUpdate()
    //{
    //    speed = cdc.character.GetMoveSpeed();
    //}
    //maybe use later in case I want to change move speed?
    //should probably just change it once instead of every frame though
}
