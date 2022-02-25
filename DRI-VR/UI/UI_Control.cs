using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI manager that handles the user selecting the mode for the scenario

public class UI_Control : MonoBehaviour
{
    private bool _canUseScript = true;
    private TimeSystem time;
    [SerializeField] private bool testVideo = true;

    // The different screens of the menu
    [SerializeField] private GameObject screen_selectScenario;
    [SerializeField] private UI_SelectMode screen_selectMode;
    [SerializeField] private UI_Results screen_results;

    // Storing the scenario and modes that were selected in the menus
    private Scenario thisScenario = null;
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
    }

    // For the quit button in the menu
    public void QuitApp()
    {
        Application.Quit();
    }

    // UI_ScenarioItem uses this to select the scenario chosen by the user
    public void ChooseScenario(Scenario newScenario)
    {
        // Store scenario
        thisScenario = newScenario;

        // Set scenario, enable the mode menu
        screen_selectMode.SetScenario(thisScenario);
        screen_selectMode.gameObject.SetActive(true);
    }

    // UI_SelectMode uses this to select the modes chosen by the user
    public void SelectMode(ModeStop mStop, ModeFeedback mFeedback)
    {
        if (!_canUseScript) return;

        // Storing the modes
        modeStop = mStop;
        modeFeedback = mFeedback;

        // Start the time system on video or time, based on the video toggle
        if (testVideo) time.StartVideo(thisScenario, mStop, mFeedback);
        else time.StartTime(thisScenario, mStop, mFeedback);

        gameObject.SetActive(false);
    }

    // Called at the end of the scenario to show the results screen
    public void ShowResults(Scenario thisScenario)
    {
        screen_results.gameObject.SetActive(true);
        screen_results.ShowResults();
    }

    // UI_Results uses this to retry the same scenario with the same modes
    public void RetryMode()
    {
        // When scenarios and modes are stored
        if (thisScenario != null && modeStop != null && modeFeedback != null)
        {
            // Disable results screen, and recall scenario 
            screen_results.gameObject.SetActive(false);
            SelectMode(modeStop, modeFeedback); // line 52
        }
    }

    // UI_Results uses this to retry the same scenario with different modes
    public void ChangeMode()
    {
        screen_results.gameObject.SetActive(false);
    }

    // UI_Results uses this to go to the main menu
    public void MainMenu()
    {
        screen_results.gameObject.SetActive(false);
        screen_selectMode.gameObject.SetActive(false);
        screen_selectScenario.SetActive(false);
    }
}
