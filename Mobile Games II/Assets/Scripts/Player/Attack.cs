using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    //references to other components
    private Animator anim;
    private Movement m;

    //sword attack attributes
    [Header("Sword Attack")]
    [Space(7f)]
    public int swordDmg; //damage to apply
    //list of next targets
    public List<GameObject> enemiesList = new List<GameObject>();
    //current target
    [HideInInspector]
    public GameObject currentTarget;

    void Awake()
    {
        //grab references
        m = GetComponent<Movement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //is attack animation playing?
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("att"))
        {
            //if so, lock character in one place
            m.canWalk = false;
        }

        //else let player walk
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("att") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("spell"))
        {
            m.canWalk = true;
        }

        //is current target empty?
        if (currentTarget == null)
        {
            //if there are another enemies in the list
            if (!(enemiesList.Count == 0))
            {
                if (enemiesList.Count == 1)
                {
                    enemiesList.RemoveAt(0);
                }

                //if the current target was the only element in the list
                else if (enemiesList.Count > 0)
                {
                    enemiesList.RemoveAt(0);
                    currentTarget = enemiesList[0];
                }
            }
        }
    }

    public void PerformSwordAttack()
    {

        //if attack animation is not being performed (player's not attacking)
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            //if current target is not empty
            if (currentTarget != null)
            {
                //rotate towards target
                gameObject.transform.rotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
                //deal damage to target
                currentTarget.GetComponent<EnemyHealth>().ApplyDamage(swordDmg);
            }

            anim.SetTrigger("attack"); //play attack animation    
        }
       
    }
}
