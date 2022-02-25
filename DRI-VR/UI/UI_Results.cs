using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The ui manager that shows the results after a scenario

public class UI_Results : MonoBehaviour
{
    // The texts that tell the user how many correct and wrong answers given
    [SerializeField] private Text txt_correct;
    [SerializeField] private Text txt_wrong;

    // Storing the global menu managers
    [SerializeField] UI_ChooseMode chooseMenu; // Basic menu
    [SerializeField] UI_Control ui_control; // Designed menu

    // The star that shows percentage on screen
    [SerializeField] Image img_Score;

    // Called at the end of the scenario
    public void ShowResults()
    {
        // Change how many correct/wrong answers were given
        txt_correct.text = "CORRECT: " + Scores.Correct;
        txt_wrong.text = "WRONG: " + Scores.Wrong;

        // Calculate percentage, show star image only for percentage
        float percentage = Scores.Correct / Scores.Total;
        img_Score.fillAmount = percentage;
    }

    // Called by button retry on screen
    public void Btn_Retry()
    {
        // When scene has designed menu
        if (chooseMenu != null)
        {
            chooseMenu.RetryMode();
            gameObject.SetActive(false);
        }
        // Else when scene has basic menu
        else if(ui_control != null) ui_control.RetryMode();
    }

    // Called by button change mode on screen
    public void Btn_ChangeMode()
    {
        // When scene has designed menu
        if (chooseMenu != null)
        {
            chooseMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        // Else when scene has basic menu
        else if (ui_control != null) ui_control.ChangeMode();
    }

    // Called by button return to main menu on screen
    public void Btn_MainMenu()
    {
        // When scene has designed menu
        if (chooseMenu != null)
        {
            chooseMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        // Else when scene has basic menu
        else if (ui_control != null) ui_control.MainMenu();
    }
}
