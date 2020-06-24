using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private GameManager gmc;

    [SerializeField] private Button startgame;
    [SerializeField] private InputField namefield;

    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        startgame.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        gmc.LoadLevel(namefield.text);
    }
}
