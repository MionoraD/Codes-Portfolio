using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageMainMenu : MonoBehaviour
{
    [SerializeField] private PlayersMenu playerMenu;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject homeScreen;

    [SerializeField] private Text warning;

    public void PlayGame()
    {
        if (playerMenu.CanStartGame())
        {
            if(FadeInOut.screen != null)
                FadeInOut.screen.FadeOut();
            else
            {
                homeScreen.SetActive(false);
                loadingScreen.SetActive(true);
            }

            if (ManagingScenes.scenes != null)
                ManagingScenes.scenes.StartLevel(1, 2);
            else
                Debug.Log("Open the game with NeverUnload Scene instead!");
        }
        else
        {
            warning.text = "Not all players have chosen their character yet";
        }
    }
}
