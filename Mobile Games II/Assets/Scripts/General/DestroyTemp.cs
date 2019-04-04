using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTemp : MonoBehaviour
{
    //invoke destroy on start
    private void Start()
    {
        Destroy(gameObject, .5f);
    }
}
