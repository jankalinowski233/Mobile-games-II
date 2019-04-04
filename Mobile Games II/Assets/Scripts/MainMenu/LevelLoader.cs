using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    //GameObjects references
    public GameObject loadingScr;
    public Slider progressBar;
    public Text progressTxt;

    //deactivate loading screen at start
    private void Start()
    {
        loadingScr.SetActive(false);
    }

    //loads level with correct name
    public void LoadLevel(string name)
    {
        StartCoroutine(LoadAsync(name));
    }

    IEnumerator LoadAsync(string name)
    {
        //load scene asynchronously
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        //show loading screen panel
        loadingScr.SetActive(true);

        //show loading progress on progress bar
        while(!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            progressBar.value = progress;
            progressTxt.text = progress * 100f + "%";

            yield return null;
        }
    }
}
