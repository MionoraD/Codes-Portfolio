using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the hitbox

public class Hitbox : MonoBehaviour
{
    private bool _canUseScript = true;
    private TimeSystem _timeSystem;
    [HideInInspector] public bool IsPlaying = false;

    [Header("The hitbox")]
    [SerializeField] private Result _boxResult; // script that shows the user when the hibtox has been hit

    // Whether this hitbox should be hit or missed
    public bool ShouldHit
    {
        get { return hit_this_box; }
    }
    [SerializeField] private bool hit_this_box = true;

    // The feedback that should be given when the box is hit
    [SerializeField] private string hitbox_feedback = "There is something to be said about hitting this box";

    // For the mode in which the video is stopped by the system
    [Header("Stop the video at time")]
    [SerializeField]
    private HitboxPosition stopVideoAt = new HitboxPosition();

    // For all the other modes
    [Header("Hitbox positions")]
    [SerializeField]
    private List<HitboxPosition> _positionList = new List<HitboxPosition>();

    private int _index = 0;

    // To store where the hitbox is currently at, find the class at line 242
    private HitboxPosition CurrentPosition
    {
        set
        {
            _currentPositon = value;
            _index++;
        }
    }
    private HitboxPosition _currentPositon = null;
    
    // To store where the hitbox will go next, find the class at line 242
    private HitboxPosition NextPosition
    {
        set
        {
            _nextPosition = value;

            // Reset time stuff
            timeToReach = HitboxPosition.SecondsBetween(_currentPositon, _nextPosition);
            timestep = 0;
        }
    }
    private HitboxPosition _nextPosition = null;

    private float timestep = 0;
    private float timeToReach = 0;

    // Storing when it has hit (so the user can not hit it again)
    public bool HasBeenHit
    {
        get { return hasbeenselected; }
    }
    private bool hasbeenselected = false;

    // To store which mode it will be using
    private ModeStop modeStop = null;

    // For the modes in which the user takes a break before or after the hitbox has been hit
    public bool TakenBreak
    {
        get { return hastakenbreak; }
    }
    private bool hastakenbreak = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the time system that tracks time during the scene
        _timeSystem = FindObjectOfType<TimeSystem>();
        if(_timeSystem == null)
        {
            Debug.LogError("The scene is missing a TimeSystem");
            _canUseScript = false;
        }

        // Check if this hitbox has a result script
        if(_boxResult == null)
        {
            Debug.LogError(name + " has no box attached to it");
            _canUseScript = false;
        }
        else
        {
            _boxResult.gameObject.SetActive(false);
        }

        // Make sure that the hitbox has a starting and ending position
        if(_positionList.Count <= 0)
        {
            Debug.LogError(name + " has no positions attached to it");
            _canUseScript = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Stop this script when error or scene is not running
        if (!_canUseScript || !_timeSystem.IsRunning) return;

        // Check which mode is running during the scene
        if(modeStop is ModeSystemstop)
        {
            CheckSystemStop();
        }
        else
        {
            SetHitboxAtPosition();
        }
    }

    // When the system stops the video
    private void CheckSystemStop()
    {
        // Check if the time of the position has passed (cannot hit the exact frame because sometimes the video skips over that frame)
        if (stopVideoAt != null && stopVideoAt.TimePassed(_timeSystem.Now))
        {
            // when the system has not yet taken the break
            if (!hastakenbreak)
            {
                _timeSystem.StartBreak();
                transform.position = stopVideoAt.Pos;
                _boxResult.gameObject.SetActive(true);
                hastakenbreak = true;
            }
            // What happens after the break has been taken
            else if (!_timeSystem.IsTakingBreak)
            {
                // Show message that the box has been missed
                MissedBox();
                _boxResult.gameObject.SetActive(false);
            }
        }
    }

    // In every other mode the user has to click on this box
    private void SetHitboxAtPosition()
    {
        // Check when the hitbox can be hit
        if (_timeSystem.IsTakingBreak) return;

        // Find out which position the hitbox is at
        if (_currentPositon == null)
        {
            _index = 0;

            // Set the current and next positions
            CurrentPosition = _positionList[_index];
            NextPosition = _positionList[_index];

            // Set the hitbox to currentposition
            transform.position = _currentPositon.Pos;
        }
        else if (_nextPosition == null) return;
        else if (_nextPosition.TimePassed(_timeSystem.Now))
        {
            // When the system has gotten passed the next position, set next to current
            CurrentPosition = _nextPosition;

            // Find next position on new index (new index set by changing the current position, see line 39)
            if (_index >= _positionList.Count)
            {
                if (!hasbeenselected) MissedBox();
                _boxResult.gameObject.SetActive(false);
                _nextPosition = null;
            }
            else NextPosition = _positionList[_index];
        }
        // When not passed the next position, but passed the current position
        else if (_currentPositon.TimePassed(_timeSystem.Now))
        {
            // Make the hitbox active (for when the currentposition is the starting position)
            _boxResult.gameObject.SetActive(true);

            // Make the hitbox move to the next frame in the scene
            timestep += _timeSystem.TimeStep / timeToReach;
            transform.position = Vector3.Lerp(_currentPositon.Pos, _nextPosition.Pos, timestep);
        }
    }

    // Called when the scene is started to set the modes that are being used
    public void StartHitbox(ModeStop mStop, ModeFeedback mFeedback)
    {
        // Set the mode
        modeStop = mStop;

        // Reset all the variables neccessary
        _currentPositon = null;
        _nextPosition = null;

        hasbeenselected = false;
        hastakenbreak = false;

        // Tell the result script which feedback mode to use
        _boxResult.ResetResults(mFeedback);
    }

    // When this box is hit
    public void HitThisBox()
    {
        // Tell the result whether this box should have been hit or not
        if (hit_this_box) _boxResult.IsCorrect(hitbox_feedback);
        else _boxResult.IsWrong(hitbox_feedback);

        // Store that this box has been hit
        hasbeenselected = true;
    }

    // When this box has been missed by the user
    public void MissedBox()
    {
        // Commented out because we decided not to show this anymore

        //Debug.Log("Missed " + hitbox_feedback);
        //if (hit_this_box) _box.IsWrong(hitbox_feedback);
        //else _box.IsCorrect(hitbox_feedback);

        //hasbeenselected = false;
    }
}

// The class that is storing a position at which the hitbox can be at
[System.Serializable]
public class HitboxPosition
{
    // The time that the hitbox should be at this position
    [SerializeField]
    private TimeStamp _time;
    public TimeStamp TimePosition
    {
        get { return _time; }
    }

    // The actual position that the hitbox should be at
    [SerializeField]
    private Vector3 _position;
    public Vector3 Pos
    {
        get { return _position; }
    }

    // Check if the given time is passed the timestamp of this position
    public bool TimePassed(TimeStamp time)
    {
        return _time.IsBefore(time);
    }

    // Calculate how much time in between positions
    public static float SecondsBetween(HitboxPosition startPos, HitboxPosition endPos)
    {
        return TimeStamp.TimeBetween(startPos.TimePosition, endPos.TimePosition);
    }
}