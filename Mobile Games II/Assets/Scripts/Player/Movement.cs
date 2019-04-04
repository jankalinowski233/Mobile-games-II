using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //audio references
    AudioSource audioSource;
    public AudioClip step;
    bool playingStep = false;

    public float speed; //speed of character
    [HideInInspector]
    public bool canWalk = true; //can the character move?
    private Vector3 currentPos; //vector used to lock player at certain position

    private Rigidbody rb; //reference to rigidbody
    private Animator anim; //reference to animator

    public Joystick joystick; //reference to joystick script

    private void Awake()
    {
        //grab references from Player GameObject
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    //all physics shall happen in FixedUpdate
    void FixedUpdate()
    {
        //move and rotate character
        MoveAndRotate();
        //update current position of player
        currentPos = transform.position;
    }

    //rotate player
    void MoveAndRotate()
    {
        //is player allowed to walk?
        if (canWalk)
        {
            //has joystick been moved?
            Vector3 vector = Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical;

            //if joystick's been moved
            if (vector != Vector3.zero)
            {
                //move the character in the world space
                transform.Translate(vector * speed * Time.deltaTime, Space.World);
                //rotate the character
                transform.rotation = Quaternion.LookRotation(vector);
                //play walking animation
                anim.SetBool("isWalking", true);

                if(playingStep == false)
                {
                    StartCoroutine(PlayStep());
                }
             
            }

            //if joystick's been released
            else
            {
                //stop playing animation
                anim.SetBool("isWalking", false);
            }
        }

        //if player's not allow to walk (e.g. he's attacking or casting a spell)
        else
        {
            transform.position = currentPos;
        }
        
    }

    //play step sound
    IEnumerator PlayStep()
    {
        playingStep = true;
        audioSource.PlayOneShot(step);
        yield return new WaitForSeconds(0.3f);
        playingStep = false;
    }
}
