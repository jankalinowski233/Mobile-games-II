using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //player reference
    public GameObject player;
    //initial camera position
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //lock camera position above player
        offset = transform.position - player.transform.position; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //make camera following the player
        transform.position = player.transform.position + offset; 
    }
}
