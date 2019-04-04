using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //attack script reference
    Attack att;

    private void Start()
    {
        //find script
        att = GetComponentInParent<Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if an enemy has entered detection area
        if (other.CompareTag("Enemy") && other.GetType() == typeof(CapsuleCollider))
        {
            //add an enemy to the list
            att.enemiesList.Add(other.gameObject);
            //if current target was empty - set the first enemy from the list to be current target
            if (att.currentTarget == null) att.currentTarget = att.enemiesList[0];
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if an enemy has exited detecion area
        if (other.CompareTag("Enemy") && other.GetType() == typeof(CapsuleCollider))
        {
            //remove an enemy from the list
            att.enemiesList.Remove(other.gameObject);
            //remove current target
            if (att.currentTarget != null) att.currentTarget = null;
        }
    }
}
