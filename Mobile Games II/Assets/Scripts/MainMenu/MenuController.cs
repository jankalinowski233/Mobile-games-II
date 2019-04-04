using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //references
    public Text tapToStart;

    private void Start()
    {
        //starts blinking text
        StartCoroutine(BlinkingText());
    }

    //blinks text
    IEnumerator BlinkingText()
    {
        tapToStart.text = "Tap to start";
        yield return new WaitForSeconds(1f);
        tapToStart.text = "";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(BlinkingText());
    }
}
