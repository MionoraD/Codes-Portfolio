using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script handles how the result will be shown to the user during the scene

public class Result : MonoBehaviour
{
    // At some point, materials were used to show the results
    [SerializeField] private Material m_standard;
    [SerializeField] private Renderer box_renderer;
    [SerializeField] private Material m_correct;
    [SerializeField] private Material m_wrong;

    // When the result became a gameobject of its own
    [Header("Feedback UI")] 
    [SerializeField] private GameObject canvas_explanation;

    // Which gameobject shows the positive feedback
    [SerializeField] private GameObject ui_correct;
    [SerializeField] private Text txt_correct;

    // Which gameobject shows the negative feedback
    [SerializeField] private GameObject ui_wrong;
    [SerializeField] private Text txt_wrong;

    private bool _hasResult = false;

    // Which mode of feedback is being used during the scene
    private ModeFeedback modeFeedback;

    // Variables to show the feedback only for so much time
    private float time = 0;
    private float duration = 5;
    private bool isShowingMsg = false;

    void Start()
    {
        // Find the renderer to change the material later on
        if(box_renderer != null)
            m_standard = box_renderer.material;

        // Disable the feedback at the start
        canvas_explanation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // When showing the message
        if (isShowingMsg)
        {
            // Find how much time has passed since start
            time += Time.deltaTime;

            // If time is longer than the feedback should be shown
            if (time >= duration)
            {
                canvas_explanation.SetActive(false);
                isShowingMsg = false;
            }
        }
    }

    // When the user was wrong
    public void IsWrong(string explanation)
    {
        if (_hasResult) return;

        // Tell the score that the user scored a wrong answer
        Scores.AddWrong();
        _hasResult = true;

        // Stop here when feedback is shown only after the scene
        if (modeFeedback is ModeFeedbackAfter) return;

        // If renderer found, set material to wrong
        if (box_renderer != null)
            box_renderer.material = m_wrong;

        // Disable correct feedback
        ui_correct.SetActive(false);

        // Enable wrong feedback
        ui_wrong.SetActive(true);
        txt_wrong.text = explanation;

        // Show feedback
        canvas_explanation.SetActive(true);

        time = 0;
        isShowingMsg = true;
    }

    // When the user was correct
    public void IsCorrect(string explanation)
    {
        if (_hasResult) return;

        // Tell the score that the user scored a correct answer
        Scores.AddCorrect();
        _hasResult = true;

        // Stop here when feedback is shown only after the scene
        if (modeFeedback is ModeFeedbackAfter) return;

        // If renderer found, set material to correct
        if (box_renderer != null)
            box_renderer.material = m_correct;

        // Disable wrong feedback
        ui_wrong.SetActive(false);

        // Enable correct feedback
        ui_correct.SetActive(true);
        txt_correct.text = explanation;

        // Show feedback
        canvas_explanation.SetActive(true);

        time = 0;
        isShowingMsg = true;
    }

    // Reset the result at the start of the scene
    public void ResetResults(ModeFeedback mFeedback)
    {
        if (box_renderer == null) return;

        // Set new feedback mode
        modeFeedback = mFeedback;

        // Set material of box to standard
        if (box_renderer != null)
            box_renderer.material = m_standard;

        _hasResult = false;
    }
}
