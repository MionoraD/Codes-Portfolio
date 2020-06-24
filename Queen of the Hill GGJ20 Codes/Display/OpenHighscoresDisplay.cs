using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenHighscoresDisplay : MonoBehaviour
{
    Button btnOpenScreen;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnOpenScreen = gameObject.GetComponent<Button>();
        btnOpenScreen.onClick.AddListener(LoadHighscoreScreen);

        gmc = FindObjectOfType<GameManager>();
    }

    void LoadHighscoreScreen()
    {
        gmc.LoadHighScoreScreen();
    }
}
