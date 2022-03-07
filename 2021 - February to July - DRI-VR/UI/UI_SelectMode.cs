using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;

// UI manager of the selecting mode screen 

public class UI_SelectMode : MonoBehaviour
{
    // Storing the scenario that this belongs to
    private Scenario thisScenario;

    // Storing the stop mode
    private ModeStop Stopping
    {
        set 
        { 
            mStopping = value;
            UpdateStartButton();
        }
    }
    private ModeStop mStopping = null;

    // Storing the feedback mode
    private ModeFeedback GivingFeedback
    {
        set
        {
            mFeedback = value;
            UpdateStartButton();
        }
    }
    private ModeFeedback mFeedback = null;

    // Storing if weather video has been found
    private bool hasWeather = false;

    // The global ui manager
    [SerializeField] private UI_Control ui_control;

    [Header("Stop mode")] 
    [SerializeField] private Transform parent_stopmodetoggles; // Parent transform of the stop mode buttons
    // Icon images of the stop mode buttons
    [SerializeField] private SVGImage bg_btn_stopSystem;
    [SerializeField] private SVGImage bg_btn_stopPlayer;
    [SerializeField] private SVGImage bg_btn_stopNone;

    [Header("Feedback mode")]
    [SerializeField] private Transform parent_feedbacktoggles; // Parent transform of feedback buttons
    // Icon images of the feedback buttons
    [SerializeField] private SVGImage bg_btn_feedbackduring;
    [SerializeField] private SVGImage bg_btn_feedbackafter;

    [Header("Weather mode")]
    [SerializeField] private GameObject prefab_btn_weatherVideo; // Prefab weather video button
    [SerializeField] private Transform parent_weathertoggles; // Parent transform of weather buttons

    [Header("Start btn")]
    [SerializeField] private Button btn_startScenario; // Start the scenario button

    [Header("Btn colors")]
    [SerializeField] private Color normal_color; // Standard color, for when the button is not selected
    [SerializeField] private Color selected_color; // Selected color, for when the button is selected

    // Called by UI_Control to load in all the options for the scenario
    public void SetScenario(Scenario scenario)
    {
        // Store this scenario
        thisScenario = scenario;

        // Reset modes
        Stopping = null;
        GivingFeedback = null;

        // Clear weather buttons
        foreach(Transform child in parent_weathertoggles)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Create weather buttons
        foreach(WeatherClip clip in thisScenario.WeahterClips)
        {
            GameObject btn = Instantiate(prefab_btn_weatherVideo, parent_weathertoggles);
            UI_weatherBtn toggle = btn.GetComponent<UI_weatherBtn>();
            toggle.SetWeatherClip(this, clip);
        }

        // Setup button functions
        btn_startScenario.onClick.AddListener(() => {
            ui_control.SelectMode(mStopping, mFeedback);
        });
        btn_startScenario.gameObject.SetActive(false);
    }

    // Called when a button is selected to change all buttons to standard color
    private void SetSelectedButton(Transform parent, SVGImage selectedBtn)
    {
        SVGImage[] backgrounds = parent.GetComponentsInChildren<SVGImage>();

        foreach (SVGImage item in backgrounds)
        {
            item.color = normal_color;
        }

        selectedBtn.color = selected_color;
    }

    // Functions to set the stop mode, called by stop mode buttons
    public void SetModeStopSystem()
    {
        Stopping = new ModeSystemstop();
        SetSelectedButton(parent_stopmodetoggles, bg_btn_stopSystem);
    }

    public void SetModeStopPlayer()
    {
        Stopping = new ModePlayerStop();
        SetSelectedButton(parent_stopmodetoggles, bg_btn_stopPlayer);
    }

    public void SetModeNonStop()
    {
        Stopping = new ModeNonStop();
        SetSelectedButton(parent_stopmodetoggles, bg_btn_stopNone);
    }

    // Functions to set the feedback mode, called by feedback mode buttons
    public void SetModeFeedbackDuring()
    {
        GivingFeedback = new ModeFeedbackDuring();
        SetSelectedButton(parent_stopmodetoggles, bg_btn_feedbackduring);
    }

    public void SetModeFeedbackAfter()
    {
        GivingFeedback = new ModeFeedbackAfter();
        SetSelectedButton(parent_stopmodetoggles, bg_btn_feedbackafter);
    }

    // Function to set the weather mode
    public void SetWeather(SVGImage bg_btn, WeatherClip clip)
    {
        hasWeather = thisScenario.SetWeatherVideo(clip); // Returns true if video found
        UpdateStartButton();

        SetSelectedButton(parent_weathertoggles, bg_btn);
    }

    // Check if scenario can start
    public void UpdateStartButton()
    {
        // Enable start button if all modes (stop, feedback & weather) have been chosen, else disable start button
        if(mStopping != null && mFeedback != null && hasWeather)
        {
            btn_startScenario.gameObject.SetActive(true);
        }
        else
        {
            btn_startScenario.gameObject.SetActive(false);
        }
    }
}
