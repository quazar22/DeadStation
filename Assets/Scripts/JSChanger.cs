using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSChanger : MonoBehaviour
{
    public Joystick rs;
    private CharacterMovement cm;
    private CharacterMovementLEFTJOYSTICK cmlj;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CharacterMovement>();
        cmlj = GetComponent<CharacterMovementLEFTJOYSTICK>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableStick()
    {
        Color color = new Color();
        if (rs.enabled)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = 1.0f; color.b = 1.0f; color.g = 1.0f; color.r = 1.0f;
        }
        rs.bgImg.color = color;
        rs.joystickimg.color = color;
        rs.enabled = !rs.enabled;
        cm.enabled = !cm.enabled;
        cmlj.enabled = !cmlj.enabled;
    }

}
