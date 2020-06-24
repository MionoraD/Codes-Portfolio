using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestResultDisplay : MonoBehaviour
{
    Text text;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        gmc = FindObjectOfType<GameManager>();

        List<HighScore> listScore = HighscoresManager.Load();
        string txt = "";
        if(listScore != null)
        {
            HighScore best = listScore[0];
            txt = best.getName() + " got " + best.getScore();
        }
        else
        {
            txt = "You have not played the game yet!";
        }

        text.text = txt;
    }
}
