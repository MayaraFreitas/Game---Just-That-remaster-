using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float z;
    private Vector3 offset; //Private variable to store the offset distance between the player and camera

    void Update()
    {
        offset = transform.position - player.transform.position;
        //transform.position = player.transform.position + new Vector3(0, (offset.y * 0.2f), offset.x);
        transform.position = player.transform.position + new Vector3(0, (offset.y * 0.2f), z);
    }
}
