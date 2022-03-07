using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// This system controls the time during the scenario, the hitboxes respond to this system

public class TimeSystem : MonoBehaviour
{
    // There should only be one time system, so the system cannot cause confusion
    #region Global TimeSystem
    private static TimeSystem _instance;
    public static TimeSystem Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }
    #endregion

    // Variables to show the ui on screen during the video
    [SerializeField] private bool _hasUI = true;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Text txt_Timer;
    [SerializeField] private VideoPlayer _vPlayer;

    // The input managers of the system (normal or vr)
    [SerializeField] private Input_Manager input;
    [SerializeField] private VRInput_Manager vr_hand;
    [SerializeField] private LineRenderer hand_renderer;
    [SerializeField] private LaserPointer pointer;
    [SerializeField] private LineRenderer pointer_renderer;

    // The scenario that will be shown
    private Scenario currentScenario;
    // The ui that shows the feedback at the end of the scenario
    [SerializeField] private UI_Feedback ui_feedback;

    // Variables needed for timer
    public bool IsRunning
    {
        get { return (_timerIsRunning || _videoIsRunning); }
    }
    private bool _timerIsRunning = false;
    private bool _videoIsRunning = false;
    
    public bool IsTakingBreak
    {
        get { return _takebreak; }
    }
    private bool _takebreak = false;

    private List<Hitbox> hitboxlist_takingbreak = new List<Hitbox>();

    // To stop at this time
    private float _endTime = 120;

    // Variables needed to count time, other scripts can only read them
    public float CurrentSeconds
    {
        get { return _countSeconds; }
    }
    private float _countSeconds = 0;

    public TimeStamp Now
    {
        get { return _currentTime; }
    }
    private TimeStamp _currentTime = new TimeStamp(0, 0);

    public float TimeStep
    {
        get { return currentStep; }
    }
    private float currentStep = 0;

    // The ui menu and ui during the scenario
    [SerializeField] private UI_Control ui_control;
    [SerializeField] private UI_InGame ui_ingame;

    void Start()
    {
        // Disable the timer ui when opening the application
        if(_hasUI)
        {
            timerObject.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check when the timer is counting (during the video outside of a break)
        if((_timerIsRunning || _videoIsRunning) && !_takebreak)
        {
            // What was the last moment
            float lastcoundseconds = _countSeconds;

            // Add time that has happened since last moment (video misbehaves, and thus never truly matches deltatime)
            if (_videoIsRunning) _countSeconds = (float)_vPlayer.time;
            if(_timerIsRunning) _countSeconds += Time.deltaTime;

            // How much time has passed
            currentStep = _countSeconds - lastcoundseconds;

            // Create a timestamp, class found at 249
            _currentTime = new TimeStamp(_countSeconds);

            // Update the time ui to the current time
            if (_hasUI) txt_Timer.text = _currentTime.ToString();

            // Check if end time is reached
            if(_countSeconds >= _endTime || _videoIsRunning && _countSeconds >= (_endTime - 1))
            {
                Debug.Log("Reached end video");

                // Scenario no longer running
                _timerIsRunning = false;
                _videoIsRunning = false;

                // Disable time ui
                if (_hasUI)
                {
                    timerObject.gameObject.SetActive(false);
                }

                // Show the results at the end of the video
                if (ui_control != null)
                {
                    ui_control.gameObject.SetActive(true);
                    ui_control.ShowResults(currentScenario);
                }

                // Disable the ui shown during the scene
                if (ui_ingame != null)
                {
                    ui_ingame.gameObject.SetActive(false);
                }

                // If the interaction system has vr input disable this input
                if(vr_hand != null)
                {
                    vr_hand.enabled = false;
                    hand_renderer.enabled = false;
                    pointer.enabled = true;
                    pointer_renderer.enabled = true;
                }
            }
        }
    }

    // Called from menu when the user starts the scenario (when not showing a video)
    public void StartTime(Scenario thisScenario, ModeStop modeStop, ModeFeedback modeFeedback)
    {
        // Start timer for 60 seconds
        StartTimer(60);
        _timerIsRunning = true;

        // Set the scenario and modes given
        ResetScenario(thisScenario, modeStop, modeFeedback);
    }

    // Called from menu when the user starts the scenario (when showing a video)
    public void StartVideo(Scenario thisScenario, ModeStop modeStop, ModeFeedback modeFeedback)
    {
        // Stop the video player, set new video, start playing the new video (stop the video to start the new video from the beginning)
        _vPlayer.Stop();
        _vPlayer.clip = thisScenario.Clip;
        _vPlayer.Play();

        // Start the timer for the scenario time
        StartTimer(thisScenario.Time);
        _videoIsRunning = true;

        // Set the scenario and modes given
        ResetScenario(thisScenario, modeStop, modeFeedback);
    }

    // Called at the start of the scene, so that all variables start at zero
    public void ResetScenario(Scenario scenario, ModeStop modeStop, ModeFeedback modeFeedback)
    {
        // Enable input manager & tell them what modes to use
        if (input != null) input.CurrentMode = modeStop;
        else if (vr_hand != null)
        {
            vr_hand.enabled = true;
            hand_renderer.enabled = true;
            vr_hand.CurrentMode = modeStop;
            pointer.enabled = false;
            pointer_renderer.enabled = false;
        }

        // Set the current scenario, and set the modes to that scenario
        currentScenario = scenario;
        currentScenario.StartScenario(modeStop, modeFeedback);

        // Set the feedback mode
        ui_feedback.CurrentMode = modeFeedback;
        if (modeFeedback is ModeFeedbackDuring) ui_ingame.gameObject.SetActive(true);

        // Start at 0 seconds
        _countSeconds = 0;
    }

    // Called when the video is taking a break, could be called by input managers or by hitbox (depending on the mode)
    public void StartBreak()
    {
        _takebreak = true;

        // Find which hitboxes which can take a break
        hitboxlist_takingbreak = new List<Hitbox>();

        Hitbox[] list = GameObject.FindObjectsOfType<Hitbox>();
        foreach(Hitbox item in list)
        {
            if (!item.TakenBreak)
            {
                Debug.Log(item.name + " is taking break");
                hitboxlist_takingbreak.Add(item);
            }
        }

        // Pause the video when player is showing a video
        if (_videoIsRunning) _vPlayer.Pause();
    }

    // Hitting a hitbox
    public void Hit(Hitbox hitbox)
    {
        // When the system takes a break in the mode (by input manager or hitbox)
        if(IsTakingBreak)
        {
            // Hit the box when the box is taking a break
            foreach(Hitbox box in hitboxlist_takingbreak)
            {
                Debug.Log("In the break " + box.name);
                if (box == hitbox) hitbox.HitThisBox();
                else box.MissedBox();
            }

            // Stop taking a break, start playing the video
            _takebreak = false;
            if (_videoIsRunning) _vPlayer.Play();
        }
        else
        {
            // Hit the box in other modes
            if (hitbox != null) hitbox.HitThisBox();
        }
    }

    // Start the timer without video
    private void StartTimer(float endTime)
    {
        // Show the timer ui
        if (_hasUI)
        {
            timerObject.gameObject.SetActive(true);
        }

        _countSeconds = 0;
        _endTime = endTime;
    }
}

// Class to store the current time
[System.Serializable]
public class TimeStamp
{
    // The minutes
    [SerializeField]
    private int _minutes;
    public int Min
    {
        get { return _minutes; }
    }

    // The seconds
    [SerializeField]
    private float _seconds;
    public float Sec
    {
        get { return _seconds; }
    }

    // Get the time one second later
    public TimeStamp GetLater()
    {
        return new TimeStamp(_minutes, _seconds++);
    }

    // Constructors
    public TimeStamp() { }

    public TimeStamp(int min, float sec)
    {
        _minutes = min;
        _seconds = sec;
    }

    public TimeStamp(float seconds)
    {
        _minutes = Mathf.FloorToInt(seconds / 60);
        _seconds = seconds % 60;
    }

    // Create a string from the time
    public override string ToString()
    {
        float hundredths = (_seconds * 100) % 100;
        return string.Format("{0:00}:{1:00}:{2:00}", _minutes, _seconds, hundredths);
    }

    // Check if the given timestamp is before the current timestamp
    public bool IsBefore(TimeStamp time)
    {
        return _minutes < time.Min || _minutes == time.Min && _seconds <= time.Sec;
    }

    // Give time in seconds
    public float GetTimeInSeconds()
    {
        return (Min * 60) + Sec;
    }

    // Calculate how much seconds there are between two timestamps
    public static float TimeBetween(TimeStamp start, TimeStamp end)
    {
        float startSeconds = start.GetTimeInSeconds();
        float endSeconds = end.GetTimeInSeconds();

        return endSeconds - startSeconds;
    }
}
