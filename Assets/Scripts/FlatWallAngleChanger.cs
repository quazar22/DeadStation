using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatWallAngleChanger : MonoBehaviour
{
    private static CameraMovement mainCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        if(mainCamera == null)
            mainCamera = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        mainCamera.GoToOverheadRef();
    }


    private void OnTriggerExit(Collider other)
    {
        mainCamera.GoToAngledRef();
    }

}
