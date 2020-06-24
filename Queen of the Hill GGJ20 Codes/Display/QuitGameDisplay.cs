using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameDisplay : MonoBehaviour
{
    Button btnQuit;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnQuit = gameObject.GetComponent<Button>();
        btnQuit.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
