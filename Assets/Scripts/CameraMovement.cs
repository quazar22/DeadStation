using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	// this is the transform used as a reference to move the camera
	public Transform reference;
	//  these are the factors used to change the speed in which the camera follows the plane
	//public float posF,rotF;
    public GameObject player;
    private Vector3 offset;

	void Start ()
    {
        //transform.rotation = reference.rotation;
        offset = transform.position - player.transform.position;
	}
	
	void FixedUpdate () 
	{
        // we use the lerp for changing the position and rotation of the camera
        transform.position = player.transform.position + offset;
		//transform.position = Vector3.Lerp (transform.position, reference.position, posF);		
		//transform.rotation  = Quaternion.Lerp (transform.rotation, reference.rotation, rotF);
	}
}
