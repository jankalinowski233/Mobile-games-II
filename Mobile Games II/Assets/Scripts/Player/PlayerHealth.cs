using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //audio references
    AudioSource audioSrc;
    public AudioClip dmgClip;

    //health attributs
    public int health = 100;
    private int currentHealth;

    //death controller
    [HideInInspector]
    public bool isDead = false;

    //other components references
    Movement m;
    Attack a;
    Spell s;
    CapsuleCollider ccoll;
    Rigidbody rb;
    private Animator anim;

    //UI elements references
    public Slider healthSlider;
    public Image dmgImg;
    public Image healImg;

    //flashing damage image
    private bool damaged = false;
    private float fadeSpeed = 2f;
    private Color flash = new Color(1f, 0f, 0f, 0.1f);

    //flashing heal image
    private bool healed = false;
    private Color healFlash = new Color(0f, 1f, 0f, 0.1f);
    
    private void Awake()
    {
        //grab references
        anim = GetComponent<Animator>();
        m = GetComponent<Movement>();
        a = GetComponent<Attack>();
        s = GetComponent<Spell>();
        ccoll = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
    }

    void Start()
    {
        //set beginning attributes
        dmgImg.color = Color.clear;
        currentHealth = health;
        healthSlider.value = currentHealth;
    }

    private void Update()
    {
        //has the player been damaged?
        if (damaged == true) dmgImg.color = flash; //flash red image
        //fade image out
        else dmgImg.color = Color.Lerp(dmgImg.color, Color.clear, fadeSpeed * Time.deltaTime);

        damaged = false;

        //has the player been healed?
        if (healed == true) healImg.color = healFlash; //flash green image
        //fade image out
        else healImg.color = Color.Lerp(healImg.color, Color.clear, fadeSpeed * Time.deltaTime);

        healed = false;
    }

    public void GetDamage(int dmg)
    {
        //if player should die
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
            return;
        }

        //if player should just get damage
        if (currentHealth > 0)
        {
            currentHealth -= dmg;
            healthSlider.value = currentHealth;
            audioSrc.PlayOneShot(dmgClip);
        }

        damaged = true;
    }

    public void Heal(int hp)
    {
        //heal player
        currentHealth += hp;

        //constraint max health to 100
        if(currentHealth > health)
        {
            currentHealth = health;
        }

        healed = true;

        //update slider
        healthSlider.value = currentHealth;
    }

    //when player dies
    void Die()
    {
        //disable components
        ccoll.enabled = false;
        m.enabled = false;
        a.enabled = false;
        s.enabled = false;
        rb.isKinematic = true;

        //play death anim
        anim.SetTrigger("dead");
        
    }
}
