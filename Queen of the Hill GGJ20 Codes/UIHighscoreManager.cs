using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighscoreManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabHighscoreUI;

    // Start is called before the first frame update
    void Start()
    {
        List<HighScore> highscores = HighscoresManager.Load();
        if(highscores != null)
        {
            foreach(HighScore score in highscores)
            {
                GameObject ui = Instantiate(prefabHighscoreUI, gameObject.transform);
                HighScoreUI uiCode = ui.GetComponent<HighScoreUI>();
                if (uiCode != null) uiCode.SetupUI(score);
            }
        }
        else
        {
            GameObject ui = Instantiate(prefabHighscoreUI, gameObject.transform);
            HighScoreUI uiCode = ui.GetComponent<HighScoreUI>();
            if (uiCode != null) uiCode.SetupWithoutHighScore("No Highscores");
        }
    }
}
