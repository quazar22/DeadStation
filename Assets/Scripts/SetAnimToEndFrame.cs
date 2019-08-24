using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimToEndFrame : MonoBehaviour
{
    Animator anim;
    public string anim_name;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
        anim.Play(anim_name, 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
