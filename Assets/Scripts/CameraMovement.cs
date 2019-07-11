using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 offset;
    private readonly Vector3 default_offset = new Vector3(0.2f, 18.6f, -13.2f);
    private readonly Vector3 default_offset1 = new Vector3(0.2f, 13.5f, -11.4f);
    private Transform pos;


	void Start ()
    {
        pos = GameObject.Find(Character.PLAYER).transform.GetChild(0).transform; //player object must be first in the hierarchy
        Debug.Log(transform.position - pos.position);
	}
	
	void FixedUpdate () 
	{
        transform.position = pos.position + default_offset1;
	}

}
