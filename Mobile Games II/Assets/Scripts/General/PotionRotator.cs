using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRotator : MonoBehaviour
{
    //rotate axis and speed
    public Vector3 rot;

    void Update()
    {
        //rotate potion
        transform.Rotate(rot * Time.deltaTime);
    }
}
