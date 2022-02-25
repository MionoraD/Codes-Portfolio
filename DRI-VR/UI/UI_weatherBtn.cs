using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

// UI manager to handle a weather button in the select mode menu (UI_SelectMode)

public class UI_weatherBtn : MonoBehaviour
{
    // Store the video
    private WeatherClip thisClip;

    // Icon to represent the weather
    [SerializeField] private SVGImage image;

    // The UI manager that controls the select mode menu
    private UI_SelectMode ui_select;

    // Called when button created to set the variables
    public void SetWeatherClip(UI_SelectMode select,  WeatherClip clip)
    {
        thisClip = clip;
        ui_select = select;
        image.sprite = clip.Icon;
    }

    // Called when button pressed to tell the select mode menu that this weather has been chosen
    public void SetThisWeather()
    {
        if (ui_select != null) ui_select.SetWeather(image, thisClip);
    }
}
