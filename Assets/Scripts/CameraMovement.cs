using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 offset;
    private readonly Vector3 default_offset2 = new Vector3(0.5f, 16.3f, -13f);
    private Transform pos;

    private Transform OverheadReference;
    private Transform AngledReference;

    private Vector3 CurrentLocation;

	void Start ()
    {
        pos = GameObject.Find(Character.PLAYER).transform.GetChild(0).transform; //player object must be first in the hierarchy
        OverheadReference = GameObject.Find("player/overhead_camera_reference").GetComponent<Transform>();
        AngledReference = GameObject.Find("player/angled_camera_reference").GetComponent<Transform>();
        CurrentLocation = pos.position + default_offset2;
        transform.LookAt(pos);
	}
	
	void FixedUpdate () 
	{
        transform.position = pos.position + default_offset2;
	}


}
