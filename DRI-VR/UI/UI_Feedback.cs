using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI manager that handles the feedback given when hitbox is selected

public class UI_Feedback : MonoBehaviour
{
    // Storing which mode the feedback is shown in (during or after the scene)
    public ModeFeedback CurrentMode
    {
        set { currentMode = value; }
    }
    private ModeFeedback currentMode = null;

    // Positive feedback ui
    [SerializeField] private GameObject correct_field;
    [SerializeField] private Text txt_correct;
    // Negative feedback ui
    [SerializeField] private GameObject wrong_field;
    [SerializeField] private Text txt_wrong;

    // Storing what ui is currently showing (positive or negative feedback)
    private GameObject currentMessage;

    // Variables to show the message only for a set time
    private float time = 0;
    private float duration = 5;
    private bool isShowingMsg = false;

    void Start()
    {
        // Disable both correct and wrong ui fields
        correct_field.SetActive(false);
        wrong_field.gameObject.SetActive(false);
    }

    // Called when hitbox is selected
    public void ShowResult(bool isCorrect, string explanation)
    {
        // When mode is show feedback during
        if (currentMode is ModeFeedbackDuring)
        {
            Debug.Log("Result " + isCorrect + " " + explanation);
            if (isShowingMsg && isCorrect) return;

            // Disable current message, when message is already showing
            if (currentMessage != null) currentMessage.SetActive(false);

            // When correct, current message is correct ui
            if (isCorrect)
            {
                currentMessage = correct_field;
                txt_correct.text = explanation;
            }
            // When wrong, current message is wrong ui
            else
            {
                currentMessage = wrong_field;
                txt_wrong.text = explanation;
            }

            // Set current message active, start timer
            currentMessage.SetActive(true);
            time = 0;
            isShowingMsg = true;
        }
    }

    void Update()
    {
        // When showing message
        if(isShowingMsg)
        {
            // Add time since last update
            time += Time.deltaTime;

            // If the time has taken the duration, stop showing message
            if(time >= duration)
            {
                currentMessage.SetActive(false);
                isShowingMsg = false;
            }
        }
    }
}
