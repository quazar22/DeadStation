using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 offset;
    private readonly Vector3 default_offset = new Vector3(0.2f, 18.6f, -13.2f);
    private Transform pos;


	void Start ()
    {
        pos = GameObject.Find(Character.PLAYER).transform.GetChild(0).transform; //player object must be first in the hierarchy
	}
	
	void FixedUpdate () 
	{
        transform.position = pos.position + default_offset;
	}

}
