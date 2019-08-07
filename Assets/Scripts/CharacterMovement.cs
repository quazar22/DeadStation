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
    private float speed = 2.0f;

    private float interpspeed = 1f;
    private Animator anim; //potentially replace with a new class called AnimationController
    private bool shouldWalk;
    public bool canMove;
    private Transform aim_angle;
    Vector3 movement;

    //each animation runs at 30fps
    private float RunForward = 9f; //22 frames, 2 steps, 0.733 seconds (30fps / (22frames / 2 strides))  = 2.72 strides/second * 2.661 units/stride = 7.237 units/second
    private float WalkForward = 4.55f; //30 frames, 2 steps, 1 second     (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.275 units/stride = 4.55 units/second
    private float RunBackward = 9f; //16 frames, 2 steps, 0.533 seconds (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.680 units/stride = 6.3 units/second
    private float WalkBackward = 4.468f; //30 frames, 2 steps, 1 second    (30 fps / (30frames / 2 strides)) = 2.00 strides/second * 2.234 units/stride = 4.468 units/second

    private float RunStrafeRight = 9f; //16 frames, 2 steps, 0.533 seconds   (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.818 units/stride = 6.8175 units/second
    private float RunStrafeLeft = 9f; //16 frames, 2 steps, 0.533 seconds    (30 fps / (16frames / 2 strides)) = 3.75 strides/second * 1.814 units/stride = 6.8025 units/second
    private float WalkStrafeLeft = 4.55f; //31 frames, 2 steps, 1.033 seconds   (30 fps / (31frames / 2 strides)) = 1.93 strides/second * 1.867 units/stride = 3.6033 units/second
    private float WalkStrafeRight = 4.55f; //43 seconds, 2 steps, 1.433 seconds (30 fps / (43frames / 2 strides)) = 1.40 strides/second * 1.583 units/stride = 2.2162 units/second

    private float RunForwardLeft = 10.636f; //15 frames, 2 steps, 0.5 seconds     (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.659 units/stride = 10.636 units/second
    private float RunForwardRight = 10.132f; //15 frames, 2 steps, 0.5 seconds    (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.533 units/stride = 10.132 units/second
    private float WalkForwardRight = 4.774f; //30 frames, 2 steps, 1 second      (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.387 units/stride = 4.774 units/second
    private float WalkForwardLeft = 4.710f; //30 frames, 2 steps, 1 second       (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.355 units/stride = 4.710 units/second

    private float RunBackwardLeft = 9.640f; //15 frames, 2 steps, 0.5 seconds     (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.410 units/stride = 9.640 units/second
    private float RunBackwardRight = 8.336f; //15 frames, 2 steps, 0.5 seconds   (30 fps / (15frames / 2 strides)) = 4 strides/second * 2.084 units/stride = 8.336 units/second
    private float WalkBackwardRight = 4.712f; //30 frames, 2 steps, 1 second     (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.356 units/stride = 4.712 units/second
    private float WalkBackwardLeft = 4.670f; //30 frames, 2 steps, 1 second      (30 fps / (30frames / 2 strides)) = 2 strides/second * 2.335 units/stride = 4.67 units/second

    void Start()
    {
        Application.targetFrameRate = 30;
        shouldWalk = false;
        canMove = true;
        pc = GetComponent<CharacterController>();
        cdc = GetComponent<CharacterDataController>();
        interpspeed = cdc.character.GetInterpSpeed();
        speed = cdc.character.GetMoveSpeed();
        anim = GetComponentInChildren<Animator>();
        aim_angle = GameObject.Find("player/aim_cone_blue").GetComponent<Transform>();
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
        movement = new Vector3(x1, 0, y1);

        if (x2 != 0f && y2 != 0f && canMove)
        {
            Vector3 newvec = new Vector3(transform.eulerAngles.x, Mathf.Atan2(x2, y2) * Mathf.Rad2Deg, transform.eulerAngles.z);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newvec), interpspeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newvec), 5f);
        }

        if (distance != 0f && canMove)
        {
            float y_component_radians = gameObject.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            float cos = Mathf.Cos(y_component_radians);
            float sin = Mathf.Sin(y_component_radians);
            float direction_y = y1 * cos + x1 * sin; //positive means forward movement, negative means backward
            float direction_x = x1 * cos - y1 * sin; //negative means strafe left, positive means strafe right

            float angle = Mathf.Atan2(direction_y, direction_x) * Mathf.Rad2Deg;

            if (shouldWalk)
            {
                distance = 0.5f;
            }

            anim.SetFloat("direction_y", direction_y);
            anim.SetFloat("direction_x", direction_x);
            anim.SetFloat("angle", angle);
            anim.SetFloat("distance", distance);

            float walking_magnitude = GetWalkingMagnitude2(angle, distance);

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

            if (distance <= 0.5f)
            {
                anim.speed = distance * 2f;
                anim.speed = Mathf.Clamp(anim.speed, 0.5f, 1f);
                anim.SetFloat("AnimMultiplier", 1f / anim.speed);
                //movement *= distance * 2f;
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

    }

    public float GetMovementMagnitude()
    {
        return movement.magnitude;
    }

    public void RotateUpperBody()
    {
        anim.SetLookAtWeight(1f, 1f, 1f, 1f, .5f);
        anim.SetLookAtPosition(aim_angle.position);
    }

    public void RotateLowerBody(float angle)
    {
        if(angle < 180 && angle > 0f)
        {
            //anim.transform.rotation = Quaternion.LookRotation(movement);
            anim.transform.rotation = Quaternion.RotateTowards(anim.transform.rotation, Quaternion.LookRotation(movement), 5f);
        } else if(angle > -180f && angle < 0f)
        {
            //anim.transform.rotation = Quaternion.LookRotation(-movement);
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

    //returns movement in units/second
    float GetWalkingMagnitude(float angle, float distance)
    {
        float outFloat = 0f;
        if(angle > 75f && angle < 105f)
        {
            outFloat = distance > 0.5f ? RunForward : WalkForward;
        } else if(angle < 15f && angle > -15f)
        {
            outFloat = distance > 0.5f ? RunStrafeRight : WalkStrafeRight;
        } else if(angle > 15f && angle < 75f)
        {
            outFloat = distance > 0.5f ? RunForwardRight : WalkForwardRight;
        } else if(angle > 105f && angle < 165f)
        {
            outFloat = distance > 0.5f ? RunForwardLeft : WalkForwardLeft;
        } else if(angle > 165f || angle < -165f)
        {
            outFloat = distance > 0.5f ? RunStrafeLeft : WalkStrafeLeft;
        } else if(angle > -165f && angle < -105f)
        {
            outFloat = distance > 0.5f ? RunBackwardLeft : WalkBackwardLeft;
        } else if(angle > -105f && angle < -75f)
        {
            outFloat = distance > 0.5f ? RunBackward : WalkBackward;
        } else if(angle > -75f && angle < -15f)
        {
            outFloat = distance > 0.5f ? RunBackwardRight : WalkBackwardRight;
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
