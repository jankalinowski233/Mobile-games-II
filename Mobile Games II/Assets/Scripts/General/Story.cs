using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Story controller
/// </summary>
public class Story : MonoBehaviour
{
    //critical story points
    public GameObject[] storyPoints;

    //player reference
    public GameObject player;

    //story attribute controllers
    public float distance = 0.5f;
    int controller = 0;
    //blockade
    public GameObject block;

    //other components references
    private GameManager gm;
    Movement m;

    //respawning enemies
    [Header("Respawning enemies")]
    public GameObject[] spawnPoints;
    public GameObject[] enemies;
    //random seed
    int random;

    //story player references
    public GameObject mainUI;
    public GameObject storyUI;
    public Text story;
    public Button cont;

    //story sentences
    [TextArea(3, 10)]
    public string[] sentences;
    public Animator panelAnim;

    private void Start()
    {
        //grab references
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        m = player.GetComponent<Movement>();
    }

    private void Update()
    {
        //is the player near a dialogue trigger point?
        if(Vector3.Distance(player.transform.position, storyPoints[0].transform.position) <= distance && controller == 0)
        {
            StartCoroutine(PlayStory());
            controller++;
        }

        //has players killed first wave of enemies?
        if (gm.enemies.Count == 0 && controller == 1)
        {
            //deactivate blockade
            block.SetActive(false);
            //clear enemies list
            if(gm.enemies.Count > 0)
            {
                gm.enemies.RemoveAt(0);
            }

            StartCoroutine(PlayStory());

            //spawn more enemies in town
            Spawn();
            gm.SetCounter();

            controller++;
        }

        //has player killed second wave of enemies?
        if (gm.enemies.Count == 0 && controller == 2)
        {
            StartCoroutine(PlayStory());
            mainUI.SetActive(false);
            //player won
            gm.Won();
            controller++;
        }
    }

    void Spawn()
    {
        //spawn enemies
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            //randomize index
            random = Random.Range(0, 2);
            switch(random)
            {
                //spawn enemies at correct index
                case 0:
                    Instantiate(enemies[random], spawnPoints[i].transform.position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(enemies[random], spawnPoints[i].transform.position, Quaternion.identity);
                    break;
            }
        }
    }

    //resume game
    public void Resume()
    {
        Time.timeScale = 1;
        panelAnim.Play("SlideOut");
        mainUI.SetActive(true);
        storyUI.SetActive(false);
        m.canWalk = true;
    }

    //play dialogue
    IEnumerator PlayStory()
    {
        //lock player
        m.canWalk = false;
        //reset text
        story.text = "";
        //change UI
        mainUI.SetActive(false);
        storyUI.SetActive(true);
        //slide dialogue panel in
        panelAnim.Play("SlideIn");

        //display correct sentence
        switch (controller)
        {
            case 0:  
                foreach(char letter in sentences[0].ToCharArray())
                {
                    story.text += letter;
                    yield return null;
                }
                break;

            case 1:
                foreach (char letter in sentences[1].ToCharArray())
                {
                    story.text += letter;
                    yield return null;
                }
                break;

            case 2:
                foreach (char letter in sentences[2].ToCharArray())
                {
                    story.text += letter;
                    yield return null;
                }
                break;
        }

        //freeze game
        Time.timeScale = 0;
    }
}
