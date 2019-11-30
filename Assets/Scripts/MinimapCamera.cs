using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(Character.PLAYER).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //get x and z
        Vector3 pos = player.position;
        gameObject.transform.position = new Vector3(player.transform.position.x, 80.5f, player.transform.position.z);
    }
}
