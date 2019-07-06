using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParenting : MonoBehaviour
{
    public Transform rightgrip;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(rightgrip);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
