using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 offset;
    private readonly Vector3 default_offset = new Vector3(0.2f, 18.6f, -13.2f);
    private readonly Vector3 default_offset1 = new Vector3(0.2f, 13.5f, -11.4f);
    private readonly Vector3 default_offset2 = new Vector3(0.5f, 16.3f, -13f);
    private Transform pos;

    private Transform refCam;

	void Start ()
    {
        pos = GameObject.Find(Character.PLAYER).transform.GetChild(0).transform; //player object must be first in the hierarchy
        //pos = GameObject.Find(Character.PLAYER).transform;
        refCam = GameObject.Find(Character.PLAYER).transform.Find("refCam");
	}
	
	void FixedUpdate () 
	{
        transform.position = pos.position + default_offset2;
	}

}
