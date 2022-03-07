using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI manager to handle the scenario menu

public class UI_SelectScenario : MonoBehaviour
{
    // Where the scenarios are stored
    [SerializeField] private Transform parent_Scenarios;
    private Scenario[] scenarios;

    // Creating buttons for each scenario that have been stored
    [SerializeField] private Transform parent_MenuScenarios;
    [SerializeField] private GameObject prefab_menuItem_Scenario;

    void Start()
    {
        // Find all scenarios
        scenarios = parent_Scenarios.GetComponentsInChildren<Scenario>();

        // Create a scenario button for each scenario
        foreach(Scenario item in scenarios)
        {
            GameObject menuItem = Instantiate(prefab_menuItem_Scenario, parent_MenuScenarios);
            UI_ScenarioItem scenarioItem = menuItem.GetComponent<UI_ScenarioItem>();
            scenarioItem.SetScenario(item);
        }
    }
}
