using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    //audio references
    AudioSource audioSrc;
    public AudioClip fireballCast;

    //text
    public Text notEnoughMana;

    //spell attributes
    [Range(0, 10)]
    public float radius;
    public LayerMask objects;
    [HideInInspector]
    public int spellDmg;

    //mana
    public float mana;
    public Slider manaSlider;

    //spell spawn point
    public Transform aoe;
    public Transform fireballSpawner;

    //fireball spawning
    public float amountOfFireballSpawns;
    private float angle = 0f;
    public GameObject fireball;

    //visual effects
    [Header("VFX")]
    [Space(7f)]
    public GameObject lightningVFX;
    public GameObject frostVFX;

    //other components references
    Animator anim;
    Movement m;

    //mana regain
    public float manaTimer;
    private float startTimer;

    private void Awake()
    {
        //grab references
        anim = GetComponent<Animator>();
        m = GetComponent<Movement>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //set initial values
        notEnoughMana.text = "";
        UpdateSlider();
    }

    private void Update()
    {
        //regain mana
        if(startTimer <= 0)
        {
            mana += 2;
            if (mana > 100) mana = 100;
            UpdateSlider();
            startTimer = manaTimer;
        }

        else
        {
            startTimer -= Time.deltaTime;
        }

        //lock player if casting a spell
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("spell"))
        {
            m.canWalk = false;
        }

        //else let player walk
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("spell") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("att")) m.canWalk = true;
    }

    public void CastSpell(int spellType)
    {
        //cast correct spell
        switch(spellType)
        {
            case 1:
                Fireball();
                break;
            case 2:
                Frost();
                break;
            case 3:
                Lightning();
                break;
        }
    }

    //cast fireballs
    void Fireball()
    {
        //set damage
        spellDmg = 8;

        //if not enough mana
        if (mana < 20)
        {
            StartCoroutine(ChangeText());
            return;
        }

        //else cast
        else
        {
            mana -= 20;
            anim.SetTrigger("fireball");
            StartCoroutine(SpawnFireball());

        }
        
    }

    //cast ice spikes
    void Frost()
    {
        //set damage
        spellDmg = 12;

        //if not enough mana
        if (mana < 25)
        {
            StartCoroutine(ChangeText());
            return;
        }

        //else cast
        else
        {
            mana -= 25;
            anim.SetTrigger("frost");
            StartCoroutine(SpawnFrost());
        }
    }
    
    //cast lightnings
    void Lightning()
    {
        //set damage
        spellDmg = 18;

        //if not enough mana
        if (mana < 35)
        {
            StartCoroutine(ChangeText());
            return;
        }

        //else cast
        else
        {
            mana -= 35;
            anim.SetTrigger("lightning");
            StartCoroutine(SpawnLightning());   
        }
    }

    //update slider value
    void UpdateSlider()
    {
        manaSlider.value = mana;
    }

    private void OnDrawGizmos()
    {
        //draw gizmos to debug code
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(aoe.position, radius);
    }

    //spawns fireballs
    IEnumerator SpawnFireball()
    {
        //play sound
        audioSrc.PlayOneShot(fireballCast);
        //short delay to match casting with animation
        yield return new WaitForSeconds(1.6f);

        //set initial angle
        angle = 0f;
        //update slider
        UpdateSlider();

        //cast fireballs around player
        for (int i = 0; i < amountOfFireballSpawns; i++)
        {
            print(angle);
            Instantiate(fireball, fireballSpawner.position, fireballSpawner.transform.rotation);
            fireballSpawner.transform.Rotate(new Vector3(0f, angle, 0f));
            angle = angle + 45;
        }
    }

    //cast ice spikes
    IEnumerator SpawnFrost()
    {
        //short delay to match casting with animation
        yield return new WaitForSeconds(0.5f);
        //rescale VFX
        frostVFX.transform.localScale = new Vector3(2f, 1f, 2f);
        //instantiate VFX
        GameObject temp = Instantiate(frostVFX, transform.position, Quaternion.identity);
        
        //update slider
        UpdateSlider();

        //deal damage to all colliders in range of spikes
        Collider[] damagedObjects = Physics.OverlapSphere(aoe.position, radius, objects);
        foreach (Collider col in damagedObjects)
        {
            if (col.GetType() == typeof(CapsuleCollider) && col.CompareTag("Enemy"))
            {
                col.GetComponent<EnemyHealth>().ApplyDamage(spellDmg);
            }
        }

        //destroy spell after 4 seconds
        yield return new WaitForSeconds(4f);
        Destroy(temp);
    }

    //spawn lightnings
    IEnumerator SpawnLightning()
    {
        //short delay to match casting with animation
        yield return new WaitForSeconds(1.6f);
        //rescale VFX
        lightningVFX.transform.localScale = new Vector3(3f, 1f, 3f);
        //instantiate VFX
        GameObject temp = Instantiate(lightningVFX, transform.position, Quaternion.identity);
        
        //update slider
        UpdateSlider();

        //deal damage to all colliders in range
        Collider[] damagedObjects = Physics.OverlapSphere(aoe.position, radius, objects);
        foreach (Collider col in damagedObjects)
        {
            if (col.GetType() == typeof(CapsuleCollider) && col.CompareTag("Enemy"))
            {
                col.GetComponent<EnemyHealth>().ApplyDamage(spellDmg);
            }
        }

        //destroy after short delay
        yield return new WaitForSeconds(4f);
        Destroy(temp);
    }

    //change text if players do not have enough mana
    IEnumerator ChangeText()
    {
        notEnoughMana.text = "Not enough mana!";
        yield return new WaitForSeconds(4.0f);
        notEnoughMana.text = "";
    }
}
