using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayer : MonoBehaviour
{
    //other components
    NavMeshAgent agent;
    Animator anim;
    EnemyAttack att;

    //detecion radius
    public float radius;

    //player reference
    GameObject player;

    //movement controllers
    [HideInInspector]
    public bool canAttack = false;
    private bool canWalk = true;

    //current position
    Vector3 currentPos;

    private void Awake()
    {
        //grab references
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        att = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        //find player
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        //has the player entered detection radius and is not close enough to attack?
        if (Vector3.Distance(transform.position, player.transform.position) <= radius && !canAttack)
        {
            agent.SetDestination(player.transform.position);
        }

        //if player isn't close enough to attack
        if (agent.remainingDistance > agent.stoppingDistance && !canAttack)
        {
            anim.SetBool("isWalking", true);
            agent.isStopped = false;
        }

        //if player is close enough to an enemy
        else if (agent.remainingDistance <= agent.stoppingDistance && !canAttack)
        {
            anim.SetBool("isWalking", false);
            agent.isStopped = true;
        }

        //if player can be attacked
        else if(agent.remainingDistance <= agent.stoppingDistance && canAttack)
        {
            anim.SetBool("isWalking", false);
            agent.isStopped = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //has the player entered attack radius?
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
            canAttack = true;
            att.remainingTimeBtwAttacks = 1.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //has the player exited attack radius?
        if (other.CompareTag("Player") && other.GetType() == typeof(CapsuleCollider))
        {
            canAttack = false;
        }
    }

    private void OnDrawGizmos()
    {
        //gizmos to debug code
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
    }



}
