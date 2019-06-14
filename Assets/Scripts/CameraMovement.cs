using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	// this is the transform used as a reference to move the camera
	//  these are the factors used to change the speed in which the camera follows the plane
	//public float posF,rotF;
    //public GameObject player;
    private Vector3 offset;
    private readonly Vector3 default_offset = new Vector3(0.2f, 18.6f, -13.2f);
    //private readonly Vector3 default_offset = new Vector3(0.2f, 17.9f, -17.9f);
    private Transform pos;
    //rotation = new Vector3(46.564f, 0f, 0f); //50.8?


	void Start ()
    {
        //offset = transform.position - player.transform.position;
        pos = GameObject.Find("player").transform.GetChild(0).transform; //player must be first in the hierarchy
        //Debug.Log(transform.position - pos.position);
	}
	
	void FixedUpdate () 
	{
        //transform.position = player.transform.position + default_offset;
        transform.position = pos.position + default_offset;
		//transform.position = Vector3.Lerp (transform.position, reference.position, posF);		
		//transform.rotation  = Quaternion.Lerp (transform.rotation, reference.rotation, rotF);
	}

}
