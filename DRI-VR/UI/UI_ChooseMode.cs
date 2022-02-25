using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// UI manager that handles the user selecting the mode for the scenario
// * This is an earlier version that is used for a basic menu, later on this had to be adapted to the designed menu into the UI_Control

public class UI_ChooseMode : MonoBehaviour
{
    private bool _canUseScript = true;
    private TimeSystem time;

    // Toggle if the scenerario will use the video or not (for performing purposes during testing)
    [SerializeField] private Toggle toggle_video;
    [SerializeField] private float time_duration;

    // Toggle to show feedback during the video
    [SerializeField] private Toggle toggle_feedback;

    // List of scenarios, so the user can choose a scenario
    [SerializeField] private GameObject parent_Scenarios;
    private Scenario[] scenarios;
    [SerializeField] private Dropdown dropdown_scenarios;

    // Storing the modes that are chosen
    private ModeStop modeStop = null;
    private ModeFeedback modeFeedback = null;

    void Start()
    {
        // Find the time system
        time = FindObjectOfType<TimeSystem>();
        if (time == null)
        {
            Debug.LogError("The scene is missing a TimeSystem");
            _canUseScript = false;
        }

        // Find the scenarios in the scene
        scenarios = parent_Scenarios.GetComponentsInChildren<Scenario>();

        // Create dropdown list for the scenarios
        List<string> options = new List<string>();
        foreach (var option in scenarios)
        {
            options.Add(option.name);
        }
        dropdown_scenarios.ClearOptions();
        dropdown_scenarios.AddOptions(options);
    }

    // Pressing one of the mode buttons
    private void StartTimer(ModeStop mStop)
    {
        if (!_canUseScript) return;

        // Store the mode
        modeStop = mStop;

        // Set the feedback mode according to the toggle
        if (toggle_feedback.isOn) modeFeedback = new ModeFeedbackDuring();
        else modeFeedback = new ModeFeedbackAfter();

        // Find which scenario is selected
        int index = dropdown_scenarios.value;
        Scenario thisScenario = scenarios[index];

        // Start the time system on video or time, based on the video toggle
        if (toggle_video.isOn) time.StartVideo(thisScenario, modeStop, modeFeedback);
        else time.StartTime(thisScenario, modeStop, modeFeedback);

        gameObject.SetActive(false);
    }

    // Choosing for the mode in which the player stops the video during the scene
    public void StartPlayerStopMode()
    {
        ModeStop thisMode = new ModePlayerstop();
        StartTimer(thisMode); // Line 54
    }

    // Choosing for the mode in which the system stops the video during the scene
    public void StartSystemStopMode()
    {
        ModeStop thisMode = new ModeSystemstop();
        StartTimer(thisMode); // Line 54
    }

    // Choosing for the mode in which the scene is not stopped
    public void StartNonStopMode()
    {
        ModeStop thisMode = new ModeNonStop();
        StartTimer(thisMode); // Line 54
    }

    // Use the same mode as used before
    public void RetryMode()
    {
        if(modeStop != null) StartTimer(modeStop); // Line 54
    }
}
