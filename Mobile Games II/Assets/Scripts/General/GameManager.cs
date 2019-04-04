using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //audio references
    AudioSource audioSource;
    public AudioClip victory;
    public AudioClip defeat;
    bool isDeadPlaying = false;

    //main UI reference
    public GameObject mainUI;

    //UI elements
    public Text enemiesCounterTxt;
    public Image victoryImg;
    public Image youDiedImg;

    //number of enemies
    GameObject[] tempEnemies;
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public int enemiesAmount;

    //player references
    GameObject player;
    PlayerHealth ph;

    private void Awake()
    {
        //grab references
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //set enemies amount
        SetCounter();
        //find player
        player = PlayerManager.instance.player;
        //get player's health
        ph = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //is player dead?
        if (ph.isDead == true && isDeadPlaying == false)
        {
            StartCoroutine(ShowDiedText());
            isDeadPlaying = true;
        }
            
    }

    public void SetCounter()
    {
        //find all enemies
        tempEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        //add all enemies to the array
        for(int i = 0; i < tempEnemies.Length; i++)
        {
            enemies.Add(tempEnemies[i]);
        }
        
        //set the counter
        enemiesAmount = enemies.Count;
        //set string
        enemiesCounterTxt.text = enemiesAmount.ToString();
    }

    public void UpdateCounter(int amount)
    {
        //update enemies counter
        enemiesAmount += amount;
        enemiesCounterTxt.text = enemiesAmount.ToString();
    }

    //when player dies
    IEnumerator ShowDiedText()
    {
        //stop playing default music
        audioSource.Stop();
        yield return new WaitForSeconds(4f);
        //set image to be active
        youDiedImg.gameObject.SetActive(true);
        //deactivate main UI
        mainUI.SetActive(false);
        //play new theme
        audioSource.clip = defeat;
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        //exit game scene
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowVictoryText()
    {
        //stop default theme
        audioSource.Stop();
        yield return new WaitForSeconds(4f);
        //set new theme
        audioSource.PlayOneShot(victory);
        //show victory image
        victoryImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    //when player finishes level
    public void Won()
    {
        StartCoroutine(ShowVictoryText());
    }

}
