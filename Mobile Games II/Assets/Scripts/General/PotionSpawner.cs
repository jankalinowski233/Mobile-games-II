using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    //gameobject
    public GameObject potion;
    //spawn locations
    GameObject[] spawnLoc;
    private void Start()
    {
        //find all spawn locations
        spawnLoc = GameObject.FindGameObjectsWithTag("Potion spawner");
        SpawnPotions();
    }

    public void SpawnPotions()
    {
        //as many potions as many locations
        for (int i = 0; i < spawnLoc.Length; i++)
        {
            Instantiate(potion, spawnLoc[i].transform.position, Quaternion.identity);
        }
    }
}
