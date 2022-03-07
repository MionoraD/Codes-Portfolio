using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btn_Hitbox : MonoBehaviour
{
    // Earlier used script for hitting the hitbox

    protected bool _canUseScript;
    protected TimeSystem _timeSystem;
    private Transform _player;

    // To which box does this script belong
    public Hitbox Box
    {
        set { thisBox = value; }
    }
    private Hitbox thisBox;

    // Which mode is this script using
    public ModeStop CurrentMode
    {
        set { currentMode = value; }
    }
    private ModeStop currentMode;

    // Start is called before the first frame update
    void Start()
    {
        // Find the time system
        _timeSystem = FindObjectOfType<TimeSystem>();
        if (_timeSystem == null)
        {
            Debug.LogError("The scene is missing a TimeSystem");
            _canUseScript = false;
        }

        // Find the player in the scene
        _player = GameObject.FindGameObjectWithTag("player").transform;
        if(_player == null)
        {
            Debug.LogError("Missing player tag in the scene");
            _canUseScript = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Make the button turn to player
        transform.LookAt(_player);
    }

    // What should happen is box is hit
    public void HitThisBox()
    {

    }
}
