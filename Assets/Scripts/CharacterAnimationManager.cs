using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    //some of these functions are called from animation events, so their use might not be obvious
    private Animator m_anim;
    private CharacterDataController m_cdc;
    private WeaponManager m_wm;
    private Transform aim_angle;
    private CharacterMovement m_cm;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_cdc = transform.parent.GetComponent<CharacterDataController>();
        m_wm = transform.parent.GetComponent<WeaponManager>();
        m_cm = transform.parent.GetComponent<CharacterMovement>();
        aim_angle = GameObject.Find("player/aim_angle").GetComponent<Transform>();
    }

    
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        m_cm.RotateUpperBody();
    }

    private void FixedUpdate()
    {
        //HandleUpperBodyAnimations();
    }

    public void ThrowGrenade()
    {
        m_anim.SetInteger("UpperBodyAnimState", 4);
    }

    public void ReleaseGrenade()
    {
        m_anim.SetInteger("UpperBodyAnimState", 0);
    }

    public void BeginDeathAnimation()
    {
        m_anim.speed = 1f;
        m_anim.SetLayerWeight(1, 0f);
        m_anim.SetBool("Alive", false);
    }

    public void ResetToIdle()
    {
        int anim_state = m_anim.GetInteger("UpperBodyAnimState");
        if (anim_state == 3 && m_cdc.character.AnimPlayTime.ElapsedMilliseconds > 0.55f * 1000f)
        {
            m_anim.SetInteger("UpperBodyAnimState", 0);
            m_cdc.character.AnimPlayTime.Reset();
        } else if(anim_state == 1 || anim_state == 2)
        {
            m_anim.SetInteger("UpperBodyAnimState", 0);
        }
    }

    public void BeginShooting()
    {
        if (m_anim.GetInteger("UpperBodyAnimState") == 0 && m_wm.GetCurrentWeapon().timer.ElapsedMilliseconds >= m_wm.GetCurrentWeapon().rate_of_fire * 1000f)
        {
            m_anim.SetInteger("UpperBodyAnimState", m_wm.GetCurrentWeapon().recoilCount);
        }
        else if (m_anim.GetInteger("UpperBodyAnimState") == 0)
        {
            m_anim.SetInteger("UpperBodyAnimState", 0);
        }
    }

    public void StopSingleShot()
    {
        m_anim.SetInteger("UpperBodyAnimState", 0);
    }

    public void HandleUpperBodyAnimations()
    {
        if (m_anim.GetInteger("UpperBodyAnimState") == 3 && m_cdc.character.AnimPlayTime.ElapsedMilliseconds > 0.55f * 1000f)
        {
            m_anim.SetInteger("UpperBodyAnimState", 0);
            m_cdc.character.AnimPlayTime.Reset();
        }
    }

    public void TakeDamage()
    {
        m_anim.SetInteger("UpperBodyAnimState", 3);
    }

}
