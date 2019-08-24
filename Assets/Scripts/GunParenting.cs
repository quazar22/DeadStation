using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParenting : MonoBehaviour
{
    public Transform grip;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(grip);
    }

}
