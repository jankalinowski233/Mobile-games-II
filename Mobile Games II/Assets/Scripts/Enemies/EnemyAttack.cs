using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Components references
    MoveTowardsPlayer mtp;
    Animator anim;

    //player reference
    GameObject player;

    //attack attributes
    [Header("Attack")]
    [Space(7)]
    public int damage;
    public float timeBtwAttacks = 2f;
    [HideInInspector]
    public float remainingTimeBtwAttacks;
    //delay to match dealing damage with animation
    public float delay = 1.05f;

    private void Awake()
    {
        //grab references
        anim = GetComponent<Animator>();
        mtp = GetComponent<MoveTowardsPlayer>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        remainingTimeBtwAttacks = timeBtwAttacks;
    }

    // Update is called once per frame
    void Update()
    {
        //can the enemy attack?
        if (mtp.canAttack == true)
        {
            //rotate towards player
            gameObject.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

            //has enough time between attacks passed?
            if (remainingTimeBtwAttacks <= 0)
            {
                //attack
                anim.SetBool("attack", true);
                StartCoroutine(DoDamage(delay));
                remainingTimeBtwAttacks = timeBtwAttacks;
            }

            else
            {
                anim.SetBool("attack", false);
                remainingTimeBtwAttacks -= Time.deltaTime;
            }
        }

        //enemy cannot attack
        else
        {
            anim.SetBool("attack", false);
        }

    }

    //deals damage
    IEnumerator DoDamage(float attackDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        player.GetComponent<PlayerHealth>().GetDamage(damage);
    }

}
