using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Text title;
    private GameObject scores;
    private Scores lastScores;

	[SerializeField] private GameObject endButton;

    [SerializeField] private Button next;
    [SerializeField] private RectTransform winnerScore;
    [SerializeField] private RectTransform loserScore;

    [SerializeField] private MenuCamera cameraMenu;
    [SerializeField] private Transform end, main;

    [SerializeField] private GameObject mainScreen;
	[SerializeField] private Button playGame;

    // Start is called before the first frame update
    void Start()
    {
        next.onClick.AddListener(ToMainMenu);

        scores = GameObject.FindGameObjectWithTag("Score");
        if(scores == null)
        {
			StartCoroutine(SwitchScreen());
        }
        else
        {
            mainScreen.SetActive(false);

            lastScores = scores.GetComponent<Scores>();
            title.text = lastScores.ShowScores(winnerScore, loserScore);

            cameraMenu.ScreenSwitch(end);

            FadeInOut.screen.FadeIn();

			next.Select();
		}

        GameObject loader = GameObject.Find("PlayerLoader");
        if(loader != null)
        {
            foreach(Transform child in loader.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }

    private void ToMainMenu()
    {
        Destroy(scores);
        StartCoroutine(SwitchScreen());
    }

    private IEnumerator SwitchScreen()
    {
        FadeInOut.screen.FadeOut();

        yield return new WaitForSeconds(FadeInOut.screen.SetTime);

        cameraMenu.ScreenSwitch(main);
        mainScreen.SetActive(true);
        playGame.Select();
        endButton.SetActive(false);
        gameObject.SetActive(false);

        FadeInOut.screen.FadeIn();
    }
}
