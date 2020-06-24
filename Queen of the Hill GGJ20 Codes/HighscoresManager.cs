using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighscoresManager : MonoBehaviour
{
    public static List<HighScore> highscores = new List<HighScore>();

    public static void Save(HighScore newHighScore)
    {
        // Load old scores
        Load();
        // Add new score
        highscores.Add(newHighScore);
        // Sort list scores, highest ranked first
        highscores.Sort();
        highscores.Reverse();
        // Keep best 10 scores
        List<HighScore> bestTen = new List<HighScore>();
        int j = 0;
        for(int i=0; i < highscores.Count; i++)
        {
            bestTen.Add(highscores[i]);
            j++;
            if (j == 10) return;
        }
        highscores = bestTen;
        
        // Save scores
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highscores.gd");
        bf.Serialize(file, HighscoresManager.highscores);
        file.Close();
    }

    public static List<HighScore> Load()
    {
        if (File.Exists(Application.persistentDataPath + "/highscores.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highscores.gd", FileMode.Open);
            HighscoresManager.highscores = (List<HighScore>)bf.Deserialize(file);
            file.Close();

            return highscores;
        }

        return null;
    }
}

[System.Serializable]
public class HighScore : IComparable<HighScore>
{
    string name;
    int score;

    public string getName() { return name; }
    public int getScore() { return score; }

    public HighScore(string playerName, int playerScore)
    {
        name = playerName;
        score = playerScore;
    }

    public int CompareTo(HighScore item)
    {       // A null value means that this object is greater.
        if (item == null)
        {
            return 1;
        }
        else
        {
            return this.score.CompareTo(item.getScore());
        }
    }
}
