using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    private static string sceneToLoadName = "MainMenu";
    private AsyncOperation sceneToLoad;

    public static void LoadScene(string sceneName)
    {
        sceneToLoadName = sceneName;
        SceneManager.LoadScene("SceneLoading");
    }

    private void Start()
    {
        sceneToLoad = SceneManager.LoadSceneAsync(sceneToLoadName);
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        while (!sceneToLoad.isDone)
        {
            progressBar.fillAmount = sceneToLoad.progress;
            yield return null;
        }
    }
}