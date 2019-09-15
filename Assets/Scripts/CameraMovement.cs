using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 offset;
    private readonly Vector3 default_offset2 = new Vector3(0.5f, 16.3f, -13f);
    private Transform pos;

    private Transform OverheadReference;
    private Transform AngledReference;

    private Vector3 OverheadOffset;
    private Vector3 AngledOffset;

    private int CurrentLocation = 0;
    private Vector3 CurrentOffset;

	void Start ()
    {
        pos = GameObject.Find(Character.PLAYER).transform.GetChild(0).transform; //player object must be first in the hierarchy
        OverheadReference = GameObject.Find("player/overhead_camera_reference").GetComponent<Transform>();
        AngledReference = GameObject.Find("player/angled_camera_reference").GetComponent<Transform>();

        OverheadOffset = OverheadReference.position - pos.position;
        AngledOffset = AngledReference.position - pos.position;

        CurrentOffset = AngledOffset;

        CurrentLocation = 0;
        transform.LookAt(pos);
	}

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, pos.position + CurrentOffset, 0.1f);
        //transform.LookAt(pos);
    }

    void FixedUpdate () 
	{
        //transform.position = Vector3.MoveTowards(transform.position, pos.position + CurrentOffset, 0.1f);
        if (CurrentLocation == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos.position + AngledOffset, 0.1f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos.position + OverheadOffset, 0.1f);
        }
        transform.LookAt(pos);
    }

    public void GoToOverheadRef()
    {
        CurrentOffset = OverheadOffset;
        //CurrentLocation = OverheadReference.position;
        CurrentLocation = 1;
    }

    public void GoToAngledRef()
    {
        CurrentOffset = AngledOffset;
        //CurrentLocation = AngledReference.position;
        CurrentLocation = 0;
    }


}
