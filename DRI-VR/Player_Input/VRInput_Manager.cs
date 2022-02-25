using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// This script handles the input from the vr controller

public class VRInput_Manager : MonoBehaviour
{
    protected bool _canUseScript;
    protected TimeSystem _timeSystem;

    [SerializeField]
    protected LayerMask hitboxMask;

    // Stores which mode the current scene is using in selecting the hitboxes
    public ModeStop CurrentMode
    {
        set { currentMode = value; }
    }
    private ModeStop currentMode;

    // The line painted by the hand
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected float lineWidth = 0.1f;
    [SerializeField] protected float lineMaxLength = 1f;

    private Vector3 endPosition = new Vector3();

    void Start()
    {
        // Find the time system
        _timeSystem = FindObjectOfType<TimeSystem>();
        if (_timeSystem == null)
        {
            Debug.LogError("The scene is missing a TimeSystem");
            _canUseScript = false;
        }

        // Start painting the line into the scene
        Vector3[] startLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        lineRenderer.SetPositions(startLinePositions);
        lineRenderer.enabled = true;
    }

    void Update()
    {
        if (_canUseScript || currentMode == null) return;

        // When the scenario is playing
        if (_timeSystem.IsRunning)
        {
            // Update the input
            OVRInput.Update();

            // Get btn values
            bool btn_trigger = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
            bool btn_xbtn = OVRInput.Get(OVRInput.Button.One);
            
            // Find the end position of the pointer
            endPosition = transform.position + (lineMaxLength * transform.forward);

            // When taking a break during the modes in which the system takes a break
            if(currentMode is ModePlayerstop || currentMode is ModeSystemstop)
            {
                // Enable line during break
                if(_timeSystem.IsTakingBreak)
                {
                    lineRenderer.enabled = true;
                }
                // Disable line outside of break
                else
                {
                    lineRenderer.enabled = false;
                }
            }

            // If taking a break button is used, take a break in the right mode, when not taking a break
            if(btn_xbtn)
            {
                if(currentMode is ModePlayerstop && !_timeSystem.IsTakingBreak)
                {
                    _timeSystem.StartBreak();
                }
            }

            // If select button is used
            if(btn_trigger)
            {
                // When taking break in certain modes
                if ((currentMode is ModePlayerstop || currentMode is ModeSystemstop) && _timeSystem.IsTakingBreak)
                {
                    SelectHitbox();
                }
                // In other modes
                else if (currentMode is ModeNonStop)
                {
                    SelectHitbox();
                }
            }

            // Draw line in scene
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPosition);
        }
        else
        {
            // Disable line outside of scenario
            lineRenderer.enabled = false;
        }
    }

    // Called when selecting video
    public void SelectHitbox()
    {
        Hitbox box = null;

        // Detect if we can select item
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500, hitboxMask))
        {
            endPosition = hit.point;

            Transform selection = hit.transform;
            box = selection.parent.GetComponent<Hitbox>();
        }

        // Tell time system which box is hit
        _timeSystem.Hit(box);
    }
}
