using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //component's reference
    Renderer meshRend;

    //shader controller
    [HideInInspector]
    public bool isTarget = false;

    private void Awake()
    {
        //grab components
        meshRend = GetComponent<SkinnedMeshRenderer>();
    }


    private void Update()
    {
        //is an enemy player's target?
        if (isTarget == true)
        {
            meshRend.material.SetColor("_OutlineColor", Color.red);
        }

        else meshRend.material.SetColor("_OutlineColor", Color.black);

    }
}
