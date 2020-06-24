using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tracker : MonoBehaviour
{
    public static Tracker _instance;
    private int enemiescaught, coinscollected, partsreturned, wallsrepaired, wallsbuild, gunsrepaired, gunsbuild, rounds;

    private Scene endgame;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        endgame = SceneManager.GetSceneByBuildIndex(2);
    }

    // Reset scores
    public void StartGame()
    {
        enemiescaught = 0;
        coinscollected = 0;
        partsreturned = 0;
        wallsrepaired = 0;
        wallsbuild = 0;
        gunsrepaired = 0;
        gunsbuild = 0;
        rounds = 0;
    }

    // Setters
    public void CatchEnemy() { enemiescaught++; }
    public void CollectCoin() { coinscollected++; }
    public void ReturnPart() { partsreturned++; }
    public void RepairWall() { wallsrepaired++; }
    public void BuildWall() { wallsbuild++; }
    public void RepairGun() { gunsrepaired++; }
    public void BuildGun() { gunsbuild++; }
    public void SurvivedRound() { rounds++; }

    // Getters
    public int ScoreEnemy() { return enemiescaught; }
    public int CoinsCollected() { return coinscollected; }
    public int PartsReturned() { return partsreturned; }
    public int WallsRepaired() { return wallsrepaired; }
    public int WallsBuild() { return wallsbuild; }
    public int GunsRepaired() { return gunsrepaired; }
    public int GunsBuild() { return gunsbuild; }
    public int RoundsSurvived() { return rounds - 1; }
    public int ReturnScore() { return CalculateScore(); }

    public int CalculateScore()
    {
        int endscore = 0;

        endscore += ScoreEnemy() * 10;
        endscore += PartsReturned();
        endscore += WallsRepaired();
        endscore += WallsBuild() * 3;
        endscore += RoundsSurvived();

        return endscore;
    }

    public void StoreScore()
    {
        GameManager gmc = FindObjectOfType<GameManager>();
        string name = gmc.getPlayerName();
        int score = CalculateScore();

        HighScore highscore = new HighScore(name, score);
        HighscoresManager.Save(highscore);
        Debug.Log(name + " got score: " + score);
    }
}
