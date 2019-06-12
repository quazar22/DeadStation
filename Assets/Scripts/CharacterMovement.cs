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

    //each animation runs at 30fps
    private float RunForward = 2.661f; //22 frames, 2 steps, 0.733 seconds (30fps / (22frames / 2 strides))  = 2.72 strides/second * 2.661 units/stride = 7.237 units/second
    private float WalkForward = 2.275f; //30 frames, 2 steps, 1 second     (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.275 units/stride = 4.55 units/second
    private float RunBackward = 1.68f; //16 frames, 2 steps, 0.533 seconds (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.680 units/stride = 6.3 units/second
    private float WalkBackward = 2.234f; //30 frames, 2 steps, 1 second    (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.234 units/stride = 4.468 units/second

    private float RunStrafeRight = 1.818f; //16 frames, 2 steps, 0.533 seconds   (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.818 units/stride = 6.8175 units/second
    private float RunStrafeLeft = 1.814f; //16 frames, 2 steps, 0.533 seconds    (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.814 units/stride = 6.8025 units/second
    private float WalkStrafeLeft = 1.867f; //31 frames, 2 steps, 1.033 seconds   (30 fps / (31frames / 2 strides)) = 1.93 strides/second * 1.867 units/stride = 3.6033 units/second
    private float WalkStrafeRight = 1.583f; //43 seconds, 2 steps, 1.433 seconds (30 fps / (43frames / 2 strides)) = 1.40 strides/second * 1.583 units/stride = 2.2162 units/second

    private float RunForwardLeft = 2.659f; //15 frames, 2 steps, 0.5 seconds     (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.659 units/stride = 10.636 units/second
    private float RunForwardRight = 2.533f; //15 frames, 2 steps, 0.5 seconds    (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.533 units/stride = 10.132 units/second
    private float WalkForwardRight = 2.387f; //30 frames, 2 steps, 1 second      (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.387 units/stride = 4.774 units/second
    private float WalkForwardLeft = 2.355f; //30 frames, 2 steps, 1 second       (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.355 units/stride = 4.710 units/second

    private float RunBackwardLeft = 2.41f; //15 frames, 2 steps, 0.5 seconds     (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.410 units/stride = 9.640 units/second
    private float RunBackwardRight = 2.084f; //15 frames, 2 steps, 0.5 seconds   (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.084 units/stride = 8.336 units/second
    private float WalkBackwardRight = 2.356f; //30 frames, 2 steps, 1 second     (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.356 units/stride = 4.712 units/second
    private float WalkBackwardLeft = 2.335f; //30 frames, 2 steps, 1 second      (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.335 units/stride = 4.67 units/second

    public float deltaTime;
    public Vector3 fmovement;

    void Start()
    {
        Debug.Log("fr = " + Application.targetFrameRate);
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

            float angle = Mathf.Atan2(direction_y, direction_x) * Mathf.Rad2Deg;
            
            if (shouldWalk)
            {
                direction_y = Mathf.Clamp(direction_y, -0.5f, 0.5f);
                direction_x = Mathf.Clamp(direction_x, -0.5f, 0.5f);
            }
            anim.SetFloat("direction_y", direction_y);
            anim.SetFloat("direction_x", direction_x);
            anim.SetFloat("angle", angle);
            anim.SetFloat("distance", distance);
        }
        else
        {
            anim.SetFloat("direction_y", 0);
            anim.SetFloat("direction_x", 0);
            anim.SetFloat("angle", 0);
            anim.SetFloat("distance", 0);
        }
        //Debug.Log("step distance = " + Vector3.Distance(ll.transform.position, rl.transform.position));
        if(distance > 0.5f)
        {
            movement *= speed;
        } else if(distance < 0.5f && distance > 0f)
        {
            movement *= speed * 0.75f;
        }
        pc.SimpleMove(movement);
    }

    //returns movement in units/second
    float GetWalkingMagnitude()
    {
        float outFloat = 0f;
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward"))
        {
            outFloat = 7.237f;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Rifle Walk Forward"))
        {
            outFloat = 4.55f;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward"))
        {
            outFloat = 4.468f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward"))
        {
            outFloat = 6.3f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward Left"))
        {
            outFloat = 10.636f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Forward Right"))
        {
            outFloat = 10.132f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Forward Left"))
        {
            outFloat = 4.710f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Forward Right"))
        {
            outFloat = 4.774f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Strafe Left"))
        {
            outFloat = 3.6033f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Strafe Right"))
        {
            outFloat = 2.2162f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Strafe Right"))
        {
            outFloat = 6.8175f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Strafe Left"))
        {
            outFloat = 6.8025f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward Left"))
        {
            outFloat = 4.67f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk Backward Right"))
        {
            outFloat = 4.712f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward Right"))
        {
            outFloat = 8.336f;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run Backward Left"))
        {
            outFloat = 9.640f;
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
