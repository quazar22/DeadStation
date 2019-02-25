using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLaser : MonoBehaviour
{
    private LineRenderer lr;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = GameObject.Find(Character.char_names[1]);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, player.transform.position);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.black);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                Debug.Log("hit something");
            }
        }
        else
            lr.SetPosition(1, player.transform.position);
    }
}
