using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI manager for the ui shown during a scene

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;

    // Texts that tell the user how many correct and wrong answers were given
    [SerializeField] private Text txt_correct;
    [SerializeField] private Text txt_wrong;

    // Status on how well the user is doing with the scenario (good vs bad answers)
    [SerializeField] private GameObject status_poor;
    [SerializeField] private GameObject status_medium;
    [SerializeField] private GameObject status_good;
    [SerializeField] private GameObject status_perfect;

    // Slider that shows how much of the scenario has already played
    [SerializeField] private Slider sl_process;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        // Set status to poor
        SetStatus(0);
    }

    // Called when hitbox is hit
    public void UpdateUI()
    {
        // Change how many correct/wrong answers were given
        txt_correct.text = "CORRECT: " + Scores.Correct;
        txt_wrong.text = "WRONG: " + Scores.Wrong;

        // Calculate how many percentage the good answers are to the total answers given
        float percentage = Scores.Correct / Scores.Total;

        // Show the process and status on screen
        sl_process.value = percentage;
        SetStatus(percentage * 100);
    }

    private void SetStatus(float percentage)
    {
        // Disable all status bars
        status_poor.SetActive(false);
        status_medium.SetActive(false);
        status_good.SetActive(false);
        status_perfect.SetActive(false);

        // Enable the status bar belonging to the percentage
        if (percentage < 25) status_poor.SetActive(true);
        else if (percentage < 50) status_medium.SetActive(true);
        else if (percentage < 90) status_good.SetActive(true);
        else status_perfect.SetActive(true);
    }
}
