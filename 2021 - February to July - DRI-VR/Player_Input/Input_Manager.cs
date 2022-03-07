using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script collects the player interaction during a scene
// *This script was created to organize the selction into a more modular script, so that it can be used to create the VR input

public class Input_Manager : MonoBehaviour
{
    protected bool _canUseScript;
    protected TimeSystem _timeSystem;

    [SerializeField]
    protected LayerMask hitboxMask;
    
    // There are different modes in how to handle the interaction, this tells the script which mode that would be
    public ModeStop CurrentMode
    {
        set { currentMode = value; }
    }
    private ModeStop currentMode;

    public virtual void Start()
    {
        // Find the time script that controls how long the scene will take
        _timeSystem = FindObjectOfType<TimeSystem>();
        if (_timeSystem == null)
        {
            Debug.LogError("The scene is missing a TimeSystem");
            _canUseScript = false;
        }
    }

    public virtual void Update()
    {
        // Check if in the correct mode
        if (_canUseScript || currentMode == null) return;

        // During the scene, when the player presses the button
        if(_timeSystem.IsRunning && Input.GetMouseButtonDown(0))
        {
            // What action to take for what mode
            if (currentMode is ModePlayerstop && !_timeSystem.IsTakingBreak)
            {
                _timeSystem.StartBreak();
            }
            else if (((currentMode is ModePlayerstop || currentMode is ModeSystemstop) && _timeSystem.IsTakingBreak) || currentMode is ModeNonStop)
            {
                SelectHitbox();
            }
        }
    }

    // What should the script do when selecting a box
    public virtual void SelectHitbox()
    {
        Hitbox box = null;

        // Detect if we can select item
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, hitboxMask))
        {
            Transform selection = hit.transform;
            box = selection.parent.GetComponent<Hitbox>();
        }

        // Send time system the possible found box (or null if not found)
        _timeSystem.Hit(box);
    }
}
