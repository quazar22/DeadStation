/**
 * This class controls the animations for the player character,
 * it mostly reads movement data from the CharacterMovement class
 * and then reacts accordingly
 * 
 * */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class CharacterAnimationManager : MonoBehaviour
{
    //some of these functions are called from animation events, so their use might not be obvious
    private Animator m_anim;
    private CharacterDataController m_cdc;
    private WeaponManager m_wm;
    private Transform aim_angle;
    private CharacterMovement m_cm;

    private GameObject grenade;
    private GameObject grenade_in_hand;
    //private AimTrigger m_at;
    private Stopwatch m_st;

    private bool isThrowing;
    private bool shouldTakeDamage;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_cdc = transform.parent.GetComponent<CharacterDataController>();
        m_wm = transform.parent.GetComponent<WeaponManager>();
        m_cm = transform.parent.GetComponent<CharacterMovement>();
        m_st = new Stopwatch();

        m_anim.SetFloat("AnimMultiplier", 1f);

        grenade = Resources.Load<GameObject>("Prefabs/Weapons/grenade");
        isThrowing = false;
        shouldTakeDamage = false;
    }

    private void FixedUpdate()
    {
        if (m_wm.isShooting && !isThrowing && !shouldTakeDamage)
        {
            BeginShooting();
        }
        else if (shouldTakeDamage && !isThrowing)
        {
            BeginDamageAnimation();
        }
        else if (isThrowing)
        {
            BeginThrowingAnimation();
        }
        else if (!m_wm.isShooting && !isThrowing && !shouldTakeDamage)
        {
            ResetToIdle();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        m_cm.RotateUpperBody();
    }

    IEnumerator FadeLayer(int layer)
    {
        while(m_anim.GetLayerWeight(layer) > 0f)
        {
            float lw = m_anim.GetLayerWeight(layer);
            m_anim.SetLayerWeight(layer, lw -= 4 * m_anim.GetFloat("AnimMultiplier") * Time.deltaTime);
            yield return new WaitForSeconds(0.016f);
        }
    }

    IEnumerator IncreaseLayerWeight(int layer)
    {
        while (m_anim.GetLayerWeight(layer) < 1f)
        {
            float lw = m_anim.GetLayerWeight(layer);
            m_anim.SetLayerWeight(layer, lw += 4 * Time.deltaTime);
            yield return new WaitForSeconds(0.016f);
        }
    }

    public void ToggleThrow()
    {
        if (isThrowing)
        {
            StartCoroutine("FadeLayer", 2);
        }
        isThrowing = !isThrowing;
    }

    public void ThrowGrenade()
    {
        if (!isThrowing)
        {
            isThrowing = true;
            m_anim.SetFloat("AnimMultiplier", 1f / m_anim.speed);
            Transform pos = gameObject.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandMiddle1");
            grenade_in_hand = Instantiate(grenade, pos.transform.position, Quaternion.identity, pos.transform);
            grenade_in_hand.transform.localPosition += new Vector3(0.04f, 0f);
        }
    }

    public void ReleaseGrenade()
    {
        m_anim.SetInteger("UpperBodyAnimState", 0);
        grenade_in_hand.transform.parent = null;
        grenade_in_hand.AddComponent<GrenadeScript>();
        grenade_in_hand = null;
    }

    public void BeginThrowingAnimation()
    {
        m_anim.SetLayerWeight(2, 1f);
        m_anim.SetInteger("UpperBodyAnimState", 4);
    }

    public void BeginDeathAnimation()
    {
        m_anim.speed = 1f;
        m_anim.SetLayerWeight(1, 0f);
        m_anim.SetLayerWeight(2, 0f);
        m_anim.SetBool("Alive", false);
    }

    public void ResetToIdle()
    {
        m_anim.SetInteger("UpperBodyAnimState", 0);
        shouldTakeDamage = false;
        m_wm.shot_count = 0;
    }

    public void BeginShooting()
    {
        Weapon weapon = m_wm.GetCurrentWeapon();

        if (m_anim.GetLayerWeight(1) <= 0f)
        {
            StartCoroutine("IncreaseLayerWeight", 1);
        }
        
        m_anim.SetInteger("UpperBodyAnimState", weapon.recoilCount);
        m_anim.SetFloat("AnimMultiplier", 1f / m_anim.speed);
    }

    public void StopSingleShot()
    {
        m_anim.SetInteger("UpperBodyAnimState", 0);
    }

    public void BeginDamageAnimation()
    {
        if(m_st.ElapsedMilliseconds >= 590f)
        {
            m_anim.SetInteger("UpperBodyAnimState", 0);
            shouldTakeDamage = false;
            m_st.Stop();
            m_st.Reset();
        } else {
            m_anim.SetInteger("UpperBodyAnimState", 3);
        }
    }

    public void TakeDamage()
    {
        if (!m_st.IsRunning)
            m_st.Start();
        shouldTakeDamage = true;
    }

}
