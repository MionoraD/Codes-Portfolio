using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.Video;

// This class stores the different scenes that can be used

public class Scenario : MonoBehaviour
{
    // Name of hte scene
    public string Name
    {
        get { return _name; }
    }
    [SerializeField] private string _name;

    // Small description of the scene
    public string Description
    {
        get { return _description; }
    }
    [SerializeField] private string _description;

    // The image of the scene
    public Image Img
    {
        get { return _img; }
    }
    [SerializeField] private Image _img;

    // The videos that belong to this scene (different videos for different weathers)
    public List<WeatherClip> WeahterClips
    {
        get { return listVideoClips; }
    }
    [SerializeField] List<WeatherClip> listVideoClips = new List<WeatherClip>();

    // The standard video of the scene
    public VideoClip Clip
    {
        get { return _clip; }
    }
    [SerializeField] private VideoClip _clip;

    // The list of hitboxes that belong to this scene
    public Hitbox[] listHitbox
    {
        get { return hitboxList; }
    }
    private Hitbox[] hitboxList;

    // The time that this scene will take
    private float endtime = 0;
    public float Time
    {
        get { return endtime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find all the hitboxes that belong to this scene
        hitboxList = GetComponentsInChildren<Hitbox>();

        // If the scene has a video, set the time to video length
        if (_clip != null) endtime = (float)_clip.length;
    }

    // Called when opening the scene to set the modes
    public void StartScenario(ModeStop modeStop, ModeFeedback modeFeedback)
    {
        // Tell each hitbox which modes the scene will be using
        foreach (Hitbox item in hitboxList)
        {
            item.StartHitbox(modeStop, modeFeedback);
        }

        // Reset the scores
        Scores.Reset();
    }

    // Change the weather of the current video
    public bool SetWeatherVideo(WeatherClip clip)
    {
        // Find video
        _clip = clip.Clip;
        
        // If video found, set new video time
        if (_clip != null)
        {
            endtime = (float)_clip.length;
            return true;
        }

        return false;
    }
}

// The class to store a weather video
[System.Serializable]
public class WeatherClip
{
    // The name of the weather
    [SerializeField] private string _weather;

    // The icon of the weather to show in the menu
    public Sprite Icon
    {
        get { return image; }
    }
    [SerializeField] private Sprite image;

    // The video that belongs to this weather
    public VideoClip Clip
    {
        get { return _clip; }
    }
    [SerializeField] private VideoClip _clip;

    // Check if the given name is this weather
    public bool IsWeather(string weather)
    {
        return (_weather.Equals(weather));
    }
}

// To store the scores of the user
public static class Scores
{
    // Collect all scores (for checking if all the scores are collected, or if an error occured)
    public static int Total
    {
        get { return total; }
    }
    private static int total = 0;

    // Storing the number of times the user was correct
    public static int Correct
    {
        get { return correct; }
    }
    private static int correct = 0;

    // Storing the number of times the user was wrong
    public static int Wrong
    {
        get { return wrong; }
    }
    private static int wrong = 0;

    // Reseting all the scores
    public static void Reset()
    {
        total = 0;
        correct = 0;
        wrong = 0;
    }

    // Add a correct score
    public static void AddCorrect()
    {
        correct++;
        total++;

        UI_InGame.instance.UpdateUI();
    }

    // Add a wrong score
    public static void AddWrong()
    {
        wrong++;
        total++;

        UI_InGame.instance.UpdateUI();
    }
}
