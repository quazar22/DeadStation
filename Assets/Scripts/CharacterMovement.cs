using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//movement and player animation handler
public class CharacterMovement : MonoBehaviour
{
    public JoystickLocationChanger leftstick;
    public JoystickLocationChanger rightstick;

    private CharacterController pc;
    private CharacterDataController cdc;

    private float interpspeed = 1f;
    private Animator anim; //potentially replace with a new class called AnimationController
    private bool shouldWalk;
    public bool canMove;
    //private Transform aim_angle;
    Vector3 movement;

    private Transform front;

    //each animation runs at 30fps
    private readonly float RunForward = 9f; //22 frames, 2 steps, 0.733 seconds (30fps / (22frames / 2 strides))  = 2.72 strides/second * 2.661 units/stride = 7.237 units/second
    private readonly float WalkForward = 4.55f; //30 frames, 2 steps, 1 second     (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.275 units/stride = 4.55 units/second
    private readonly float RunBackward = 9f; //16 frames, 2 steps, 0.533 seconds (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.680 units/stride = 6.3 units/second
    private readonly float WalkBackward = 4.468f; //30 frames, 2 steps, 1 second    (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.234 units/stride = 4.468 units/second

    void Start()
    {
        Application.targetFrameRate = 30;
        shouldWalk = false;
        canMove = true;
        pc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        interpspeed = cdc.character.GetInterpSpeed();
        anim = GetComponentInChildren<Animator>();
        front = GameObject.Find("player/front").GetComponent<Transform>();
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

        float left_stick_distance = Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2));
        movement = new Vector3(x1, 0, y1);

        if (x2 != 0f && y2 != 0f && canMove)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x, Mathf.Atan2(x2, y2) * Mathf.Rad2Deg, transform.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newvec), 5f);
        }

        if (left_stick_distance != 0f && canMove)
        {
            float y_component_radians = gameObject.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float cos = Mathf.Cos(y_component_radians);
            float sin = Mathf.Sin(y_component_radians);
            float direction_y = y1 * cos + x1 * sin; //positive means forward movement, negative means backward
            float direction_x = x1 * cos - y1 * sin; //negative means strafe left, positive means strafe right

            float angle = Mathf.Atan2(direction_y, direction_x) * Mathf.Rad2Deg;

            if (shouldWalk)
            {
                left_stick_distance = 0.5f;
            }

            anim.SetFloat("direction_y", direction_y);
            anim.SetFloat("direction_x", direction_x);
            anim.SetFloat("angle", angle);
            anim.SetFloat("distance", left_stick_distance);

            float walking_magnitude = GetWalkingMagnitude2(angle, left_stick_distance);

            movement = ClampMagnitude(movement.normalized * walking_magnitude, 9f, 3f);

            if (walking_magnitude > 7.5f)
            {
                anim.speed = 0.75f;
                movement *= 0.75f;
            }
            else
            {
                anim.speed = 1f;
            }

            if (left_stick_distance <= 0.5f)
            {
                anim.speed = left_stick_distance * 2f;
                anim.speed = Mathf.Clamp(anim.speed, 0.5f, 1f);
                anim.SetFloat("AnimMultiplier", 1f / anim.speed);
                movement *= anim.speed;
            }

            RotateLowerBody(angle);

            pc.SimpleMove(movement);
        }
        else
        {
            anim.SetFloat("direction_y", 0);
            anim.SetFloat("direction_x", 0);
            anim.SetFloat("angle", 0);
            anim.SetFloat("distance", 0);
            pc.SimpleMove(new Vector3(0f, 0f, 0f));
        }

        //Debug.Log(anim.GetFloat("distance"));

    }

    public float GetMovementMagnitude()
    {
        return movement.magnitude;
    }

    public void RotateUpperBody()
    {
        if (anim)
        {
            anim.SetLookAtWeight(1f, 1f, 1f, 1f, .5f);
            anim.SetLookAtPosition(front.position);
        }
    }

    public void RotateLowerBody(float angle)
    {
        if(angle < 180f && angle > 0f)
        {
            anim.transform.rotation = Quaternion.RotateTowards(anim.transform.rotation, Quaternion.LookRotation(movement), 5f);
        } else if(angle > -180f && angle < 0f)
        {
            anim.transform.rotation = Quaternion.RotateTowards(anim.transform.rotation, Quaternion.LookRotation(-movement), 5f);
        }
    }

    //simplified, but prettier version
    float GetWalkingMagnitude2(float angle, float distance)
    {
        float outFloat = 0f;

        if(angle <= 181f && angle >= 0f)
        {
            outFloat = distance > 0.5f ? RunForward : WalkForward;
        } else
        {
            outFloat = distance > 0.5f ? RunBackward : WalkBackward;
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

}
