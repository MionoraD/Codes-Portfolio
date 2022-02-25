using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to handle 'UI Scenario button' shown on the select scenario menu screen

public class UI_ScenarioItem : MonoBehaviour
{
    // The scenario that this item belongs to 
    private Scenario thisScenario;

    // The visuals shown on screen
    [SerializeField] private Text txt_title;
    [SerializeField] private Text txt_description;
    [SerializeField] private Image bg_image;

    // Set the scenario of this item, called by the UI_SelectScenario script
    public void SetScenario(Scenario scenario)
    {
        // Store scenario
        thisScenario = scenario;

        // Set visuals of item
        txt_title.text = thisScenario.Name;
        txt_description.text = thisScenario.Description;
        bg_image = thisScenario.Img;
    }

    // Called when item is pressed
    public void ChooseScenario()
    {
        // Find UI manager to select this scenario to
        UI_Control ui_control = GameObject.FindObjectOfType<UI_Control>();
        if(ui_control != null)
        {
            ui_control.ChooseScenario(thisScenario);
        }
    }
}
