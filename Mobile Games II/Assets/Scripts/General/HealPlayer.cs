using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    //audio reference
    public AudioClip healSound;

    //player reference
    GameObject player;

    //heal attributes
    public int heal;

    private void Start()
    {
        //find player
        player = PlayerManager.instance.player;
    }

    private void OnTriggerEnter(Collider other)
    {
        //has player entered potion collider?
        if(other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
            //heal player
            player.GetComponent<AudioSource>().PlayOneShot(healSound);
            player.GetComponent<PlayerHealth>().Heal(heal);
            Destroy(gameObject);
        }
    }
}
