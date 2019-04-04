using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    //audio references
    AudioSource audioSrc;
    public AudioClip blastSound;

    //fireball attributes
    [Range(0, 1000)]
    public float speed;
    private int dmg;

    //collision attributes
    [Range(0, 10)]
    public float radius;
    public LayerMask enemies;

    //other components references
    Rigidbody rb;
    Spell sp;

    //visual effects
    public ParticleSystem vfx;
    public GameObject blastVFX;

    private void Awake()
    {
        //grab references
        rb = GetComponent<Rigidbody>();
        sp = GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //set damage
        dmg = sp.spellDmg;
        //start destroy counter
        StartCoroutine(DestroyFireball());
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    void MoveForward()
    {
        //move
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //has the fireball collided with an enemy?
        if(other.CompareTag("Enemy") && other.GetType() == typeof(CapsuleCollider))
        {
            //find all colliders in the sphere
            Collider[] objectsToDamage = Physics.OverlapSphere(gameObject.transform.position, radius, enemies);

            //apply damage to all found gameobjects
            foreach(Collider col in objectsToDamage)
            {
                //constraint getting, so it's not detected by presence of multiple colliders
                if (col.GetType() == typeof(CapsuleCollider) && col.CompareTag("Enemy"))
                {
                    col.GetComponent<EnemyHealth>().ApplyDamage(dmg);
                }
            }

            //destroy fireball
            StartCoroutine(DestroyOnTrigger());
        }

        //has the ball collided with a house?
       else if(other.CompareTag("House"))
       {
            //destroy fireball
            StartCoroutine(DestroyOnTrigger());
       }
    }

    IEnumerator DestroyFireball()
    {
        //start timer
        yield return new WaitForSeconds(4f);
        //play blast VFX
        GameObject temp = Instantiate(blastVFX, transform.position, Quaternion.identity);
        //stop flying sound
        audioSrc.Stop();
        //play destroy sound
        audioSrc.PlayOneShot(blastSound);
        //stop playing particle system
        vfx.Stop();
        yield return new WaitForSeconds(2f);
        //destroy
        Destroy(temp);
        Destroy(gameObject);
        
    }

    IEnumerator DestroyOnTrigger()
    {
        //destroy on entering other collider
        audioSrc.Stop();
        audioSrc.PlayOneShot(blastSound);
        vfx.Stop();
        GameObject temp = Instantiate(blastVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Destroy(temp);
        Destroy(gameObject);
    }
}
