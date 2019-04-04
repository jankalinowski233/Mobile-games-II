using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //makes player a singleton
    public static PlayerManager instance;
    public GameObject player;

    private void Awake()
    {
        instance = this;
    }
}
