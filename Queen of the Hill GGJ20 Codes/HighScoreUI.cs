using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private Text txtName, txtScore;

    public void SetupUI(HighScore score)
    {
        txtName.text = score.getName();
        txtScore.text = score.getScore().ToString();
    }

    public void SetupWithoutHighScore(string text)
    {
        txtName.text = text;
    }
}
