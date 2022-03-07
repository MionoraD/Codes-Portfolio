using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagingScenes : MonoBehaviour
{
    public static ManagingScenes scenes;

    public bool gameStart;
    [SerializeField] private bool client;

    [SerializeField] private GameObject prefabScores;

    [SerializeField] private GameObject prefabTestController;

    private void Awake()
    {
        if (client)
        {
            SceneManager.LoadScene("Scenes/Menu/PhoneMenu", LoadSceneMode.Additive);
        }
        else if (!gameStart)
        {
            scenes = this;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        if (gameStart)
        {
            StartTestLevel(1);
        }
    }

    IEnumerator Unload(int scene)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(scene);
    }

    IEnumerator WaitForLoad(int currentScene, int newScene)
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
        StartCoroutine(Unload(currentScene));
    }

    public void StartLevel(int currentScene, int newScene)
    {
        StartCoroutine(WaitForLoad(currentScene, newScene));
    }
    public void StartLevelFromMenu(int newScene)
    {
        StartCoroutine(WaitForLoad(1, newScene));
    }
    public void OpenMenu(int currentScene)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        StartCoroutine(Unload(currentScene));
    }

    public void FindScores(Transform parentCharacters)
    {
        GameObject scores = Instantiate(prefabScores, this.transform);
        Scores calculate = scores.GetComponent<Scores>();
        if (calculate != null) calculate.SetScores(parentCharacters);
    }

    public void Endlevel(int currentScene)
    {
        OpenMenu(currentScene);
    }

    public void StartTestLevel(int level)
    {
        GameObject playerLoad = GameObject.Find("PlayerLoader");

        if (playerLoad != null)
        {
            GameObject firstController = Instantiate(prefabTestController, playerLoad.transform);
            StandardController stController = firstController.GetComponent<StandardController>();
            stController.SetupButtons(true, 0);

            GameObject secondController = Instantiate(prefabTestController, playerLoad.transform);
            StandardController stController2 = secondController.GetComponent<StandardController>();
            stController2.SetupButtons(false, 1);
        }

        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
