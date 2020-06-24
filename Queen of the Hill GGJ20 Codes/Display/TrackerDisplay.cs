using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerDisplay : MonoBehaviour
{
    [SerializeField] private Text txtenemy, txtcoins, txtparts, txtrepwall, txtbwall, txtrepgun, txtbgun, txtRounds, txtScore;

    void Start()
    {
        Tracker._instance.StoreScore();

        txtenemy.text = Tracker._instance.ScoreEnemy().ToString();
        txtcoins.text = Tracker._instance.CoinsCollected().ToString();
        txtparts.text = Tracker._instance.PartsReturned().ToString();
        txtrepwall.text = Tracker._instance.WallsRepaired().ToString();
        txtbwall.text = Tracker._instance.WallsBuild().ToString();
        txtrepgun.text = Tracker._instance.GunsRepaired().ToString();
        txtbgun.text = Tracker._instance.GunsBuild().ToString();
        txtRounds.text = Tracker._instance.RoundsSurvived().ToString();
        txtScore.text = Tracker._instance.ReturnScore().ToString();
    }
}
