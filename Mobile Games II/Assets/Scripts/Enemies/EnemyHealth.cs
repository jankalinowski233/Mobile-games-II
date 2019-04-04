using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    //audio reference
    AudioSource audioSource;
    public AudioClip dmgSound;
    public AudioClip deathSound;

    //health attributes
    public int health = 30;
    private int currHealth;
    private bool isDead = false;

    //other components references
    Animator anim;
    CapsuleCollider ccoll;
    SphereCollider scoll;
    Rigidbody rb;
    NavMeshAgent navMesh;

    MoveTowardsPlayer m;
    EnemyAttack ea;
    EnemyController ec;

    //particle system
    public ParticleSystem bloodVFX;
    
    //Other gameobjects reference
    GameObject player;
    Attack playerAttack;
    GameObject gm;
    GameManager manager;

    //death attributes
    [Header("Death")]
    [Space(7f)]
    public float sinkSpeed = 2f;
    private bool shouldDisappear = false;

    private void Awake()
    {
        //grab components
        anim = GetComponent<Animator>();
        ccoll = GetComponent<CapsuleCollider>();
        scoll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        m = GetComponent<MoveTowardsPlayer>();
        ea = GetComponent<EnemyAttack>();
        ec = GetComponentInChildren<EnemyController>();
    }

    private void Start()
    {
        //finde player
        player = PlayerManager.instance.player;
        playerAttack = player.GetComponent<Attack>();

        //find game manager
        gm = GameObject.Find("GameManager");
        manager = gm.GetComponent<GameManager>();

        //set health
        currHealth = health;
        //stop playing particle system
        bloodVFX.Stop();
    }

    private void Update()
    {
        //is enemy dead
        if(shouldDisappear == true)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }

        //is the enemy current player's target? 
        if (playerAttack.currentTarget == this.gameObject)
        {
            ec.isTarget = true;
        }
        else ec.isTarget = false;
    }

    public void ApplyDamage(int amount)
    {
        //decrease health
        currHealth -= amount;

        //is an enemy attacking?
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //if enemy's health has been decreased below 0 and enemy is not dead already
            if (!isDead && currHealth <= 0)
            {
                //die
                StartCoroutine("ShouldDie");
                return;
            }
            //play damage sound
            audioSource.PlayOneShot(dmgSound);
            //show blood
            StartCoroutine(PlayVFX());
        }

        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //interrupt attack, die
            if (!isDead && currHealth <= 0)
            {
                StartCoroutine("ShouldDie");
                return;
            }
            audioSource.PlayOneShot(dmgSound);
            StartCoroutine(PlayVFX());
        }
    }

    //play particle system
    IEnumerator PlayVFX()
    {
        bloodVFX.Play();
        yield return new WaitForSeconds(0.5f);
        bloodVFX.Stop();
    }

    IEnumerator ShouldDie()
    {
        //if an enemy should die and is current players target, remove it from player's current target
        if(this.gameObject == playerAttack.currentTarget)
        {
            playerAttack.currentTarget = null;
        }
        
        //decrease number of enemies
        manager.UpdateCounter(-1);

        //play death sound
        audioSource.PlayOneShot(deathSound);

        //disable components
        isDead = true;
        ccoll.enabled = false;
        scoll.enabled = false;
        rb.isKinematic = true;
        navMesh.enabled = false;

        m.enabled = false;
        ea.enabled = false;

        ec.isTarget = false;

        //play death animation
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(5f);
        
        //change boolean to true, so the enemy will sink into the ground
        shouldDisappear = true;

        //destroy gameObject
        yield return new WaitForSeconds(4f);
        manager.enemies.Remove(this.gameObject);
        Destroy(gameObject);
        

    }
}
