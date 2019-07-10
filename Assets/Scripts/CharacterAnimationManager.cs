using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    //some of these functions are called from animation events, so their use might not be obvious
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    public void BeginShooting()
    {

    }

    public void StopSingleShot()
    {
        anim.SetInteger("UpperBodyAnimState", 0);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("hit here");
    }

}
